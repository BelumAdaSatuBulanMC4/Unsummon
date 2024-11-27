using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public TMP_InputField codeRoomInput;

    [SerializeField] private Button createRoomButton;
    [SerializeField] private Button joinRoomButton;

    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;
    [SerializeField] private TextMeshProUGUI textIsConnect;
    private bool isConnect = false;

    private void Start()
    {
        Debug.Log($"{DataPersistence.LoadUsername()}");
        createRoomButton.onClick.AddListener(StartHost);
        joinRoomButton.onClick.AddListener(StartClient);
        SignInUser();
    }

    private void Update()
    {
        CheckInternetConnection();
        if (!isConnect) return;
        CheckIfPlayerStillJoinAsync();
        CheckSignInStatus();
    }

    private async void SignInUser()
    {
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
            PlayerInfo.Instance.SetPlayerId(AuthenticationService.Instance.PlayerId);
        }
        catch (Exception e)
        {
            Debug.Log($"Start MainMenuManager - login failed: {e.Message}");
            return;
        }
    }

    private async void CheckIfPlayerStillJoinAsync()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            Debug.Log("MainMenuManager Update - Host sudah berjalan, matikan terlebih dahulu.");
            HostManager.Instance.DeleteLobbyAsync();
            NetworkManager.Singleton.Shutdown();
        }
        else if (NetworkManager.Singleton.IsClient)
        {
            Debug.Log("MainMenuManager Update - Client sudah berjalan, matikan terlebih dahulu.");
            if (PlayerInfo.Instance.CurrentLobbyId != null)
            {
                await HostManager.Instance.RemovePlayerFromLobby(PlayerInfo.Instance.PlayerId, PlayerInfo.Instance.CurrentLobbyId);

            }
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void CheckSignInStatus()
    {
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            Debug.Log("Update MainMenuManager - User belum SignIn, retrying");
            SignInUser();
        }
    }

    private void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            textIsConnect.text = "No internet!";
            isConnect = false;
        }
        else
        {
            textIsConnect.text = "Connect!";
            isConnect = true;
        }
    }

    public void StartHost()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UI_PopUpLostConnection.SetActive(true);
            return;
        }
        Debug.Log("Seharusnya start Host");
        HostManager.Instance.StartHost();
        if (HostManager.Instance.lostConnection)
        {
            UI_PopUpLostConnection.SetActive(true);
            return;
        }
        SceneManager.LoadScene("LobbyRoom");
    }

    public async void StartClient()
    {
        Debug.Log("Seharusnya start Client");
        if (codeRoomInput.text.Length == 6)
        {
            await ClientManager.Instance.StartClient(codeRoomInput.text);
            // PlayerPrefs.SetString("RoomCode", codeRoomInput.text);
            // PlayerPrefs.Save();
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
            }

        }
        else if (codeRoomInput.text == "TESTING")
        {
            SceneManager.LoadScene("LobbyRoomTesting");
            HostManager.Instance.StartHost();
        }
        else
        {
            UI_PopUpRoomNotFound.SetActive(true);
        }

    }

}
