using System.Collections;
using System.Collections.Generic;
// using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinningCondition : NetworkBehaviour
{
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text secondaryText;
    [SerializeField] private TMP_Text informationText;
    [SerializeField] private Image splash;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button playAgainButton;

    private bool isKidsWin;
    private bool isPocongWin;
    private string whoAreYou;

    void Start()
    {
        isKidsWin = GameManager.instance.GetKidsWin();
        isPocongWin = GameManager.instance.GetPocongWin();
        whoAreYou = UserManager.instance.getYourRole();

        if (isKidsWin)
        {
            SetupSceneKids(isKidsWin);
        }

        if (isPocongWin)
        {
            SetupScenePocong(isPocongWin);
        }
    }

    public void HomeButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void PlayAgainButton()
    {
        // SceneManager.LoadScene("Lobby");
    }

    private void SetupSceneKids(bool isWin)
    {
        if (isWin)
        {
            victoryText.text = "Victory!";
            secondaryText.text = "Cursed Conquest";
            informationText.text = "The candles are lit, and the pocong is banished back to hell!";
            splash.enabled = false;
        }
        else
        {
            victoryText.text = "Defeat";
            secondaryText.text = "Eternal Doom";
            informationText.text = "The pocong has devoured all the children, your family will be in hell forever.";
            splash.enabled = true;
        }
    }

    private void SetupScenePocong(bool isWin)
    {
        if (isWin)
        {
            victoryText.text = "Victory!";
            secondaryText.text = "Occult Ascendancy";
            informationText.text = "The pocong reigns supreme! All the children have been eaten.";
            splash.enabled = false;
        }
        else
        {
            victoryText.text = "Defeat";
            secondaryText.text = "Ritual Collapse";
            informationText.text = "The light prevails! The pocong is dragged back to hell.";
            splash.enabled = true;
        }
    }
}
