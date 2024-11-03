using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GyroGaugeController : MonoBehaviour
{
    [SerializeField] private RectTransform gaugeNeedle;
    [SerializeField] private RectTransform gaugeBackground;
    [SerializeField] private Slider slider;

    public float sensitivity = 5.0f;
    public float minPitch = -0.5f;
    public float maxPitch = 0.5f;

    private float minPositionX;
    private float maxPositionX;

    [SerializeField] private TMP_Text rollVal;
    [SerializeField] private TMP_Text pitchVal;
    [SerializeField] private TMP_Text yawVal;

    private SwiftPlugin swiftPlugin;

    private void Awake()
    {
        swiftPlugin = GetComponent<SwiftPlugin>();
    }

    private void Start()
    {
        minPositionX = gaugeBackground.rect.xMin;
        maxPositionX = gaugeBackground.rect.xMax;

        swiftPlugin.StartGyro();
    }

    private void OnDestroy()
    {
        swiftPlugin.StopGyro();
    }

    private void Update()
    {
        double roll = swiftPlugin.GetRollValue();
        double pitch = swiftPlugin.GetPitchValue();
        double yaw = swiftPlugin.GetYawValue();

        rollVal.text = roll.ToString("F2");
        pitchVal.text = pitch.ToString("F2");
        yawVal.text = yaw.ToString("F2");

        float normalizedPitch = Mathf.InverseLerp(minPitch, maxPitch, (float)pitch);

        float targetPositionX = Mathf.Lerp(minPositionX, maxPositionX, normalizedPitch);
        gaugeNeedle.anchoredPosition = new Vector2(targetPositionX, gaugeNeedle.anchoredPosition.y);

        float targetValue = Mathf.Lerp(slider.minValue, slider.maxValue, normalizedPitch);
        slider.value = Mathf.Clamp(targetValue, slider.minValue, slider.maxValue);
    }

}
