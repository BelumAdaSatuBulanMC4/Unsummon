using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Item : NetworkBehaviour
{
    public bool isActivated = false;
    public bool isCursed = false;
    private Animator anim;

    public AudioClip onCandle;
    public AudioClip offCandle;
    private AudioSource audioSource;
    private Light2D light;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        light = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        if (isActivated)
        {
            anim.SetFloat("isCandleActive", 1);
        }
        // else
        // {
        //     anim.SetFloat("isCandleActive", 0);
        // }
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

        //check the logic
        if (isActivated && !isCursed)
        {
            anim.SetFloat("isCandleActive", 1);
        }
        else if (!isActivated && !isCursed)
        {
            anim.SetFloat("isCandleActive", 0);
        }
        else if (!isActivated && isCursed)
        {
            // anim.SetFloat("isCandleActive", 2);
        }
    }

    public void ItemActivated()
    {
        if (!isCursed && !isActivated)
        {
            audioSource.PlayOneShot(onCandle);
            ItemActivatedServerRpc();
        }
    }
    public void ItemDeactivated()
    {
        if (isActivated)
        {
            audioSource.PlayOneShot(offCandle);
            ItemDeactivatedServerRpc();
            CurseActivated();
            Debug.Log("Cursing successfully " + isCursed);
        }
    }

    public void CurseActivated()
    {
        CurseActivatedServerRpc();
    }

    public void CurseDectivated()
    {
        CurseDectivatedServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ItemActivatedServerRpc()
    {
        ItemActivatedClientRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ItemDeactivatedServerRpc()
    {
        ItemDeactivatedClientRpc();
    }

    //turn the curse off
    [ServerRpc(RequireOwnership = false)]
    public void CurseDectivatedServerRpc()
    {
        // ItemDeactivatedClientRpc();
        CurseDectivatedClientRpc();
    }
    //turn the curse on
    [ServerRpc(RequireOwnership = false)]
    public void CurseActivatedServerRpc()
    {
        // ItemDeactivatedClientRpc();
        CurseActivatedClientRpc();
    }

    [ClientRpc(RequireOwnership = false)]
    public void ItemActivatedClientRpc()
    {
        isActivated = true;
        anim.SetFloat("isCandleActive", 1);
        // GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc(RequireOwnership = false)]
    public void ItemDeactivatedClientRpc()
    {
        isActivated = false;
        anim.SetFloat("isCandleActive", 0);
        // GetComponent<SpriteRenderer>().color = Color.white;
    }

    [ClientRpc(RequireOwnership = false)]
    public void CurseActivatedClientRpc()
    {
        isCursed = true;
        // anim.SetFloat("isCandleActive", 1);
        // GetComponent<SpriteRenderer>().color = Color.green;
    }

    [ClientRpc(RequireOwnership = false)]
    public void CurseDectivatedClientRpc()
    {
        isCursed = false;
        // anim.SetFloat("isCandleActive", 0);
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
