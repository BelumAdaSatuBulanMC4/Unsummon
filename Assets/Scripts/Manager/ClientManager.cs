using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class ClientManager : MonoBehaviour
{
    public static ClientManager Instance { get; private set; }

    public bool roomNotFound = false;
    public bool roomFull = false;
    public bool lostConnection = false;
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
    public async Task StartClient(string codeRoom)
    {
        // Reset semua status sebelum mencoba
        roomNotFound = false;
        roomFull = false;
        lostConnection = false;

        JoinAllocation allocation;
        try
        {
            allocation = await RelayService.Instance.JoinAllocationAsync(codeRoom);
        }
        catch (RelayServiceException ex)
        {
            Debug.LogError($"JOIN RELAY FAILED: {ex}");
            Debug.LogError(ex.ErrorCode);
            if (
                ex.ErrorCode == (int)RelayExceptionReason.AllocationNotFound ||
                ex.ErrorCode == (int)RelayExceptionReason.JoinCodeNotFound ||
                ex.ErrorCode == (int)RelayExceptionReason.InvalidRequest
                )
            {
                roomNotFound = true;
            }
            else if (ex.ErrorCode == (int)RelayExceptionReason.Max)
            {
                roomFull = true;
            }
            else
            {
                lostConnection = true;
            }
            return;
        }
        var relayServerData = new RelayServerData(allocation, "dtls");

        Debug.Log("Trying to connect to: " + codeRoom);

        NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

        // if (NetworkManager.Singleton.IsHost || NetworkManager.Singleton.IsClient)
        // {
        //     Debug.Log("StartClient - Host sudah berjalan, matikan terlebih dahulu.");
        //     NetworkManager.Singleton.Shutdown();
        // }


        if (NetworkManager.Singleton.StartClient())
        {
            Debug.Log("Client connect successfully!");
        }
        else
        {
            Debug.LogError("Failed to start client!");
        }
    }
}
