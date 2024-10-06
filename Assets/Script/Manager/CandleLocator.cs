using UnityEngine;
using Unity.Netcode; // For network-related components
using System.Collections.Generic;

public class CandleLocator : MonoBehaviour
{
    public int[] randomIndexNumbers;
    private bool isFirstRender = true;
    [SerializeField] private GameObject[] candleLocs;

    void Start()
    {
        randomIndexNumbers = GameManager.instance.GetRandomIndexNumbers();
    }

    private void Update()
    {
        if (isFirstRender)
        {
            ActivateObjectsAtRandomIndices();
            isFirstRender = false;
        }
    }

    private void ActivateObjectsAtRandomIndices()
    {
        if (randomIndexNumbers == null || randomIndexNumbers.Length == 0)
        {
            Debug.LogError("No random index numbers to activate items.");
            return;
        }

        foreach (GameObject obj in candleLocs)
        {
            obj.SetActive(false);
        }

        foreach (int index in randomIndexNumbers)
        {
            if (index >= 0 && index < candleLocs.Length)
            {
                candleLocs[index].SetActive(true);
                Debug.Log("Activated GameObject at index: " + index);
            }
        }
    }
}


