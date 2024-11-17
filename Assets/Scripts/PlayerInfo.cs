using Unity.Services.Authentication;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static PlayerInfo Instance { get; private set; }
    public string PlayerId { get; private set; }
    public string CurrentLobbyId { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetPlayerId(string playerId)
    {
        PlayerId = playerId;
    }

    public void ChangeCurrentLobbyId(string lobbyId)
    {
        CurrentLobbyId = lobbyId;
    }
}

