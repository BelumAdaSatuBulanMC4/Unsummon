using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private int totalItems = 5;
    private int totalKids;
    private int activeItems = 0;
    private int killedKids = 0;
    private bool kidsWin;
    private bool pocongWin;

    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text secondaryText;
    [SerializeField] private TMP_Text informationText;
    [SerializeField] private Image splash;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private GameObject result;
    Character currentChar;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            // Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindAllPlayerKids();
        currentChar = FindAuthorCharacter();
        result.SetActive(false);
    }
    // public void addActivatedItems()
    // {
    //     if (activeItems >= totalItems)
    //     {
    //         EndGame(true);
    //         return;
    //     }

    //     activeItems++;
    //     Debug.Log("Kid turned on an item!");
    // }

    private void Update()
    {
        Debug.Log($"Achive items : {activeItems}/{totalItems}");
        kidsWin = activeItems == totalItems;
        pocongWin = killedKids == totalKids;
        if (kidsWin || pocongWin)
        {
            EndGame();
        }
    }

    public void FindAllPlayerKids()
    {
        PlayerKid[] allPlayerKids = FindObjectsOfType<PlayerKid>();
        Debug.Log("total kids : " + allPlayerKids.Length);
        totalKids = allPlayerKids.Length;
    }

    public int GetActiveItems()
    {
        return activeItems;
    }

    public int GetTotalItems()
    {
        return totalItems;
    }

    // public void subActivatedItems()
    // {
    //     if (activeItems > 0)
    //     {
    //         activeItems--;
    //         Debug.Log("Pocong turned off an item!");
    //     }
    // }

    public void UpdateKilledKids()
    {
        if (killedKids < totalKids)
        {
            // killedKids++;
            AddKillKidsServerRpc();
            // Debug.Log("Killed kids " + killedKids + " / " + totalKids);
            // if (killedKids == totalKids)
            // {
            //     Debug.Log("Pocong menang");
            //     // kidsWin = false;
            //     // pocongWin = true;
            //     PocongWinServerRpc();
            //     EndGame();
            // }
        }
    }

    public void KidTurnedOnItem(Item item)
    {
        Debug.Log("type " + currentChar.GetTypeChar());
        if (activeItems < totalItems)
        {
            item.ChangeVariable();
            // activeItems++;
            AddActiveItemsServerRpc();
            // Debug.Log("Kid turned on an item. Active items: " + activeItems);

            // if (activeItems == totalItems)
            // {
            //     Debug.Log($"is win {kidsWin}");
            //     // kidsWin = true;
            //     // pocongWin = false;
            //     KidWinServerRpc();
            //     EndGame();
            // }
        }
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

    public bool GetPocongWin() => pocongWin;
    public bool GetKidsWin() => kidsWin;

    public void PocongTurnedOffItem(Item item)
    {
        if (activeItems > 0)
        {
            item.ResetValue();
            // activeItems--;
            DecActiveItemsServerRpc();
            Debug.Log("Pocong turned off an item. Active items: " + activeItems);
        }
    }

    private void EndGame()
    {
        result.SetActive(true);
        if (currentChar.GetTypeChar() == "Player")
        {
            if (kidsWin)
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
        else
        {
            if (pocongWin)
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
        // LoadGamePlaySceneServerRpc();
        // if (kidsWon)
        // {
        //     // Debug.Log("Kids have won the game!");
        // SceneManager.LoadScene("WinningCondition");
        // }
        // else
        // {
        //     // Debug.Log("Pocong has won the game!");
        //     SceneManager.LoadScene("WinningCondition");
        // }
    }

    // TAMBAH ITEMS
    [ServerRpc(RequireOwnership = false)]
    public void AddActiveItemsServerRpc()
    {
        AddActiveItemsClientRpc();
    }
    [ClientRpc]
    public void AddActiveItemsClientRpc()
    {
        activeItems++;
    }

    // KURANGI ITEMS
    [ServerRpc(RequireOwnership = false)]
    public void DecActiveItemsServerRpc()
    {
        DecActiveItemsClientRpc();
    }
    [ClientRpc]
    public void DecActiveItemsClientRpc()
    {
        activeItems--;
    }



    [ServerRpc(RequireOwnership = false)]
    public void AddKillKidsServerRpc()
    {
        AddKillKidsClientRpc();
    }

    [ClientRpc]
    public void AddKillKidsClientRpc()
    {
        killedKids++;
    }

    // [ServerRpc]
    // public void KidWinServerRpc()
    // {
    //     KidWinClientRpc();
    // }
    // [ClientRpc]
    // public void KidWinClientRpc()
    // {
    //     kidsWin = true;
    //     pocongWin = false;
    // }
    // [ServerRpc]
    // public void PocongWinServerRpc()
    // {
    //     PocongWinClientRpc();
    // }
    // [ClientRpc]
    // public void PocongWinClientRpc()
    // {
    //     pocongWin = true;
    //     kidsWin = false;
    // }

    // [ServerRpc(RequireOwnership = false)]
    // private void LoadGamePlaySceneServerRpc()
    // {
    //     NetworkManager.SceneManager.LoadScene("WinningCondition", LoadSceneMode.Single);
    // }
}

