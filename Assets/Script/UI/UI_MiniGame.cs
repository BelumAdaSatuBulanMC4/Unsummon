using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_MiniGame : MonoBehaviour
{
    public static UI_MiniGame instance;
    [SerializeField] Button candleButton;
    [SerializeField] Button cancelGame;
    [SerializeField] TMP_Text candleText;
    [SerializeField] TMP_Text instructionText;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private Item item;

    private void Start()
    {
        candleButton.onClick.AddListener(CandleInteraction);
        cancelGame.onClick.AddListener(CancelMiniGame);
    }

    private void Update()
    {
        if (item != null)
        {
            if (item.isActivated)
            {
                instructionText.text = "Hold to snuff out the candle";
            }
            else
            {
                instructionText.text = "Hold to light the candle";
            }
        }
    }

    public void CandleInteraction()
    {
        if (item.isActivated)
        {
            GameManager.instance.PocongTurnedOffItem(item);
            gameObject.SetActive(false);
            // UI_InGame.instance.CloseMiniGame();
        }
        else
        {
            GameManager.instance.KidTurnedOnItem(item);
            gameObject.SetActive(false);
            // UI_InGame.instance.CloseMiniGame();
        }
    }

    public void CurrentItem(Item newItem)
    {
        item = newItem;
    }

    public void CancelMiniGame()
    {
        gameObject.SetActive(false);
    }
}
