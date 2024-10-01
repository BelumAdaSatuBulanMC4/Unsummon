using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // public int itemValue = 0;
    public bool isActivated = false;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (isActivated)
        {
            anim.SetFloat("isCandleActive", 1);
        }
        else
        {
            anim.SetFloat("isCandleActive", 0);
        }
    }

    private void Update()
    {
        if (isActivated)
        {
            anim.SetFloat("isCandleActive", 1);
        }
        else
        {
            anim.SetFloat("isCandleActive", 0);
        }
    }

    public void ChangeVariable()
    {
        isActivated = true;
        anim.SetFloat("isCandleActive", 1);
        // GetComponent<SpriteRenderer>().color = Color.green;
    }
    public void ResetValue()
    {
        isActivated = false;
        anim.SetFloat("isCandleActive", 0);
        // GetComponent<SpriteRenderer>().color = Color.white;
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
