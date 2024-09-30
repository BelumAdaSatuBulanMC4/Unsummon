using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalItems = 5;
    private int totalKids = 4;
    private int activeItems = 0;
    private int killedKids = 0;
    private bool kidsWin;
    private bool pocongWin;


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
    public void addActivatedItems()
    {
        if (activeItems >= totalItems)
        {
            EndGame(true);
            return;
        }

        activeItems++;
        Debug.Log("Kid turned on an item!");
    }

    public int GetActiveItems()
    {
        return activeItems;
    }

    public int GetTotalItems()
    {
        return totalItems;
    }

    public void subActivatedItems()
    {
        if (activeItems > 0)
        {
            activeItems--;
            Debug.Log("Pocong turned off an item!");
        }
    }

    public void updateKilledKids()
    {
        if (killedKids < totalKids)
        {
            killedKids++;
        }
        else
        {
            kidsWin = false;
            pocongWin = true;
            EndGame(kidsWin);
        }
    }

    public void KidTurnedOnItem(Item item)
    {
        if (activeItems < totalItems)
        {
            item.ChangeVariable();
            activeItems++;
            Debug.Log("Kid turned on an item. Active items: " + activeItems);

            if (activeItems == totalItems)
            {
                kidsWin = true;
                pocongWin = false;
                EndGame(kidsWin);
            }
        }
    }

    public bool getPocongWin() => pocongWin;
    public bool getKidsWin() => kidsWin;

    public void PocongTurnedOffItem(Item item)
    {
        if (activeItems > 0)
        {
            item.ResetValue();
            activeItems--;
            Debug.Log("Pocong turned off an item. Active items: " + activeItems);
        }
    }

    private void EndGame(bool kidsWon)
    {
        if (kidsWon)
        {
            // Debug.Log("Kids have won the game!");
            SceneManager.LoadScene("");
        }
        else
        {
            Debug.Log("Pocong has won the game!");
            SceneManager.LoadScene("MiniGameScene");
        }
    }
}

