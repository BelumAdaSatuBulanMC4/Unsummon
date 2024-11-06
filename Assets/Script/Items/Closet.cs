using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Closet : MonoBehaviour
{
    public bool isUsed = false;
    private Animator anim;
    void Start()
    {

    }
    void Update()
    {
        if (isUsed)
        {
            // di sini bikin animasi dia ketutup
        }
        else
        {
            // di sini bikin animasi dia terbuka
        }
    }

    public void ClosetActivated()
    {
        if (!isUsed)
        {
            //panggil server rpc buat set dia ke true!
            UpdateUsedServerRpc(true);
        }
    }

    public void ClosetDeActivated()
    {
        if (isUsed)
        {
            UpdateUsedServerRpc(false);
            //panggil server rpc buat set dia ke false!
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateUsedServerRpc(bool state)
    {
        if (isUsed != state)
        {
            isUsed = state;
            UpdateUsedClientRpc(state);
        }
    }

    [ClientRpc]
    private void UpdateUsedClientRpc(bool state)
    {
        isUsed = state;
    }
}
