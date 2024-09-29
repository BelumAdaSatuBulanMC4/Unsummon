using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTrial : MonoBehaviour
{
    public InputField relayCodeInput; // Input field for the join relay code
    public Button hostButton;         // Button to host a lobby
    public Button joinButton;         // Button to join a lobby

    // When the Host button is clicked, start a lobby
    public void OnHostButtonClicked()
    {
        // Load the lobby scene as a host
        SceneManager.LoadScene("LobbyScene");
        // Call your HostManager to create a Relay lobby here
        HostManager.Instance.StartHost();
    }

    // When the Join button is clicked, join a lobby using the relay code
    public void OnJoinButtonClicked()
    {
        string relayCode = relayCodeInput.text;
        if (!string.IsNullOrEmpty(relayCode))
        {
            // Load the lobby scene as a client
            SceneManager.LoadScene("LobbyScene");
            // Call your ClientManager to join the Relay lobby with the code
            ClientManager.Instance.JoinRelay(relayCode);
        }
        else
        {
            Debug.Log("Enter a valid relay code!");
        }
    }
}
