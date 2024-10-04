using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_AttackButton : MonoBehaviour
{
    private Button button;
    private Pocong chara;
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
        chara = FindAuthorPocong();
        if (chara != null)
        {
            button.onClick.AddListener(TriggerKill);
        }
    }

    private void Update()
    {
        if (chara != null)
        {
            if (chara.GetAttackCooldown() <= 0 && chara.GetIsKidDetected())
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                buttonText.text = "";
                // buttonText.text = Mathf.CeilToInt(chara.GetAttackCooldown()).ToString();
            }
        }
    }

    private Pocong FindAuthorPocong()
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

    public void TriggerKill()
    {
        if (chara != null && chara.GetAttackCooldown() <= 0)
        {
            chara.AttackButton();
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
