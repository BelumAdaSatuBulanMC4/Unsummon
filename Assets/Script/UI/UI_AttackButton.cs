using System.Collections;
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

    private bool isAttackDisabled;

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
            // Check if teleport just happened
            if (chara.GetIsTeleported() && !isAttackDisabled)
            {
                StartCoroutine(DisableAttackForTeleport());
            }

            if (!isAttackDisabled && chara.GetAttackCooldown() <= 0 && chara.GetIsKidDetected())
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                if (Mathf.CeilToInt(chara.GetAttackCooldown()) <= 0)
                {
                    buttonText.text = "";
                }
                else
                {
                    buttonText.text = Mathf.CeilToInt(chara.GetAttackCooldown()).ToString(); ;
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

    public void TriggerKill()
    {
        if (chara != null && chara.GetAttackCooldown() <= 0 && !isAttackDisabled)
        {
            chara.AttackButton();
        }
    }

    private void EnableButton()
    {
        button.interactable = true;
        buttonImage.color = enabledColor;  // Set to full visibility
    }

    private void DisableButton()
    {
        button.interactable = false;
        buttonImage.color = disabledColor;  // Set to semi-transparency
    }

    // Coroutine to disable the attack for 5 seconds after teleporting
    private IEnumerator DisableAttackForTeleport()
    {
        isAttackDisabled = true;
        DisableButton();  // Disable attack button immediately after teleporting
        yield return new WaitForSeconds(5f);  // Wait for 5 seconds
        isAttackDisabled = false;  // Re-enable attack
    }
}
