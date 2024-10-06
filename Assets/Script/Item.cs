using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Item : NetworkBehaviour
{
    public bool isActivated = false;
    private Animator anim;
    private Light2D light;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        light = GetComponentInChildren<Light2D>();
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
            light.enabled = true;
        }
        else
        {
            anim.SetFloat("isCandleActive", 0);
            light.enabled = false;
        }
    }

    public void ChangeVariable()
    {
        ChangeVariableServerRpc();
    }
    public void ResetValue()
    {
        ResetValueServerRpc();
    }


    [ServerRpc(RequireOwnership = false)]
    public void ChangeVariableServerRpc()
    {
        ChangeVariableClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ResetValueServerRpc()
    {
        ResetValueClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    public void ChangeVariableClientRpc()
    {
        isActivated = true;
        anim.SetFloat("isCandleActive", 1);
        // GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc(RequireOwnership = false)]
    public void ResetValueClientRpc()
    {
        isActivated = false;
        anim.SetFloat("isCandleActive", 0);
        // GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void ActivatedCandle()
    {
        anim.SetFloat("isCandleActive", 1);
    }

    public void DeActivatedCandle()
    {
        anim.SetFloat("isCandleActive", 0);
    }
}
