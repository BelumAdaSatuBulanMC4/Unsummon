using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Button settingButton;
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;
    [SerializeField] private GameObject UI_Settings;

    private async void Start()
    {
        Debug.Log($"{DataPersistence.LoadUsername()}");
        settingButton.onClick.AddListener(() => UI_Settings.SetActive(true));
        createRoomButton.onClick.AddListener(StartHost);
        joinRoomButton.onClick.AddListener(StartClient);

        if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        {
            Debug.Log("Start MainMenuManager - Host sudah berjalan, matikan terlebih dahulu.");
            NetworkManager.Singleton.Shutdown();
        }

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

#if UNITY_IOS
        Debug.Log("Start - MainMenuManager: If iOS berhasil dipanggil UNITY_IOS");
#endif

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            Debug.Log("Start - MainMenuManager: If iOS berhasil dipanggil RuntimePlatform");
        }
    }

    public void StartHost()
    {
        Debug.Log("Seharusnya start Host");
        // Masih belum nampil pop up lost nya
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
        else
        {
            UI_PopUpRoomNotFound.SetActive(true);
        }

    }

}
