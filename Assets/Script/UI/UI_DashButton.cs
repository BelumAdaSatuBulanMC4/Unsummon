using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_DashButton : MonoBehaviour
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
            button.onClick.AddListener(TriggerDash);
        }
        else
        {
            Debug.LogWarning("No player with isAuthor found.");
        }
    }

    private void Update()
    {
        // If the character exists, check the cooldown timer
        if (chara != null)
        {
            if (chara.GetDashCooldown() <= 0)
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                buttonText.text = Mathf.CeilToInt(chara.GetDashCooldown()).ToString();
            }
        }
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

    public void TriggerDash()
    {
        if (chara != null && chara.GetDashCooldown() <= 0)
        {
            chara.DashButton();
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


// public class UI_DashButton : MonoBehaviour
// {
//     [SerializeField] Character kid;
//     private Button button;

//     private void Awake()
//     {
//         button = GetComponent<Button>();
//     }

//     private void Start()
//     {
//         button.onClick.AddListener(TriggerDash);
//     }
//     public void TriggerDash()
//     {
//         kid.DashButton();
//     }
// }
