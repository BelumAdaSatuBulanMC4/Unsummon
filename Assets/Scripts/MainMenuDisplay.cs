using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class MainMenuDisplay : MonoBehaviour
{
    public TMP_InputField codeRoomInput;
    private async void Start()
    {
        Debug.Log($"{DataPersistence.LoadUsername()}");
        try
        {
            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log($"Player ID: {AuthenticationService.Instance.PlayerId}");
        }
        catch (Exception e)
        {
            Debug.Log(e);
            return;
        }
    }

    public void StartHost()
    {
        HostManager.Instance.StartHost();
    }

    public void StartClient()
    {
        ClientManager.Instance.StartClient(codeRoomInput.text);
    }

}
