using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbiesList : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform lobbyItemParent;
    [SerializeField] private LobbyItem lobbyItemPrefab;
    [SerializeField] private Button refreshButton;
    [SerializeField] private Button backButton;

    [Header("PopUp Error")]
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;
    [SerializeField] private GameObject UI_PopUpErrorJoinLobby;
    [SerializeField] private GameObject textNoRoomAvailable;
    [SerializeField] private GameObject textLoading;
    private bool isRefreshing;
    private bool isJoining;
    private int totalRoomAvailable;

    private const float RefreshCooldown = 5f;
    private float lastRefreshTime;

    private void Start()
    {
        refreshButton.onClick.AddListener(RefreshList);
        backButton.onClick.AddListener(() => SceneManager.LoadScene("MainMenu"));
        RefreshList();
    }

    // private void OnEnable()
    // {
    //     refreshButton.onClick.AddListener(RefreshList);
    //     RefreshList();
    // }
    private void Update()
    {
        textLoading.SetActive(isRefreshing);
        // textNoRoomAvailable.SetActive(!isRefreshing);
    }

    public async void RefreshList()
    {
        // if (isRefreshing) return;
        if (isRefreshing || Time.time - lastRefreshTime < RefreshCooldown) return;

        lastRefreshTime = Time.time;
        isRefreshing = true;
        textNoRoomAvailable.SetActive(false);
        // refreshButton.interactable = false;
        // textLoading.SetActive(true);

        try
        {
            foreach (Transform child in lobbyItemParent) Destroy(child.gameObject);

            QueryLobbiesOptions options = new()
            {
                Count = 25,
                Filters = new List<QueryFilter>()
                {
                    new(
                        field: QueryFilter.FieldOptions.AvailableSlots,
                        op: QueryFilter.OpOptions.GT,
                        value: "0"),
                    new(
                        field: QueryFilter.FieldOptions.IsLocked,
                        op: QueryFilter.OpOptions.EQ,
                        value: "0")
                }
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);

            // foreach (Lobby lobby in lobbies.Results)
            for (int i = 0; i < lobbies.Results.Count; i++)
            {
                LobbyItem lobbyItemInstance = Instantiate(lobbyItemPrefab, lobbyItemParent);
                // lobbyItemInstance.Initialise(this, lobby);
                lobbyItemInstance.Initialise(this, lobbies.Results[i], $"Room {i + 1}");
            }
            totalRoomAvailable = lobbies.Results.Count;
            if (totalRoomAvailable == 0) textNoRoomAvailable.SetActive(true);
            else textNoRoomAvailable.SetActive(false);
            Debug.Log($"LobbiesList RefreshList - berhasil dijalankan, total lobby: {totalRoomAvailable}");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"RefreshList - try RefreshList failed: {e.Message}");
        }

        isRefreshing = false;
        textLoading.SetActive(false);
        // refreshButton.interactable = true;
    }

    public async void JoinAsync(Lobby lobby)
    {
        if (isJoining) { return; }

        isJoining = true;

        try
        {

            var currentLobby = await Lobbies.Instance.GetLobbyAsync(lobby.Id);
            if (currentLobby.Players.Exists(player => player.Id == AuthenticationService.Instance.PlayerId))
            {
                Debug.Log("JoinAsync - Player sudah pernah bergabung dengan lobby ini.");
                return;
            }

            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(lobby.Id);
            string joinCode = joiningLobby.Data["JoinCode"].Value;

            Debug.Log($"LobbiesList JoinAsync - RoomCode: {joinCode}");
            await ClientManager.Instance.StartClient(joinCode);
            Debug.Log($"LobbiesList JoinAsync - berhasil dijalankan");
            // SceneManager.LoadScene("LobbyRoom");
            if (ClientManager.Instance.roomNotFound)
            {
                UI_PopUpRoomNotFound.SetActive(true);
            }
            else if (ClientManager.Instance.roomFull)
            {
                UI_PopUpFull.SetActive(true);
            }
            else if (ClientManager.Instance.lostConnection)
            {
                UI_PopUpLostConnection.SetActive(true);
            }
            else
            {
                SceneManager.LoadScene("LobbyRoom");
                Debug.Log("Harusnya berhasil join");
            }
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"JoinAsync - try JoinLobbyByIdAsync failed: {e.Message}");
            UI_PopUpErrorJoinLobby.SetActive(true);
        }

        isJoining = false;
    }
}


