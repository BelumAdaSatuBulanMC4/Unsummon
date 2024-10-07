using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class GamePlayManager : NetworkBehaviour
{
    public GameObject playerKidPrefab;
    public GameObject playerPocongPrefab;
    private Vector3 spawnKidPosition = new(0, 0, 0); // Posisi Spawn playerKid
    private Vector3 spawnPocongPosition = new(0, 0, 0); // Posisi Spawn playerKid

    [SerializeField] GameObject roleKidScene;
    [SerializeField] GameObject rolePocongScene;


    private void Start()
    {

    }

    // Fungsi yang dipanggil setelah scene GamePlay di-load
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SpawnPlayers();
        }
    }

    // Spawn semua pemain di posisi yang sama
    private void SpawnPlayers()
    {
        if (!IsServer) return;
        int randomPocongId = Random.Range(0, 3);
        foreach (var client in NetworkManager.ConnectedClientsList)
        {
            if ((int)client.ClientId == randomPocongId)
            // if ((int)client.ClientId == 0)
            // if (false)
            {
                GameObject playerInstance = Instantiate(playerPocongPrefab, spawnPocongPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                ShowRolePocongSceneClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { client.ClientId }
                    }
                });
            }
            else
            {
                GameObject playerInstance = Instantiate(playerKidPrefab, spawnKidPosition, Quaternion.identity);
                playerInstance.GetComponent<NetworkObject>().SpawnAsPlayerObject(client.ClientId);
                // Panggil ClientRpc hanya untuk client ini
                ShowRoleKidSceneClientRpc(new ClientRpcParams
                {
                    Send = new ClientRpcSendParams
                    {
                        TargetClientIds = new ulong[] { client.ClientId }
                    }
                });
            }

        }
    }

    [ClientRpc]
    private void ShowRolePocongSceneClientRpc(ClientRpcParams clientRpcParams = default)
    {
        rolePocongScene.SetActive(true);
        StartCoroutine(DisableSceneAfterDelay(rolePocongScene, 15f));
    }

    [ClientRpc]
    private void ShowRoleKidSceneClientRpc(ClientRpcParams clientRpcParams = default)
    {
        roleKidScene.SetActive(true);
        StartCoroutine(DisableSceneAfterDelay(roleKidScene, 5f));
    }

    // Coroutine untuk menonaktifkan scene setelah delay tertentu
    private IEnumerator DisableSceneAfterDelay(GameObject scene, float delay)
    {
        yield return new WaitForSeconds(delay);
        scene.SetActive(false);
    }


}

