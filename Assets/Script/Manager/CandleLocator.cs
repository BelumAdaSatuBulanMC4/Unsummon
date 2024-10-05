// using UnityEngine;
// using Unity.Netcode; // For network-related components
// using System.Collections.Generic;

// public class CandleLocator : NetworkBehaviour
// {
//     public List<GameObject> candleObjects; // A list of all 20 candle objects in the scene
//     private List<GameObject> selectedCandles = new List<GameObject>(); // To store the 10 active candles

//     private const int numberOfCandlesToActivate = 10; // Number of candles to activate

//     public override void OnNetworkSpawn()
//     {
//         if (IsServer) // Only the server should manage which candles are active
//         {
//             SelectAndActivateCandles();
//             SyncActiveCandlesWithClients();
//         }
//     }

//     // Randomly select 10 candles from the pool of 20 and activate them
//     private void SelectAndActivateCandles()
//     {
//         // Create a list of available candle positions (all 20)
//         List<GameObject> availableCandles = new List<GameObject>(candleObjects);

//         // Shuffle the list and pick the first 10
//         for (int i = 0; i < numberOfCandlesToActivate; i++)
//         {
//             int randomIndex = Random.Range(0, availableCandles.Count);
//             GameObject selectedCandle = availableCandles[randomIndex];

//             // Activate the candle and add it to the selectedCandles list
//             selectedCandle.SetActive(true);
//             selectedCandles.Add(selectedCandle);

//             // Remove the selected candle from the available list to avoid duplicates
//             availableCandles.RemoveAt(randomIndex);
//         }
//     }

//     // Sync the active candles with all clients
//     [ServerRpc]
//     private void SyncActiveCandlesWithClients()
//     {
//         foreach (var candle in selectedCandles)
//         {
//             NetworkObject candleNetObj = candle.GetComponent<NetworkObject>();
//             if (candleNetObj != null)
//             {
//                 candleNetObj.Spawn(); // Make sure the object is spawned on the network
//             }
//         }
//     }
// }

