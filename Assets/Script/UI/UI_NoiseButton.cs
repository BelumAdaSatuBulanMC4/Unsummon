using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NoiseButton : MonoBehaviour
{
    private Button button;
    private PlayerSpirit chara;
    private TextMeshProUGUI buttonText;
    [SerializeField] private Color enabledColor = new Color(1f, 1f, 1f, 1f);
    [SerializeField] private Color disabledColor = new Color(1f, 1f, 1f, 0.5f);
    private Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        chara = FindAuthorSpirit();
        if (chara != null)
        {
            button.onClick.AddListener(TriggerNoise);
        }
    }

    private void Update()
    {
        if (chara != null)
        {
            if (chara.GetNoiseCooldown() <= 0)
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                buttonText.text = Mathf.CeilToInt(chara.GetNoiseCooldown()).ToString();
            }
        }
    }

    private PlayerSpirit FindAuthorSpirit()
    {
        PlayerSpirit[] spirits = FindObjectsOfType<PlayerSpirit>();
        foreach (PlayerSpirit spirit in spirits)
        {
            if (spirit.isAuthor)
            {
                return spirit;
            }
        }

        return null;
    }

    public void TriggerNoise()
    {
        if (chara != null && chara.GetNoiseCooldown() <= 0)
        {
            chara.NoiseButton();
        }
    }

    private void EnableButton()
    {
        button.interactable = true;
        buttonImage.color = enabledColor;  // Set to full visibility
    }

    // Method to disable the button
    private void DisableButton()
    {
        button.interactable = false;
        buttonImage.color = disabledColor;  // Set to semi-transparency
    }
}
