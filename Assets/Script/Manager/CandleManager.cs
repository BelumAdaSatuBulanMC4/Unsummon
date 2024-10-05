using UnityEngine;
using Unity.Netcode; // For network-related components
using System.Collections.Generic;

public class CandleManager : NetworkBehaviour
{
    public List<GameObject> candleObjects; // A list of all 20 candle objects in the scene
    private List<GameObject> selectedCandles = new List<GameObject>(); // To store the 10 active candles

    private const int numberOfCandlesToActivate = 10; // Number of candles to activate

    public override void OnNetworkSpawn()
    {
        if (IsServer) // Only the server should manage which candles are active
        {
            SelectAndActivateCandles();
            NotifyClientsAboutActiveCandlesClientRpc(GetSelectedCandlesIndices());
        }
    }

    // Randomly select 10 candles from the pool of 20 and activate them
    private void SelectAndActivateCandles()
    {
        List<GameObject> availableCandles = new List<GameObject>(candleObjects);

        for (int i = 0; i < numberOfCandlesToActivate; i++)
        {
            int randomIndex = Random.Range(0, availableCandles.Count);
            GameObject selectedCandle = availableCandles[randomIndex];

            // Activate the candle and add it to the selectedCandles list
            selectedCandle.SetActive(true);
            selectedCandles.Add(selectedCandle);

            // Remove the selected candle from the available list to avoid duplicates
            availableCandles.RemoveAt(randomIndex);
        }
    }

    // Get the indices of the selected candles to send to clients
    private int[] GetSelectedCandlesIndices()
    {
        int[] indices = new int[selectedCandles.Count];
        for (int i = 0; i < selectedCandles.Count; i++)
        {
            indices[i] = candleObjects.IndexOf(selectedCandles[i]);
        }
        return indices;
    }

    // This ClientRpc will be called on all clients to activate the correct candles
    [ClientRpc]
    private void NotifyClientsAboutActiveCandlesClientRpc(int[] candleIndices)
    {
        foreach (int index in candleIndices)
        {
            if (index >= 0 && index < candleObjects.Count)
            {
                candleObjects[index].SetActive(true); // Activate the candles on the client
            }
        }
    }
}
