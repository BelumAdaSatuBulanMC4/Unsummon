using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetUsername : MonoBehaviour
{
    [SerializeField] private Color enabledColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color disabledColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color enabledColorText = new Color(0f, 0f, 0f, 1f);
    [SerializeField] private Color disabledColorText = new Color(0f, 0f, 0f, 0.5f);
    [SerializeField] private GameObject setUsernameUI;
    [Header("username set")]
    [SerializeField] private GameObject buttonSaveName;
    [SerializeField] private GameObject textInput;
    [SerializeField] private GameObject inputInformation;

    [Header("EULA set")]
    [SerializeField] private GameObject nextButton;
    [SerializeField] private GameObject scrollObj;

    private ScrollRect scrollRect; // Reference to the ScrollRect component
    private float threshold = 0.01f;

    private bool isEndScroll = false;

    private Button nextButtonComponent;
    private Image buttonImage;

    private TMP_Text nextButtonText;

    [SerializeField] private TMP_InputField inputUsernameField;

    private void Awake()
    {
        scrollRect = scrollObj.GetComponent<ScrollRect>();
        nextButtonComponent = nextButton.GetComponent<Button>();
        buttonImage = nextButton.GetComponent<Image>();
        nextButtonText = nextButton.GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        if (!DataPersistence.GetIsFirstTime())
        {
            setUsernameUI.SetActive(false);
        }
        else
        {
            setUsernameUI.SetActive(true);
        }
    }


    private void Update()
    {
        if (scrollRect != null)
        {
            if (scrollRect.verticalNormalizedPosition <= threshold)
            {
                isEndScroll = true;
                EnableButton();
            }
            else
            {
                isEndScroll = false;
                DisableButton();
            }
        }

    }

    public void OnSaveButtonClicked()
    {
        string newUsername = "Player";
        DataPersistence.EditUsername(newUsername);
        setUsernameUI.SetActive(false);
    }

    public void OnNextButtonClicked()
    {
        buttonSaveName.SetActive(true);
        textInput.SetActive(true);
        inputInformation.SetActive(true);
        nextButton.SetActive(false);
        scrollObj.SetActive(false);
    }

    private void EnableButton()
    {
        nextButtonComponent.interactable = true;
        buttonImage.color = enabledColor;  // Set to full visibility
        nextButtonText.color = enabledColorText;
    }

    private void DisableButton()
    {
        nextButtonComponent.interactable = false;
        buttonImage.color = disabledColor;  // Set to semi-transparency
        nextButtonText.color = disabledColorText;
    }
}