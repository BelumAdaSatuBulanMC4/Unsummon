using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HidingMechanics : MonoBehaviour
{
    public static UI_HidingMechanics instance;

    [SerializeField] private RectTransform gaugeNeedle;
    [SerializeField] private RectTransform gaugeBackground;
    [SerializeField] private RectTransform gaugeRandomTarget;
    // [SerializeField] private TMP_Text speed;

    public float sensitivity = 5.0f;
    public float minPitch = -0.5f;
    public float maxPitch = 0.5f;
    public float animationSpeed = 2.0f;
    public float progressSpeed = 1f;

    private float minPositionX;
    private float maxPositionX;

    //swiftplugin pake gameManager
    private Vector2 targetPosition;
    private bool isGaugeCorrect;
    // [SerializeField] private Slider progressBar;
    [SerializeField] private Image progressBarImage;

    private Closet closet;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        float halfNeedleWidth = gaugeNeedle.rect.width / 2;
        minPositionX = gaugeBackground.rect.xMin + halfNeedleWidth;
        maxPositionX = gaugeBackground.rect.xMax - halfNeedleWidth;

        //make gameManager to start gyro
        GameManager.instance.StartGyroCoreMotion();

        SetRandomTargetPosition();

        // progressBar.value = 0.5f;
        progressBarImage.fillAmount = 1f;

        StartCoroutine(UpdateRandomTargetPositionRoutine());
        // StartCoroutine(IncreaseProgressSpeedRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        //make a get value for roll pitch yaw
        // double roll = swiftPlugin.GetRollValue();
        // double pitch = swiftPlugin.GetPitchValue();
        double pitch = GameManager.instance.GetPitchValueFromSwift();
        // double yaw = swiftPlugin.GetYawValue();

        float normalizedPitch = Mathf.InverseLerp(minPitch, maxPitch, (float)pitch);

        float targetPositionX = Mathf.Lerp(minPositionX, maxPositionX, normalizedPitch);

        targetPositionX = Mathf.Clamp(targetPositionX, minPositionX, maxPositionX);

        gaugeNeedle.anchoredPosition = new Vector2(targetPositionX, gaugeNeedle.anchoredPosition.y);

        gaugeRandomTarget.anchoredPosition = Vector2.Lerp(gaugeRandomTarget.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);

        if (targetPositionX >= gaugeRandomTarget.anchoredPosition.x - gaugeRandomTarget.rect.width / 2 &&
            targetPositionX <= gaugeRandomTarget.anchoredPosition.x + gaugeRandomTarget.rect.width / 2)
        {
            // feedbackText.text = "Correct";
            isGaugeCorrect = true;
        }
        else
        {
            // feedbackText.text = ""; 
            isGaugeCorrect = false;
        }

        UpdateProgressBar();
    }

    private IEnumerator UpdateRandomTargetPositionRoutine()
    {
        while (true)
        {
            SetRandomTargetPosition();
            yield return new WaitForSeconds(Random.Range(0.1f, 1f)); // Random delay between position changes
        }
    }

    private IEnumerator IncreaseProgressSpeedRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
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
        // progressBar.value = Mathf.MoveTowards(progressBar.value, targetProgress, 1 * Time.deltaTime);
        progressBarImage.fillAmount = Mathf.MoveTowards(progressBarImage.fillAmount, targetProgress, .7f * Time.deltaTime);
        // Debug.Log("Progress bar value: " + progressBar.value);
        // speed.text = "Speed " + progressSpeed + " and target progress: " + progressBar.value + " and image progress " + progressBarImage.fillAmount;

        if (progressBarImage.fillAmount == 0f)
        {
            CancelHidingMechanics();
        }
    }

    public void CurrentCloset(Closet newCloset)
    {
        closet = newCloset;
        // float halfNeedleWidth = gaugeNeedle.rect.width / 2;
        // minPositionX = gaugeBackground.rect.xMin + halfNeedleWidth;
        // maxPositionX = gaugeBackground.rect.xMax - halfNeedleWidth;

        // //make gameManager to start gyro
        // GameManager.instance.StartGyroCoreMotion();

        // SetRandomTargetPosition();

        // progressBar.value = 0.5f;
        // progressSpeed = 0.1f;

        // StartCoroutine(UpdateRandomTargetPositionRoutine());
        // StartCoroutine(IncreaseProgressSpeedRoutine());
        //set initialize buat closet di sini
    }

    public void CancelHidingMechanics()
    {
        //bikin stop gyro di sini
        // GameManager.instance.StopGyroCoreMotion();
        // StopAllCoroutines();
        // progressSpeed = 0.1f;
        // progressBar.value = 0.5f;
        // gameObject.SetActive(false);
        UI_InGame.instance.CloseHidingMechanics();
    }
}
