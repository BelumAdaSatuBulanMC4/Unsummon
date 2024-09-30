using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager instance;
    private int totalItems = 5;
    private int activeItems = 0;


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

    public void KidTurnedOnItem(Item item)
    {
        if (activeItems < totalItems)
        {
            item.ChangeVariable();
            activeItems++;
            Debug.Log("Kid turned on an item. Active items: " + activeItems);

            if (activeItems == totalItems)
            {
                EndGame(true);
            }
        }
    }

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
            Debug.Log("Kids have won the game!");
        }
        else
        {
            Debug.Log("Pocong has won the game!");
        }

        // Restart or end the game
        // Reset all items, notify players, etc.
    }
}

