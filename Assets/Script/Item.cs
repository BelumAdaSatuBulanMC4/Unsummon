using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Item : NetworkBehaviour
{
    public bool isActivated = false;

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
        GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc(RequireOwnership = false)]
    public void ResetValueClientRpc()
    {
        isActivated = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
