using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private int totalItems = 5;
    private int totalKids;
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
            // Destroy(gameObject);
        }
    }

    private void Start()
    {
        FindAllPlayerKids();
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

    public void FindAllPlayerKids()
    {
        PlayerKid[] allPlayerKids = FindObjectsOfType<PlayerKid>();
        Debug.Log("total kids : " + allPlayerKids.Length);
        totalKids = allPlayerKids.Length;
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

    public void UpdateKilledKids()
    {
        if (killedKids < totalKids)
        {
            killedKids++;
            Debug.Log("Killed kids " + killedKids + " / " + totalKids);
            if (killedKids == totalKids)
            {
                Debug.Log("Pocong menang");
                kidsWin = false;
                pocongWin = true;
                EndGame(pocongWin);
            }
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

    public bool GetPocongWin() => pocongWin;
    public bool GetKidsWin() => kidsWin;

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
            SceneManager.LoadScene("WinningCondition");
        }
        else
        {
            // Debug.Log("Pocong has won the game!");
            SceneManager.LoadScene("WinningCondition");
        }
    }
}

