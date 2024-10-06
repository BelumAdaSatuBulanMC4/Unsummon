using UnityEngine;
using Unity.Netcode; // For network-related components
using System.Collections.Generic;

public class CandleLocator : NetworkBehaviour
{
    public GameObject candlePrefab;
    private Vector3[] positions;

    private bool isFirstRender = true;

    private void Start()
    {
        // positions = GameManager.instance.GetRandomSelectedPoints();
        // Debug.Log("Positions length: " + positions.Length);
        // for (int i = 0; i < positions.Length; i++)
        // {
        //     GameObject candleInstance = Instantiate(candlePrefab, positions[i], Quaternion.identity);
        //     candleInstance.GetComponent<NetworkObject>().Spawn();
        //     // candleInstance.transform.SetParent(transform);
        // }
    }

    // private void Update()
    // {
    //     if (isFirstRender)
    //     {
    //         // Instantiate candlePrefabs at each position
    //         for (int i = 0; i < positions.Length; i++)
    //         {
    //             GameObject candleInstance = Instantiate(candlePrefab, positions[i], Quaternion.identity);
    //             // candleInstance.transform.SetParent(transform);
    //         }
    //         isFirstRender = false;
    //     }
    // }
}

