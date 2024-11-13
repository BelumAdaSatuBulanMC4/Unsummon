using System;
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
    public Vector3[] spawnPoints;
    public Vector3[] randomSelectedPoints;

    public int[] randomIndexNumbers;

    private const int numberOfPointsToSelect = 10;

    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private TMP_Text secondaryText;
    [SerializeField] private TMP_Text informationText;
    [SerializeField] private Image splash;
    [SerializeField] private GameObject result;
    [SerializeField] private GameObject[] candleLocs;
    [SerializeField] private GameObject[] mirrorTeleports;
    Character currentChar;

    [Header("Audio in GamePlay")]
    public AudioClip pocongWinSound;
    public AudioClip kidsWinSound;
    public AudioClip environmentGamePlay;
    private AudioSource audioSource;

    //SWIFT PLUGIN HERE!!!
    private SwiftPlugin swiftPlugin;

    private bool curseRemoved = false;

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
        audioSource = GetComponent<AudioSource>();
        swiftPlugin = GetComponent<SwiftPlugin>();
    }

    private void Start()
    {
        FindAllPlayerKids();
        currentChar = FindAuthorCharacter();
        result.SetActive(false);
        UploadNumbersServerRpc();
        swiftPlugin.Initialize();
        audioSource.clip = environmentGamePlay;
        audioSource.Play();
        // audioSource.PlayOneShot(environmentGamePlay);
    }


    private void Update()
    {
        // FindAllPlayerKids();
        Debug.Log($"Achive items : {activeItems}/{totalItems}");
        kidsWin = activeItems == totalItems;
        pocongWin = killedKids == totalKids;
        if (kidsWin || pocongWin)
        {
            EndGame();
        }
    }

    public GameObject[] GetAllMirrors()
    {
        return mirrorTeleports;
    }

    public void StartSpeechRecognitionForCurseRemoval(Item cursedItem, System.Action<string, bool> feedbackCallback)
    {
        swiftPlugin.StartRecording();
        // StartCoroutine(CheckSpeechRecognizer(cursedItem, feedbackCallback));
    }

    public string GetTheSpeech()
    {
        return swiftPlugin.GetTranscribedTextFromSwift();
    }

    private IEnumerator CheckSpeechRecognizer(Item cursedItem, System.Action<string, bool> feedbackCallback)
    {
        while (swiftPlugin.IsSwiftRecording())
        {
            yield return null; // Wait until the recording stops
        }

        string recognizedText = swiftPlugin.GetTranscribedTextFromSwift();
        if (recognizedText.ToLower().Contains("buka"))
        {
            cursedItem.RemoveCurse(); // Remove the curse if the word is correctly recognized
            feedbackCallback?.Invoke("Correct! You said 'Buka'", true); // Trigger callback for correct feedback
        }
        else
        {
            feedbackCallback?.Invoke("Incorrect, try again.", false); // Trigger callback for incorrect feedback
        }
    }

    // public void StartSpeechRecognitionForCurseRemoval(Item cursedItem)
    // {
    //     swiftPlugin.StartRecording();
    //     StartCoroutine(CheckSpeechRecognizer(cursedItem));
    // }

    // private IEnumerator CheckSpeechRecognizer(Item cursedItem)
    // {
    //     while (swiftPlugin.IsSwiftRecording())
    //     {
    //         yield return null; // Wait until the recording stops
    //     }

    //     string recognizedText = swiftPlugin.GetTranscribedTextFromSwift();
    //     if (recognizedText.ToLower().Contains("buka"))
    //     {
    //         cursedItem.CurseDectivated(); // Remove the curse if the word is correctly recognized
    //         Debug.Log("Curse removed successfully " + cursedItem.isCursed);
    //     }
    // }

    public void CancelVoiceRecognition()
    {
        swiftPlugin.StopRecording();
    }

    public int[] GetRandomIndexNumbers()
    {
        return randomIndexNumbers;
    }

    int[] GenerateUniqueRandomNumbers(int min, int max, int length)
    {
        List<int> numbers = new List<int>();
        for (int i = min; i < max; i++)
        {
            numbers.Add(i);
        }


        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, numbers.Count);
            int temp = numbers[i];
            numbers[i] = numbers[randomIndex];
            numbers[randomIndex] = temp;
        }
        return numbers.GetRange(0, length).ToArray();
    }

    public Vector3[] GetRandomSelectedPoints()
    {
        return randomSelectedPoints;
    }

    public int[] getRandomGeneratedUniquePoints()
    {
        return randomIndexNumbers;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestRandomNumbersServerRpc()
    {
        if (IsServer)
        {
            // Server responds with the random numbers
            ShareRandomNumbersClientRpc(randomIndexNumbers);
        }
    }

    [ClientRpc]
    private void ShareRandomNumbersClientRpc(int[] receivedRandomNumbers)
    {
        randomIndexNumbers = receivedRandomNumbers;
        Debug.Log("Client received random numbers: " + string.Join(", ", randomIndexNumbers));
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
    public void UpdateKilledKids()
    {
        if (killedKids < totalKids)
        {
            AddKillKidsServerRpc();
        }
    }

    public void KidTurnedOnItem(Item item)
    {
        Debug.Log("type " + currentChar.GetTypeChar());
        if (activeItems < totalItems)
        {
            item.ItemActivated();
            // item.CurseDectivated();
            AddActiveItemsServerRpc();
        }
    }

    private Character FindAuthorCharacter()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();
        // Debug.Log("jumlah author " + allCharacters.Length);
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
        if (activeItems > 0 && !item.isCursed)
        {
            item.ItemDeactivated();
            // item.CurseActivated();
            //activate curse
            // item.CurseActivated();

            // item.DeActivatedCandle();
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
                // audioSource.Play();
                // audioSource.PlayOneShot(kidsWinSound);
            }
            else
            {
                victoryText.text = "Defeat";
                secondaryText.text = "Eternal Doom";
                informationText.text = "The pocong has devoured all the children, your family will be in hell forever.";
                splash.enabled = true;
                // audioSource.clip = pocongWinSound;
                // audioSource.Play();
                // audioSource.PlayOneShot(pocongWinSound);
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
                // audioSource.clip = pocongWinSound;
                // audioSource.Play();
                // audioSource.PlayOneShot(pocongWinSound);
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

    [ServerRpc(RequireOwnership = false)]
    public void UploadNumbersServerRpc()
    {
        randomIndexNumbers = GenerateUniqueRandomNumbers(0, candleLocs.Length, 6);
        UploadNumbersClientRpc(randomIndexNumbers);
    }

    [ClientRpc]
    public void UploadNumbersClientRpc(int[] receivedRandomNumbers)
    {
        randomIndexNumbers = receivedRandomNumbers;
        // ActivateObjectsAtRandomIndices();
    }
    // PLAY SOUND WIN AND LOSE
    private void PlayResultSound()
    {
        audioSource.Stop();
        if (kidsWin) { audioSource.clip = kidsWinSound; audioSource.Play(); }
        else if (pocongWin) { audioSource.clip = pocongWinSound; audioSource.Play(); }
    }

    // =================================================================================
    // COREMOTION!!

    public void StartGyroCoreMotion()
    {
        swiftPlugin.StartGyro();
    }

    public void StopGyroCoreMotion()
    {
        swiftPlugin.StopGyro();
    }

    public double GetRollValueFromSwift()
    {
        return swiftPlugin.GetRollValue();
    }

    public double GetPitchValueFromSwift()
    {
        return swiftPlugin.GetPitchValue();
    }

    public double GetYawValueFromSwift()
    {
        return swiftPlugin.GetYawValue();
    }

    //================================================================
    //COREHAPTIC
    public void StartConHapticFeedback(float intensity)
    {
        swiftPlugin.TriggerHapticFeedback(intensity);
    }

    public void StopConHapticFeedback()
    {
        swiftPlugin.StopHapticFeedback();
    }

}