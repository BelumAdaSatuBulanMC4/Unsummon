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

    public void Initialise(LobbiesList lobbiesList, Lobby lobby)
    {
        this.lobbiesList = lobbiesList;
        this.lobby = lobby;

        lobbyNameText.text = lobby.Name;
        lobbyPlayersText.text = $"{lobby.Players.Count}/{lobby.MaxPlayers}";
        codeRoomText.text = lobby.Data["JoinCode"].Value;
    }

    public void Join()
    {
        Debug.Log("LobbyItem Join - berhasil diklik");
        lobbiesList.JoinAsync(lobby);
        // SceneManager.LoadScene("LobbyRoom");
    }
}

#if false
'only comment'
------------------
Initialise: Digunakan untuk menginisialisasi semua variable yang perlu diberikan value, dieksekusi pada LobbiesList
------------------
#endif