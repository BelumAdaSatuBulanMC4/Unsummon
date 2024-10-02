using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
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
}
