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

    [Header("PopUp Error")]
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;
    private bool isRefreshing;
    private bool isJoining;


    private void OnEnable()
    {
        refreshButton.onClick.AddListener(RefreshList);
        RefreshList();
    }

    public async void RefreshList()
    {
        if (isRefreshing) return;

        isRefreshing = true;

        try
        {
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

            foreach (Transform child in lobbyItemParent)
            {
                Destroy(child.gameObject);
            }

            foreach (Lobby lobby in lobbies.Results)
            {
                LobbyItem lobbyItemInstance = Instantiate(lobbyItemPrefab, lobbyItemParent);
                lobbyItemInstance.Initialise(this, lobby);
            }
            Debug.Log("LobbiesList RefreshList - berhasil dijalankan");
        }
        catch (LobbyServiceException e)
        {
            Debug.LogError($"RefreshList - try RefreshList failed: {e.Message}");
        }

        isRefreshing = false;
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
        }

        isJoining = false;
    }
}


#if false 
'only comment'
------------------
Initialise: Digunakan untuk menginisialisasi semua variable yang perlu diberikan value, dieksekusi pada LobbiesList
------------------
#endif
