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

    private bool isFirstUpdate = true;

    public Vector3[] spawnPoints; // All possible spawn points (20 in your case)
    public Vector3[] randomSelectedPoints; // The random points selected and shared across players

    private const int numberOfPointsToSelect = 10;

    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text secondaryText;
    [SerializeField] private TMP_Text informationText;
    [SerializeField] private Image splash;

    [SerializeField] private Button homeButton;
    [SerializeField] private Button playAgainButton;
    [SerializeField] private GameObject result;
    [SerializeField] private GameObject candlePrefab;
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
        spawnPoints = new Vector3[20];
        spawnPoints[0] = new Vector3((float)-29.74, (float)10.46, 0);
        spawnPoints[1] = new Vector3((float)-21.39, (float)4.75, 0);
        spawnPoints[2] = new Vector3((float)-12.6, (float)9.33, 0);
        spawnPoints[3] = new Vector3((float)-3.34, (float)10.34, 0);
        spawnPoints[4] = new Vector3((float)5.21, (float)10.34, 0);
        spawnPoints[5] = new Vector3((float)-2.53, (float)0.4, 0);
        spawnPoints[6] = new Vector3((float)0.32, (float)-7.4, 0);
        spawnPoints[7] = new Vector3((float)-28.35, (float)-8.49, 0);
        spawnPoints[8] = new Vector3((float)-41.32, (float)-19.35, 0);
        spawnPoints[9] = new Vector3((float)19.56, (float)-4.62, 0);
        spawnPoints[10] = new Vector3((float)22.21, (float)9.29, 0);
        spawnPoints[11] = new Vector3((float)28.98, (float)14.07, 0);
        spawnPoints[12] = new Vector3((float)42.03, (float)14.17, 0);
        spawnPoints[13] = new Vector3((float)43.58, (float)8.68, 0);
        spawnPoints[14] = new Vector3((float)61.59, (float)8.68, 0);
        spawnPoints[15] = new Vector3((float)57.24, (float)13.41, 0);
        spawnPoints[16] = new Vector3((float)71.99, (float)-2.57, 0);
        spawnPoints[17] = new Vector3((float)50.35, (float)-7.41, 0);
        spawnPoints[18] = new Vector3((float)17.25, (float)-7.41, 0);
        spawnPoints[19] = new Vector3((float)-26.5, (float)19.68, 0);

        FindAllPlayerKids();
        currentChar = FindAuthorCharacter();
        result.SetActive(false);
        GenerateRandomPoints();
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
        if (IsHost && isFirstUpdate)
        {
            ShareRandomPointsClientRpc(randomSelectedPoints);
            isFirstUpdate = false;
        }
        Debug.Log($"Achive items : {activeItems}/{totalItems}");
        kidsWin = activeItems == totalItems;
        pocongWin = killedKids == totalKids;
        if (kidsWin || pocongWin)
        {
            EndGame();
        }
    }

    private void GenerateRandomPoints()
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogError("SpawnPoints array is empty!");
            return;
        }

        randomSelectedPoints = GetRandomPoints(spawnPoints, numberOfPointsToSelect);
        Debug.Log("Random spawn points generated on the server.");
    }

    private Vector3[] GetRandomPoints(Vector3[] points, int numberOfPoints)
    {
        List<Vector3> pointsList = new List<Vector3>(points);

        // Fisher-Yates shuffle algorithm to randomly shuffle the list
        for (int i = pointsList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            Vector3 temp = pointsList[i];
            pointsList[i] = pointsList[randomIndex];
            pointsList[randomIndex] = temp;
        }

        // Select the first 'numberOfPoints' from the shuffled list
        Vector3[] selectedPoints = new Vector3[numberOfPoints];
        for (int i = 0; i < numberOfPoints; i++)
        {
            selectedPoints[i] = pointsList[i];
        }

        return selectedPoints;
    }

    // Public method for clients to access the random array
    public Vector3[] GetRandomSelectedPoints()
    {
        return randomSelectedPoints;
    }

    [ClientRpc]
    private void ShareRandomPointsClientRpc(Vector3[] points)
    {
        randomSelectedPoints = points; // Sync the random points on all clients
        Debug.Log("Random points received by the client : " + randomSelectedPoints.Length);

        // Instead of spawning candles here, request the server to spawn them
        SpawnCandlesServerRpc(randomSelectedPoints);
    }

    // ServerRpc to spawn the candles on the server
    [ServerRpc(RequireOwnership = false)]
    private void SpawnCandlesServerRpc(Vector3[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            GameObject candleInstance = Instantiate(candlePrefab, points[i], Quaternion.identity);
            NetworkObject networkObject = candleInstance.GetComponent<NetworkObject>();

            // Ensure the object is spawned on the network by the server
            if (networkObject != null)
            {
                networkObject.Spawn();
            }
            else
            {
                Debug.LogError("NetworkObject component is missing from the candlePrefab.");
            }
        }

        Debug.Log("Candles spawned on the server.");
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

