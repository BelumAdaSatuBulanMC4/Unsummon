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
        button.onClick.AddListener(TriggerNoise);

        // Debug.Log("Masuk ke button noise 1");
        // chara = FindAuthorSpirit();
        // if (chara != null)
        // {
        //     button.onClick.AddListener(TriggerNoise);
        //     Debug.Log("Masuk ke button noise 2");
        // }
    }

    private void Update()
    {
        chara = FindAuthorSpirit();

        if (chara != null)
        {
            button.onClick.AddListener(TriggerNoise);
            // Debug.Log("Masuk ke button noise 2");
        }

        if (chara != null)
        {
            if (!chara.IsMakingNoise())
            {
                EnableButton();
                buttonText.text = "";
            }
            else
            {
                DisableButton();
                // buttonText.text = Mathf.CeilToInt(chara.GetNoiseCooldown()).ToString();
            }
        }
    }

    private PlayerSpirit FindAuthorSpirit()
    {
        PlayerSpirit[] spirits = FindObjectsOfType<PlayerSpirit>();
        foreach (PlayerSpirit spirit in spirits)
        {
            // Debug.Log("Spirit found: " + spirit.name + " | isAuthor: " + spirit.GetIsAuthor());
            if (spirit.GetIsAuthor())
            {
                return spirit;
            }
        }

        return null;
    }

    public void TriggerNoise()
    {
        // Debug.Log("Halooooooo DI SiNI SUWARA");
        if (chara != null && chara.GetNoiseCooldown() <= 0)
        {
            chara.NoiseButton();
            // Debug.Log("Noise");
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