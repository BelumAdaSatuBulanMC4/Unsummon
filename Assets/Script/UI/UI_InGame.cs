using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_InGame : MonoBehaviour
{
    [Header("UI Prefabs")]
    [SerializeField] private GameObject UI_InGameKid;
    [SerializeField] private GameObject UI_InGamePocong;
    [SerializeField] private GameObject UI_InGameSpirit;
    [SerializeField] private GameObject UI_GameInfo;
    [SerializeField] private GameObject UI_InGameSettings;

    private GameObject currentInGameController;

    // [SerializeField] private GameObject[] uiElements;

    private Character authorCharacter;

    void Start()
    {
        authorCharacter = FindAuthorCharacter();

        if (authorCharacter != null)
        {
            InstantiateUIForCharacter(authorCharacter);
        }
        else
        {
            Debug.LogWarning("no one have isAuthor");
        }
    }

    public void SwitchToSettings()
    {
        UI_InGameSettings.SetActive(true);

    }

    public void CloseSettings()
    {
        UI_InGameSettings.SetActive(false);
    }

    // public void SwitchUI(GameObject uiToEnable)
    // {
    //     foreach (GameObject ui in uiElements)
    //     {
    //         ui.SetActive(false);
    //     }

    //     uiToEnable.SetActive(true);
    // }

    private Character FindAuthorCharacter()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();
        foreach (Character character in allCharacters)
        {
            if (character.isAuthor)
            {
                return character;
            }
        }
        return null;
    }

    private void InstantiateUIForCharacter(Character character)
    {
        if (character is PlayerKid)
        {
            currentInGameController = UI_InGameKid;
            Instantiate(currentInGameController, transform);
        }
        else if (character is Pocong)
        {
            currentInGameController = UI_InGamePocong;
            Instantiate(currentInGameController, transform);
        }
        else if (character is PlayerSpirit)
        {
            currentInGameController = UI_InGameSpirit;
            Instantiate(currentInGameController, transform);
        }
        else
        {
            Debug.LogWarning("Unknown character type.");
        }
    }
}
