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
    private Character tempCharacter;

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
        authorCharacter = FindAuthorCharacter();
        joystickGame = FindObjectOfType<JoystickGame>();
        Debug.Log("new character is Update: " + authorCharacter);
        InstantiateUIForCharacter(authorCharacter);


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
            else if (authorCharacter.GetTypeChar() == "Pocong")
            {
                if (authorCharacter.GetCurrentItem() != null)
                {
                    InteractButton.GetComponentInChildren<Button>().onClick.AddListener(InteractedWithItem);
                }
            }
        }
        else
        {
            Debug.LogWarning("no one have isAuthor");
            DeActivatedButton();
        }

        // HandleButtonInteraction();
    }

    public void SetAuthorCharacter(Character character)
    {
        // Debug.Log("new character is SetAuthorCharacter: " + character);
        authorCharacter = character;
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
        Debug.Log($"InteractedWithItem - Masuk fungsi InteractedWithItem");
        Debug.Log($"InteractedWithItem - GetTypeChar: {authorCharacter.GetTypeChar()}");
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
            Debug.Log($"InteractedWithItem - Masuk Else If Pocong");
            if (authorCharacter.GetCurrentItem().isActivated)
            {
                Debug.Log($"InteractedWithItem - Klo lilinnya Active Pocong bisa interact");
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
            // Debug.Log("Interact sama closet!");
            if (!authorCharacter.GetCurrentCloset().isUsed)
            {
                // Debug.Log("Harusnya sih hiding UI muncul");
                UI_InGame.instance.OpenHidingMechanics();
                // UI_HidingMechanics.instance.CurrentCloset(authorCharacter.GetCurrentCloset());
            }
        }
    }

    public void SwitchToSpirit()
    {
        UI_InGameKid.SetActive(false);
        UI_InGameSpirit.SetActive(true);
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
        Debug.Log("author character is null or not?" + authorCharacter == null);
        Character[] allCharacters = FindObjectsOfType<Character>();
        // Debug.Log("jumlah author " + allCharacters.Length);
        if (authorCharacter == null && allCharacters.Length > 0)
        {
            foreach (Character character in allCharacters)
            {
                if (character.GetIsAuthor())
                {
                    return character;
                }
            }
        }
        else if (authorCharacter != null && allCharacters.Length > 0)
        {
            return authorCharacter;
        }

        Debug.Log("jumlah author " + allCharacters.Length);
        return null;
    }

    public void InstantiateUIForCharacter(Character character)
    {
        // Debug.Log("new character is di dalem InstantiateUI: " + character);

        // if (tempCharacter != character)
        // {
        if (character is PlayerKid)
        {
            // currentInGameController = UI_InGameKid;
            UI_InGameKid.SetActive(true);
            // Instantiate(currentInGameController, transform);
        }
        else if (character is Pocong)
        {
            // currentInGameController = UI_InGamePocong;
            UI_InGamePocong.SetActive(true);
            // Instantiate(currentInGameController, transform);
        }
        else if (character is PlayerSpirit)
        {
            // Debug.Log("InGame spirit " + character.ToString());
            // currentInGameController = UI_InGameSpirit;
            UI_InGameSpirit.SetActive(true);
            UI_InGameKid.SetActive(false);
            // Instantiate(currentInGameController, transform);
        }
        else
        {
            Debug.LogWarning("Unknown character type.");
        }
        // tempCharacter = character;
        // }
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
        // else
        // {
        //     instantiatedHidingMechanics.SetActive(true); // Reactivate if already instantiated
        // }

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
        // Debug.Log("Masuk ke CloseHidingMechanics, Minigame harusnya udh ga Kelihatan!");

        if (instantiatedHidingMechanics != null)
        {
            Debug.Log("Masuk ke Destroy, Kelihatan! " + authorCharacter == null);
            authorCharacter.GetCurrentCloset().ClosetDeActivated();
            authorCharacter.ResetHidingCooldown();
            authorCharacter.HideTheCharacter(false);
            UI_InGameKid.SetActive(true);
            Destroy(instantiatedHidingMechanics);
            instantiatedHidingMechanics = null; // Clear the reference
        }

        // isOnHidingMiniGame = false;
        // Debug.Log("kelihatan? apakah karakter null : " + authorCharacter == null);
        // Debug.Log("kelihatan gak???????????????????????? DUA");
        // Debug.Log("harusnya UI InGameKid udah kelihatan lagi!");
        // HIDE here
    }

}
