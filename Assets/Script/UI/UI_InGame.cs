using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private GameObject InteractButtonHiding;

    [SerializeField] private GameObject UI_NoiseButton;
    [SerializeField] private GameObject UI_HidingMiniGame;
    private GameObject instantiatedHidingMechanics;
    public JoystickGame joystickGame;

    private GameObject currentInGameController;
    private Character authorCharacter;

    private bool isOnHidingMiniGame = false;

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

    void Start()
    {
        authorCharacter = FindAuthorCharacter();
        InstantiateUIForCharacter(authorCharacter);
        // if (authorCharacter != null)
        // {
        //     InstantiateUIForCharacter(authorCharacter);
        //     InteractButton.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithItem);
        // }
        // else
        // {
        //     Debug.LogWarning("no one have isAuthor");
        // }
    }

    private void Update()
    {
        // authorCharacter = FindAuthorCharacter();
        joystickGame = FindObjectOfType<JoystickGame>();

        if (authorCharacter != null)
        {
            if (authorCharacter.GetTypeChar() == "Player")
            {
                if (authorCharacter.GetCurrentItem() != null)
                {
                    InteractButton.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithItem);
                }
                else if (authorCharacter.GetCurrentCloset() != null)
                {
                    InteractButtonHiding.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithCloset);
                }
            }
        }
        else
        {
            Debug.LogWarning("no one have isAuthor");
            DeActivatedButton();
        }

        HandleButtonInteraction();
    }

    private void DeActivatedButton()
    {
        InteractButton.SetActive(false);
        InteractButtonHiding.SetActive(false);
        UI_InGameKid.SetActive(false);
        UI_InGamePocong.SetActive(false);
        UI_InGameSpirit.SetActive(false);
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
                Debug.Log("[ITEM] is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(true);
            }
            else if (authorCharacter.GetCurrentCloset() != null && !authorCharacter.GetCurrentCloset().isUsed)
            {
                Debug.Log("[CLOSET] is closer null? " + authorCharacter.GetCurrentItem() == null);
                if (!authorCharacter.GetCurrentCloset().isUsed)
                {
                    InteractButtonHiding.SetActive(true);
                }
            }
            else
            {
                Debug.Log("[NULL] is item null? " + authorCharacter.GetCurrentItem() == null);
                InteractButton.SetActive(false);
                InteractButtonHiding.SetActive(false);
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
                OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(authorCharacter.GetCurrentItem());
            }

            //test the closet
            // if (!authorCharacter.GetCurrentCloset().isUsed)
            // {
            //     // UI_InGame.instance.OpenMiniGame();
            //     OpenHidingMechanics();
            //     UI_HidingMechanics.instance.CurrentCloset(authorCharacter.GetCurrentCloset());
            // }
        }
        else if (authorCharacter.GetTypeChar() == "Pocong")
        {
            if (authorCharacter.GetCurrentItem().isActivated)
            {
                // GameManager.instance.PocongTurnedOnItem(item);
                OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(authorCharacter.GetCurrentItem());
                // Instantiate
            }
            // GameManager.instance.PocongTurnedOffItem(item);
        }
    }

    /// BIKIN BUAT INTERACTION WITH CLOSET HERE!
    public void InteractedWithCloset()
    {
        if (authorCharacter.GetTypeChar() == "Player")
        {
            Debug.Log("Interact sama closet!");
            if (!authorCharacter.GetCurrentCloset().isUsed)
            {
                Debug.Log("Harusnya sih hiding UI muncul");
                UI_InGame.instance.OpenHidingMechanics();
                // UI_HidingMechanics.instance.CurrentCloset(authorCharacter.GetCurrentCloset());
            }
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
            Debug.Log("InGame spirit " + character.ToString());
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

    //================================================================
    // //Gyro MotionManager
    // public void OpenHidingMechanics()
    // {
    //     UI_HidingMiniGame.SetActive(true);
    // }
    public void OpenHidingMechanics()
    {
        if (instantiatedHidingMechanics == null)
        {
            // Instantiate the UI_HidingMechanics prefab
            instantiatedHidingMechanics = Instantiate(UI_HidingMiniGame, transform.parent); // Set parent to UI container
            instantiatedHidingMechanics.SetActive(true);
        }
        else
        {
            instantiatedHidingMechanics.SetActive(true); // Reactivate if already instantiated
        }

        // Initialize UI_HidingMechanics with any required data
        UI_HidingMechanics hidingMechanics = instantiatedHidingMechanics.GetComponent<UI_HidingMechanics>();
        if (hidingMechanics != null)
        {
            hidingMechanics.CurrentCloset(authorCharacter.GetCurrentCloset());
        }

        // isOnHidingMiniGame = true;
        authorCharacter.GetCurrentCloset().ClosetActivated();
        authorCharacter.HideTheCharacter(true);
        UI_InGameKid.SetActive(false);
    }

    public void CloseHidingMechanics()
    {
        if (instantiatedHidingMechanics != null)
        {
            Destroy(instantiatedHidingMechanics);
            instantiatedHidingMechanics = null; // Clear the reference
        }

        // isOnHidingMiniGame = false;
        authorCharacter.GetCurrentCloset().ClosetDeActivated();
        authorCharacter.HideTheCharacter(false);
        UI_InGameKid.SetActive(true);
    }

}
