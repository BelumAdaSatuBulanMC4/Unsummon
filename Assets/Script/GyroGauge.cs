using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GyroGaugeController : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeNeedle;
    [SerializeField] private RectTransform gaugeBackground;
    [SerializeField] private RectTransform gaugeRandomTarget;
    [SerializeField] private Slider slider;

    public float sensitivity = 5.0f;
    public float minPitch = -0.5f;
    public float maxPitch = 0.5f;

    private float minPositionX;
    private float maxPositionX;

    [SerializeField] private TMP_Text rollVal;
    [SerializeField] private TMP_Text pitchVal;
    [SerializeField] private TMP_Text yawVal;
    [SerializeField] private TMP_Text feedbackText; // TMP_Text to display "Correct"

    private SwiftPlugin swiftPlugin;
    private float timeSinceLastUpdate = 0f;
    private float randomTargetWidth;
    private Vector2 randomTargetPosition;

    private void Awake()
    {
        swiftPlugin = GetComponent<SwiftPlugin>();
    }

    private void Start()
    {
        float halfNeedleWidth = gaugeNeedle.rect.width / 2;
        minPositionX = gaugeBackground.rect.xMin + halfNeedleWidth;
        maxPositionX = gaugeBackground.rect.xMax - halfNeedleWidth;

        swiftPlugin.StartGyro();

        // Initialize the random target
        UpdateRandomTarget();
    }

    private void OnDestroy()
    {
        swiftPlugin.StopGyro();
    }

    private void Update()
    {
        // Retrieve roll, pitch, and yaw values
        double roll = swiftPlugin.GetRollValue();
        double pitch = swiftPlugin.GetPitchValue();
        double yaw = swiftPlugin.GetYawValue();

        // Display values in the UI
        rollVal.text = roll.ToString("F2");
        pitchVal.text = pitch.ToString("F2");
        yawVal.text = yaw.ToString("F2");

        // Calculate the normalized pitch so that 0 pitch is in the center
        float normalizedPitch = Mathf.InverseLerp(minPitch, maxPitch, (float)pitch);

        // Calculate the target x position for the gauge needle
        float targetPositionX = Mathf.Lerp(minPositionX, maxPositionX, normalizedPitch);

        // Clamp the needle position to ensure it stays within the bounds
        targetPositionX = Mathf.Clamp(targetPositionX, minPositionX, maxPositionX);

        // Apply the new position to the gauge needle
        gaugeNeedle.anchoredPosition = new Vector2(targetPositionX, gaugeNeedle.anchoredPosition.y);

        // Set the slider value to the midpoint if pitch is 0
        float targetValue = Mathf.Lerp(slider.minValue, slider.maxValue, normalizedPitch);
        slider.value = Mathf.Clamp(targetValue, slider.minValue, slider.maxValue);

        // Update the random target every 5 seconds
        timeSinceLastUpdate += Time.deltaTime;
        if (timeSinceLastUpdate >= 5f)
        {
            UpdateRandomTarget();
            timeSinceLastUpdate = 0f;
        }

        // Check if the gauge needle is within the gaugeRandomTarget range
        if (targetPositionX >= gaugeRandomTarget.anchoredPosition.x - gaugeRandomTarget.rect.width / 2 &&
            targetPositionX <= gaugeRandomTarget.anchoredPosition.x + gaugeRandomTarget.rect.width / 2)
        {
            feedbackText.text = "Correct";
        }
        else
        {
            feedbackText.text = ""; // Clear the text if not within range
        }
    }

    private void UpdateRandomTarget()
    {
        // Calculate max width for the gaugeRandomTarget (no more than half width of gaugeBackground)
        float maxTargetWidth = gaugeBackground.rect.width / 2;
        randomTargetWidth = Random.Range(20f, maxTargetWidth); // Width range from 20f to maxTargetWidth
        gaugeRandomTarget.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, randomTargetWidth);

        // Calculate random position within the bounds of the gaugeBackground
        float maxPosition = maxPositionX - randomTargetWidth;
        float minPosition = minPositionX;
        float randomXPosition = Random.Range(minPosition, maxPosition);

        // Update the gaugeRandomTarget position
        gaugeRandomTarget.anchoredPosition = new Vector2(randomXPosition, gaugeRandomTarget.anchoredPosition.y);
    }
}
