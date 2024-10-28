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
    [SerializeField] GameObject imageAnimated;
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
        anim = imageAnimated.GetComponent<Animator>();
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
        // Debug.Log("item is cursed? " + item.isCursed);

        // if (item != null)
        // {
        //     // Disable the button and use speech recognition if the item is cursed
        //     if (item.isCursed)
        //     {
        //         Debug.Log("id this printed, the item is cursed!");
        //         candleButton.interactable = false; // Disable the button
        //         StartVoiceRecognitionForCurse(item); // Start speech recognition
        //         IsVoiceCorrect();
        //         instructionText.text = GameManager.instance.GetTheSpeech();
        //     }
        //     else
        //     {
        //         Debug.Log("congrats, the item is not cursed!");
        //         candleButton.interactable = true; // Enable the button if not cursed

        //         if (isHoldingButton)
        //         {
        //             holdTime += Time.deltaTime;

        //             // Scale hold time to range from startConditionValue to 5
        //             candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
        //             anim.SetFloat("CandleCondition", candleConditionValue);

        //             if (item.isActivated)
        //             {
        //                 instructionText.text = "Hold to snuff out the candle";
        //                 if (candleConditionValue >= 4f) // Snuffing the candle at value 4
        //                 {
        //                     GameManager.instance.PocongTurnedOffItem(item);
        //                     StartCoroutine(WaitAndDeactivate());
        //                 }
        //             }
        //             else
        //             {
        //                 instructionText.text = "Hold to light the candle";
        //                 if (candleConditionValue >= 2f) // Lighting the candle at value 2
        //                 {
        //                     GameManager.instance.KidTurnedOnItem(item);
        //                     StartCoroutine(WaitAndDeactivate());
        //                 }
        //             }
        //         }
        //         else
        //         {
        //             if (candleConditionValue > startConditionValue)
        //             {
        //                 holdTime -= Time.deltaTime;
        //                 candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
        //                 anim.SetFloat("CandleCondition", candleConditionValue);
        //             }
        //         }
        //     }
        // }
        if (item.isCursed)
        {
            Debug.Log("The item is now cursed!!!!");
            candleButton.interactable = false; // Disable the button
            StartVoiceRecognitionForCurse(item); // Start speech recognition
            IsVoiceCorrect();
            // instructionText.text = GameManager.instance.GetTheSpeech();
        }
        else
        {
            Debug.Log("The item is not cursed!!!!");
            candleButton.interactable = true; // Disable the button
            GameManager.instance.CancelVoiceRecognition(); ///

            if (isHoldingButton)
            {
                // Increase the hold time while holding the button
                holdTime += Time.deltaTime;

                // Scale hold time to range from startConditionValue to 5
                candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
                anim.SetFloat("CandleCondition", candleConditionValue);

                if (item != null)
                {
                    if (item.isActivated)
                    {
                        instructionText.text = "Hold to snuff out the candle";
                        if (candleConditionValue >= 4f) // Snuffing the candle at value 4
                        {
                            GameManager.instance.PocongTurnedOffItem(item);
                            isHoldingButton = false;
                            gameObject.SetActive(false);
                            // StartCoroutine(WaitAndDeactivate());
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
            }
            else
            {
                // Gradually decrease candleConditionValue to startConditionValue when not holding the button
                if (candleConditionValue > startConditionValue)
                {
                    holdTime -= Time.deltaTime; // Decrease the hold time
                    candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f + startConditionValue, startConditionValue, 5f);
                    anim.SetFloat("CandleCondition", candleConditionValue);
                }
            }
        }
    }

    private void StartVoiceRecognitionForCurse(Item cursedItem)
    {
        GameManager.instance.StartSpeechRecognitionForCurseRemoval(cursedItem, OnSpeechFeedback);
    }

    private void IsVoiceCorrect()
    {
        string recognizedText = GameManager.instance.GetTheSpeech();

        if (recognizedText.ToLower().Contains("buka") && item.isCursed)
        {
            // item.RemoveCurse();  // Remove the curse if the word is correctly recognized
            instructionText.text = "Correct! You said 'Buka'";
            GameManager.instance.KidTurnedOnItem(item);
            GameManager.instance.CancelVoiceRecognition();
            StartCoroutine(WaitAndDeactivate());
            // StartCoroutine(WaitBeforeEnablingInteraction(3f));
        }
        else
        {
            instructionText.text = "Say 'Buka' to break the curse.";
        }
    }

    // private void IsVoiceCorrect()
    // {
    //     string recognizedText = GameManager.instance.GetTheSpeech();
    //     if (recognizedText.ToLower().Contains("buka"))
    //     {
    //         // item.CurseDectivated(); // Remove the curse if the word is correctly recognized
    //         // item.ItemActivated();
    //         GameManager.instance.KidTurnedOnItem(item);
    //         StartCoroutine(WaitAndDeactivate());
    //         Debug.Log("The spell contain Buka!");
    //         Debug.Log("the item isCursed now " + item.isCursed);
    //         // recognizedText = "";

    //         // StartCoroutine(WaitAndDeactivate());
    //         // item.isCursed = false;
    //         // feedbackCallback?.Invoke("Correct! You said 'Buka'", true); // Trigger callback for correct feedback
    //     }
    //     else
    //     {
    //         Debug.Log("didn't contain Buka, the item isCursed now " + item.isCursed);
    //     }
    // }

    private void OnSpeechFeedback(string feedbackMessage, bool isCorrect)
    {
        instructionText.text = feedbackMessage;

        if (isCorrect)
        {
            GameManager.instance.KidTurnedOnItem(item);
            // GameManager.instance.CancelVoiceRecognition();
            // StartCoroutine(WaitAndDeactivate());

            isHoldingButton = false;
            gameObject.SetActive(false);


            // StartCoroutine(WaitBeforeEnablingInteraction(3f)); // Wait for 3 seconds before re-enabling the button
        }
    }

    private IEnumerator WaitBeforeEnablingInteraction(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        // item.isCursed = false; // Remove the curse
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

    private void HandleButtonHold()
    {
        if (isHoldingButton)
        {
            holdTime += Time.deltaTime;
            candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f, 0f, 5f);
            anim.SetFloat("CandleCondition", candleConditionValue);

            if (item.isActivated && candleConditionValue >= 4f)
            {
                GameManager.instance.PocongTurnedOffItem(item);
                StartCoroutine(DeactivateUI());
            }
            else if (!item.isActivated && candleConditionValue >= 2f)
            {
                GameManager.instance.KidTurnedOnItem(item);
                StartCoroutine(DeactivateUI());
            }
        }
        else
        {
            if (candleConditionValue > 0)
            {
                holdTime -= Time.deltaTime;
                candleConditionValue = Mathf.Clamp((holdTime / maxHoldTime) * 5f, 0f, 5f);
                anim.SetFloat("CandleCondition", candleConditionValue);
            }
        }
    }

    private IEnumerator DeactivateUI()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
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