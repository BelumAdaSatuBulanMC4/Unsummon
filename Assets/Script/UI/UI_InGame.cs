using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InGame : MonoBehaviour
{
    public static UI_InGame instance;

    [Header("UI Prefabs")]
    [SerializeField] private GameObject UI_GameInfo;
    [SerializeField] private GameObject UI_InGameSettings;
    [SerializeField] private GameObject UI_MiniGames;
    [SerializeField] private GameObject UI_InGamePocong;
    [SerializeField] private GameObject UI_InGameKid;
    [SerializeField] private GameObject UI_InGameSpirit;
    [SerializeField] private GameObject InteractButton;

    private GameObject currentInGameController;
    private Character authorCharacter;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // void Start()
    // {
    //     authorCharacter = FindAuthorCharacter();

    //     if (authorCharacter != null)
    //     {
    //         InstantiateUIForCharacter(authorCharacter);
    //         InteractButton.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithItem);
    //     }
    //     else
    //     {
    //         Debug.LogWarning("no one have isAuthor");
    //     }
    // }

    private void Update()
    {
        authorCharacter = FindAuthorCharacter();

        if (authorCharacter != null)
        {
            InstantiateUIForCharacter(authorCharacter);
            InteractButton.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithItem);
        }
        else
        {
            Debug.LogWarning("no one have isAuthor");
        }

        HandleButtonInteraction();
    }

    private void HandleButtonInteraction()
    {
        // Debug.Log("POCONG BUTTON ");
        if (authorCharacter.GetTypeChar() == "Pocong")
        {
            if (authorCharacter.GetCurrentItem() != null && authorCharacter.GetCurrentItem().isActivated)
            {
                Debug.LogWarning("is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(true);
            }
            else
            {
                Debug.LogWarning("is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(false);
            }
        }
        if (authorCharacter.GetTypeChar() == "Player")
        {
            if (authorCharacter.GetCurrentItem() != null && !authorCharacter.GetCurrentItem().isActivated)
            {
                Debug.LogWarning("is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(true);
            }
            else
            {
                Debug.LogWarning("is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(false);
            }
        }
    }

    public void InteractedWithItem()
    {
        if (authorCharacter.GetTypeChar() == "Player")
        {
            // Debug.Log("cek!");
            if (!authorCharacter.GetCurrentItem().isActivated)
            {
                // GameManager.instance.KidTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(authorCharacter.GetCurrentItem());
            }
        }
        else if (authorCharacter.GetTypeChar() == "Pocong")
        {
            if (authorCharacter.GetCurrentItem().isActivated)
            {
                // GameManager.instance.PocongTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(authorCharacter.GetCurrentItem());
            }
            // GameManager.instance.PocongTurnedOffItem(item);
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

    public void OpenMiniGame()
    {
        UI_MiniGames.SetActive(true);
    }

    public void CloseMiniGame()
    {
        UI_MiniGames.SetActive(false);
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
        Debug.Log("jumlah author " + allCharacters.Length);
        foreach (Character character in allCharacters)
        {
            if (character.GetIsAuthor())
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
            UI_InGameKid.SetActive(true);
            // Instantiate(currentInGameController, transform);
        }
        else if (character is Pocong)
        {
            currentInGameController = UI_InGamePocong;
            UI_InGamePocong.SetActive(true);
            // Instantiate(currentInGameController, transform);
        }
        else if (character is PlayerSpirit)
        {
            currentInGameController = UI_InGameSpirit;
            UI_InGameSpirit.SetActive(true);
            UI_InGameKid.SetActive(false);
            // Instantiate(currentInGameController, transform);
        }
        else
        {
            Debug.LogWarning("Unknown character type.");
        }
    }
}
