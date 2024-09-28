using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_CandleCounter : MonoBehaviour
{
    private TextMeshProUGUI candleText; // Reference to the TextMeshPro UI component

    private void Awake()
    {
        candleText = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        // Update the UI at the start
        UpdateCandleCounter(GameManager.instance.GetActiveItems());
    }

    private void Update()
    {
        UpdateCandleCounter(GameManager.instance.GetActiveItems());
    }
    // private void OnEnable()
    // {
    //     // Subscribe to GameManager events if needed (e.g., when activeItems change)
    //     GameManager.instance.OnItemChanged += UpdateCandleCounter;
    // }

    // private void OnDisable()
    // {
    //     // Unsubscribe when disabled
    //     GameManager.instance.OnItemChanged -= UpdateCandleCounter;
    // }

    // Method to update the candle counter text
    public void UpdateCandleCounter(int activeItems)
    {
        candleText.text = activeItems + "/" + GameManager.instance.GetTotalItems();
    }
}
