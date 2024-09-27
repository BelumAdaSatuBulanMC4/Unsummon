using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int itemValue = 0;

    public void ChangeVariable()
    {
        itemValue += 1;
        Debug.Log("Item " + name + " value changed to: " + itemValue);
    }
    public void ResetValue()
    {
        itemValue = 0;
        Debug.Log("Item " + name + " value reset to: " + itemValue);
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
