using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI lobbyPlayersText;
    [SerializeField] private TextMeshProUGUI codeRoomText;
    [SerializeField] private Button joinButton;

    private LobbiesList lobbiesList;
    private Lobby lobby;

    private void Start()
    {
        joinButton.onClick.AddListener(Join);
    }

    public void Initialise(LobbiesList lobbiesList, Lobby lobby, string roomName)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;

        if (lobby == null)
        {
            Debug.LogError("Initialise LobbyItem - Lobby is null in Initialise!");
            return;
        }

        lobbyNameText.text = roomName;
        lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        codeRoomText.text = lobby.Data["JoinCode"].Value;
        Debug.Log($"Initialise - JoinCode harusnya: {lobby.Data["JoinCode"].Value}");
        Debug.Log($"Initialise - JoinCode harusnya: {lobby.Data}");
    }

    public void Join()
    {
        Debug.Log("LobbyItem Join - berhasil diklik");
        lobbiesList.JoinAsync(lobby);
        PlayerInfo.Instance.ChangeCurrentLobbyId(lobby.Id);
        Debug.Log($"LobbyItem Join -  LobbyID: {lobby.Id}");
        // SceneManager.LoadScene("LobbyRoom");
    }
}
