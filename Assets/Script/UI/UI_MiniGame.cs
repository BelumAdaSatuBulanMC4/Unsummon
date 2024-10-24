using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_MiniGame : MonoBehaviour
{
    public static UI_MiniGame instance;
    [SerializeField] Button candleButton;
    [SerializeField] Button cancelGame;
    [SerializeField] TMP_Text candleText;
    [SerializeField] TMP_Text instructionText;
    private Animator anim;

    private bool isHoldingButton = false;
    private float holdTime = 0f;
    private float maxHoldTime = 5f; // Maximum hold time (5 seconds)
    private float startConditionValue = 0f; // Initial condition value
    private float candleConditionValue = 0f;

    private Item item;

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
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        // candleButton.onClick.AddListener(CandleInteraction);
        cancelGame.onClick.AddListener(CancelMiniGame);

        // Add OnPointerDown and OnPointerUp for holding functionality
        EventTrigger trigger = candleButton.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry pointerDownEntry = new EventTrigger.Entry();
        pointerDownEntry.eventID = EventTriggerType.PointerDown;
        pointerDownEntry.callback.AddListener((data) => { StartHoldingButton(); });
        trigger.triggers.Add(pointerDownEntry);

        EventTrigger.Entry pointerUpEntry = new EventTrigger.Entry();
        pointerUpEntry.eventID = EventTriggerType.PointerUp;
        pointerUpEntry.callback.AddListener((data) => { StopHoldingButton(); });
        trigger.triggers.Add(pointerUpEntry);
    }

    private void Update()
    {
        if (item != null)
        {
            // Disable the button and use speech recognition if the item is cursed
            if (item.isCursed)
            {
                candleButton.interactable = false; // Disable the button
                StartVoiceRecognitionForCurse(item); // Start speech recognition
            }
            else
            {
                candleButton.interactable = true; // Enable the button if not cursed

                if (isHoldingButton)
                {
                    holdTime += Time.deltaTime;

                    // Scale hold time to range from startConditionValue to 5
                    candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
                    anim.SetFloat("CandleCondition", candleConditionValue);

                    if (item.isActivated)
                    {
                        instructionText.text = "Hold to snuff out the candle";
                        if (candleConditionValue >= 4f) // Snuffing the candle at value 4
                        {
                            GameManager.instance.PocongTurnedOffItem(item);
                            StartCoroutine(WaitAndDeactivate());
                        }
                    }
                    else
                    {
                        instructionText.text = "Hold to light the candle";
                        if (candleConditionValue >= 2f) // Lighting the candle at value 2
                        {
                            GameManager.instance.KidTurnedOnItem(item);
                            StartCoroutine(WaitAndDeactivate());
                        }
                    }
                }
                else
                {
                    if (candleConditionValue > startConditionValue)
                    {
                        holdTime -= Time.deltaTime;
                        candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
                        anim.SetFloat("CandleCondition", candleConditionValue);
                    }
                }
            }
        }
    }

    private void StartVoiceRecognitionForCurse(Item cursedItem)
    {
        GameManager.instance.StartSpeechRecognitionForCurseRemoval(cursedItem, OnSpeechFeedback);
    }

    private void OnSpeechFeedback(string feedbackMessage, bool isCorrect)
    {
        instructionText.text = feedbackMessage;

        if (isCorrect)
        {
            StartCoroutine(WaitBeforeEnablingInteraction(3f)); // Wait for 3 seconds before re-enabling the button
        }
    }

    private IEnumerator WaitBeforeEnablingInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        item.isCursed = false; // Remove the curse
        candleButton.interactable = true; // Re-enable the button after 3 seconds
    }

    // public void CandleInteraction()
    // {
    //     // This is controlled by Update now
    // }

    public void CurrentItem(Item newItem)
    {
        item = newItem;
        // Set initial candle condition value based on the item's activation status
        startConditionValue = item.isActivated ? 2f : 0f;
        candleConditionValue = startConditionValue; // Start the candle condition at this value
        anim.SetFloat("CandleCondition", candleConditionValue); // Set the animator condition to start from this value
    }

    public void CancelMiniGame()
    {
        gameObject.SetActive(false);
    }

    public void CancelVoiceRecognitionzure()
    {
        GameManager.instance.CancelVoiceRecognition();
        //make something setAtive to false
    }

    // Called when button is pressed
    private void StartHoldingButton()
    {
        isHoldingButton = true;
        holdTime = 0f; // Reset the hold time
    }

    // Called when button is released
    public void StopHoldingButton()
    {
        isHoldingButton = false;
    }

    // Coroutine to wait for 1 second and then deactivate the game object
    private IEnumerator WaitAndDeactivate()
    {
        // Stop holding interaction after reaching the target condition
        isHoldingButton = false;

        // Hold the final state for 1 second
        yield return new WaitForSeconds(1f);

        // Deactivate the mini-game UI
        gameObject.SetActive(false);
    }
}
