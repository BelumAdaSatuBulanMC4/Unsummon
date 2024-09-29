using UnityEngine;
using UnityEngine.UI;

public class LobbyManagerTrial : MonoBehaviour
{
    public Text relayCodeText;  // To display the relay code
    public Text[] playerNameTexts;  // Array to display player names (up to 5 players)

    void Start()
    {
        // Display relay code for sharing (only host needs to know this)
        relayCodeText.text = "Relay Code: " + HostManager.Instance.GetRelayCode();

        // Update player names when players join the lobby
        UpdatePlayerList();
    }

    // Update the UI when a player joins
    public void UpdatePlayerList()
    {
        var players = HostManager.Instance.GetPlayerNames();  // Get the list of player names

        for (int i = 0; i < playerNameTexts.Length; i++)
        {
            if (i < players.Count)
            {
                playerNameTexts[i].text = players[i];
            }
            else
            {
                playerNameTexts[i].text = "Waiting for player...";
            }
        }
    }
}
