using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemValue = 0;
    public bool isActivated = false;

    public void ChangeVariable()
    {
        isActivated = true;
        GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void ResetValue()
    {
        isActivated = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public void DisplayInteraction()
    {
        if (GetComponent<SpriteRenderer>().color == Color.green)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
