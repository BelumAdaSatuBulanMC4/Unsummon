using UnityEngine;
using Unity.Netcode; // For network-related components
using System.Collections.Generic;
using TMPro;

public class CandleLocator : MonoBehaviour
{
    public int[] randomIndexNumbers;
    private bool isFirstRender = true;
    [SerializeField] private GameObject[] candleLocs;

    // [SerializeField] private TMP_Text text;
    // int koontool = 5;
    void Start()
    {
        randomIndexNumbers = GameManager.instance.GetRandomIndexNumbers();
        // text.text = "Random Index Numbers: " + randomIndexNumbers[0] + ", " + randomIndexNumbers[1] + ", " + randomIndexNumbers[2] + ", " + randomIndexNumbers[3];
        ActivateObjectsAtRandomIndices();
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


