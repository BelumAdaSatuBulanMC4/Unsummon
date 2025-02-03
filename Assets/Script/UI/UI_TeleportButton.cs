using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TeleportButton : MonoBehaviour
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
            button.onClick.AddListener(TriggerTeleport);
        }
    }

    private void Update()
    {
        if (chara != null)
        {
            if (chara.GetTeleportCooldown() <= 0 && chara.GetIsMirrorDetected())
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                if (Mathf.CeilToInt(chara.GetTeleportCooldown()) <= 0)
                {
                    buttonText.text = "";
                }
                else
                {
                    buttonText.text = Mathf.CeilToInt(chara.GetTeleportCooldown()).ToString();
                }
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

    public void TriggerTeleport()
    {
        if (chara != null && chara.GetTeleportCooldown() <= 0)
        {
            chara.TeleportButton();
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
