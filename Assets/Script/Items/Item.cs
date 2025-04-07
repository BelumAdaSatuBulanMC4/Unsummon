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
        anim = GetComponent<Animator>();
        audioSource = AudioManager.Instance.GetSFXAudioSource(2);
        light = GetComponentInChildren<Light2D>();
    }

    private void Start()
    {
        if (isActivated) anim.SetFloat("isCandleActive", 0);
    }

    private void Update()
    {
        if (isCursed)
        {
            anim.SetFloat("isCandleActive", 2);
        }
        else
        {
            anim.SetFloat("isCandleActive", isActivated ? 1 : 0);
            light.enabled = isActivated;
        }
    }

    public void ItemActivated()
    {
        if (!isActivated)
        {
            audioSource.PlayOneShot(onCandle);
            SetActivatedStateServerRpc(true);
            UpdateCandleToMap();
            SetCursedStateServerRpc(false);
        }
    }
    public void ItemDeactivated()
    {
        if (isActivated)
        {
            audioSource.PlayOneShot(offCandle);
            SetActivatedStateServerRpc(false);
            UpdateCurseToMap();
            SetCursedStateServerRpc(true);
        }
    }

    public void RemoveCurse()
    {
        if (isCursed)
        {
            SetCursedStateServerRpc(false);
            SetActivatedStateServerRpc(true);
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

    public void ActivatedCandle()
    {
        anim.SetFloat("isCandleActive", 1);
    }

    public void DeActivatedCandle()
    {
        anim.SetFloat("isCandleActive", 0);
    }
    public void UpdateCandleToMap()
    {
        UI_InGame.instance.RegisterCandleToMap(transform.position);
    }

    public void UpdateCurseToMap()
    {
        UI_InGame.instance.RegisterCurseToMap(transform.position);
    }


    // ============================= SERVER RPC ===================================

    [ServerRpc(RequireOwnership = false)]
    private void SetActivatedStateServerRpc(bool state)
    {
        if (isActivated != state)
        {
            isActivated = state;
            UpdateActivatedStateClientRpc(state);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetCursedStateServerRpc(bool state)
    {
        if (isCursed != state)
        {
            isCursed = state;
            UpdateCursedStateClientRpc(state);
        }
    }

    [ClientRpc]
    private void UpdateActivatedStateClientRpc(bool state)
    {
        isActivated = state;
    }

    [ClientRpc]
    private void UpdateCursedStateClientRpc(bool state)
    {
        isCursed = state;
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

    [ServerRpc(RequireOwnership = false)]
    public void CurseDectivatedServerRpc()
    {
        CurseDectivatedClientRpc();
    }
    //turn the curse on
    [ServerRpc(RequireOwnership = false)]
    public void CurseActivatedServerRpc()
    {
        CurseActivatedClientRpc();
    }

    // ============================= CLIENT RPC ===================================

    [ClientRpc(RequireOwnership = false)]
    public void ItemActivatedClientRpc()
    {
        isActivated = true;
        anim.SetFloat("isCandleActive", 1);
    }

    [ClientRpc(RequireOwnership = false)]
    public void ItemDeactivatedClientRpc()
    {
        isActivated = false;
        anim.SetFloat("isCandleActive", 0);
    }

    [ClientRpc(RequireOwnership = false)]
    public void CurseActivatedClientRpc()
    {
        isCursed = true;
    }

    [ClientRpc(RequireOwnership = false)]
    public void CurseDectivatedClientRpc()
    {
        isCursed = false;
    }
}

