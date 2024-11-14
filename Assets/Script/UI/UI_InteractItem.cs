using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_InteractItem : MonoBehaviour
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
        if (type == "Kid")
        {
            chara = FindAuthorKid();
        }
        else if (type == "Pocong")
        {
            chara = FindAuthorPocong();
        }

        if (chara != null)
        {
            button.onClick.AddListener(InteractedWithItem);
        }
        else
        {
            Debug.LogWarning("No player with isAuthor found.");
        }
    }

    private void Update()
    {
        // If the character exists, check the cooldown timer
        // if (chara != null)
        // {
        //     if (chara.GetDashCooldown() <= 0)
        //     {
        //         EnableButton();
        //         buttonText.text = "";
        //     }
        //     else
        //     {
        //         DisableButton();
        //         buttonText.text = Mathf.CeilToInt(chara.GetDashCooldown()).ToString();
        //     }
        // }
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
        // Debug.Log("POCONG BUTTON ");
        if (type == "Pocong")
        {
            if (chara.GetCurrentItem() != null && chara.GetCurrentItem().isActivated)
            {
                Debug.LogWarning("is item null? " + chara.GetCurrentItem() == null);
                // InteractButton.SetActive(true);
                EnableButton();
            }
            else
            {
                Debug.LogWarning("is item null? " + chara.GetCurrentItem() == null);
                // InteractButton.SetActive(false);
                DisableButton();
            }
        }

        if (type == "Kid")
        {
            if (chara.GetCurrentItem() != null && !chara.GetCurrentItem().isActivated)
            {
                Debug.Log("[ITEM] is item null? " + chara.GetCurrentItem() == null);
                // InteractButton.SetActive(true);
                EnableButton();
            }
            // else if (chara.GetCurrentCloset() != null && !chara.GetCurrentCloset().isUsed)
            // {
            //     Debug.Log("[CLOSET] is closer null? " + chara.GetCurrentItem() == null);
            //     if (!chara.GetCurrentCloset().isUsed)
            //     {
            //         // InteractButtonHiding.SetActive(true);
            //         EnableButton();
            //     }
            // }
            else
            {
                Debug.Log("[NULL] is item null? " + chara.GetCurrentItem() == null);
                // InteractButton.SetActive(false);
                DisableButton();
                // InteractButtonHiding.SetActive(false);
            }
        }
    }

    public void InteractedWithItem()
    {
        // Debug.Log($"InteractedWithItem - Masuk fungsi InteractedWithItem");
        // Debug.Log($"InteractedWithItem - GetTypeChar: {chara.GetTypeChar()}");
        if (type == "Kid")
        {
            // Debug.Log("cek!");
            if (!chara.GetCurrentItem().isActivated)
            {
                // GameManager.instance.KidTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(chara.GetCurrentItem());
            }
        }
        else if (type == "Pocong")
        {
            Debug.Log($"InteractedWithItem - Masuk Else If Pocong");
            if (chara.GetCurrentItem().isActivated)
            {
                Debug.Log($"InteractedWithItem - Klo lilinnya Active Pocong bisa interact");
                // GameManager.instance.PocongTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(chara.GetCurrentItem());
                // Instantiate
            }
            // GameManager.instance.PocongTurnedOffItem(item);
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
