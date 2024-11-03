using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroGaugeController : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeNeedle;
    [SerializeField] private RectTransform gaugeBackground;
    [SerializeField] private RectTransform gaugeRandomTarget;
    [SerializeField] private Slider slider;

    public float sensitivity = 5.0f;
    public float minPitch = -0.5f;
    public float maxPitch = 0.5f;
    public float animationSpeed = 2.0f; // Speed for the smooth animation
    public float progressSpeed = 0.1f; // Initial speed of progress increase/decrease

    private float minPositionX;
    private float maxPositionX;

    [SerializeField] private TMP_Text rollVal;
    [SerializeField] private TMP_Text pitchVal;
    [SerializeField] private TMP_Text yawVal;
    [SerializeField] private TMP_Text feedbackText; // TMP_Text to display "Correct"

    private SwiftPlugin swiftPlugin;
    private Vector2 targetPosition; // Target position for smooth movement of gaugeRandomTarget
    private bool isGaugeCorrect;

    [SerializeField] private Slider progressBar; // Slider for the progress bar

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

        // Set initial target position
        SetRandomTargetPosition();

        // Set initial progress bar value to 50%
        progressBar.value = 0.5f;

        // Start coroutines to update the target position and increase progress speed
        StartCoroutine(UpdateRandomTargetPositionRoutine());
        StartCoroutine(IncreaseProgressSpeedRoutine());
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

        // Smoothly move the gaugeRandomTarget to the target position
        gaugeRandomTarget.anchoredPosition = Vector2.Lerp(gaugeRandomTarget.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);

        // Check if the gauge needle is within the gaugeRandomTarget range
        if (targetPositionX >= gaugeRandomTarget.anchoredPosition.x - gaugeRandomTarget.rect.width / 2 &&
            targetPositionX <= gaugeRandomTarget.anchoredPosition.x + gaugeRandomTarget.rect.width / 2)
        {
            feedbackText.text = "Correct";
            isGaugeCorrect = true;
        }
        else
        {
            feedbackText.text = ""; // Clear the text if not within range
            isGaugeCorrect = false;
        }

        // Update the progress bar based on isGaugeCorrect
        UpdateProgressBar();
    }

    private IEnumerator UpdateRandomTargetPositionRoutine()
    {
        while (true)
        {
            SetRandomTargetPosition();
            yield return new WaitForSeconds(Random.Range(1f, 2f)); // Random delay between position changes
        }
    }

    private IEnumerator IncreaseProgressSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            progressSpeed += 0.1f;
        }
    }

    private void SetRandomTargetPosition()
    {
        // Calculate random position within the bounds of the gaugeBackground
        float maxPosition = maxPositionX - gaugeRandomTarget.rect.width / 2;
        float minPosition = minPositionX + gaugeRandomTarget.rect.width / 2;
        float randomXPosition = Random.Range(minPosition, maxPosition);

        // Set the target position
        targetPosition = new Vector2(randomXPosition, gaugeRandomTarget.anchoredPosition.y);
    }

    private void UpdateProgressBar()
    {
        // Define target progress based on isGaugeCorrect
        float targetProgress = isGaugeCorrect ? 1f : 0f;

        // Smoothly transition the progress bar value to the target progress
        progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, progressSpeed * Time.deltaTime);
    }
}
