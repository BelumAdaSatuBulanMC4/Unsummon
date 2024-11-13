using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HideButton : MonoBehaviour
{
    private Button button;
    private Character chara;
    private TextMeshProUGUI buttonText;

    [SerializeField] string type;
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
        chara = FindAuthorKid();

        if (chara != null)
        {
            button.onClick.AddListener(InteractedWithCloset);
        }
        else
        {
            Debug.LogWarning("No player with isAuthor found.");
        }
    }

    private void Update()
    {
        if (chara != null)
        {
            if (chara.GetCurrentCloset() != null)
            {
                button.onClick.AddListener(InteractedWithCloset);
            }
        }
        else
        {
            Debug.LogWarning("No player with isAuthor found.");
        }

        HandleButtonInteraction();
    }

    private Character FindAuthorKid()
    {
        PlayerKid[] allKids = FindObjectsOfType<PlayerKid>();
        foreach (PlayerKid playerKid in allKids)
        {
            if (playerKid.isAuthor)
            {
                return playerKid;
            }
        }

        return null;
    }

    private Character FindAuthorPocong()
    {
        Pocong[] pocong = FindObjectsOfType<Pocong>();
        foreach (Pocong playerPocong in pocong)
        {
            if (playerPocong.isAuthor)
            {
                return playerPocong;
            }
        }

        return null;
    }

    private void HandleButtonInteraction()
    {
        if (chara.GetCurrentCloset() != null && !chara.GetCurrentCloset().isUsed && chara.GetHidingCooldown() <= 0)
        {
            EnableButton();
            buttonText.text = "";
        }
        else
        {
            DisableButton();
            if (Mathf.CeilToInt(chara.GetHidingCooldown()) <= 0)
            {
                buttonText.text = "";
            }
            else
            {
                buttonText.text = Mathf.CeilToInt(chara.GetHidingCooldown()).ToString();
            }
        }
    }

    public void InteractedWithCloset()
    {
        if (!chara.GetCurrentCloset().isUsed)
        {
            Debug.Log("Harusnya sih hiding UI muncul");
            UI_InGame.instance.OpenHidingMechanics();
            // UI_HidingMechanics.instance.CurrentCloset(authorCharacter.GetCurrentCloset());
        }
    }

    // Method to enable the button
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
