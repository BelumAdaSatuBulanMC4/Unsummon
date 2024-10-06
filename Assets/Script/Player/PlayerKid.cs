using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class PlayerKid : Character
{
    private Animator anim;
    private Collider2D myCollider;

    [Header("KnockBack")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockBackPower;
    private bool isKnocked;

    [Header("Prefabs")]
    [SerializeField] private GameObject spiritPrefab;   // Assign the Spirit prefab in the Inspector
    [SerializeField] private GameObject deadBodyPrefab;

    [Header("Collision Spirit")]
    [SerializeField] private Transform spiritCheck;
    [SerializeField] private float spiritCheckRadius;
    [SerializeField] private LayerMask whatIsPlayerSpirit;
    private Collider2D[] detectedSpirits;

    [SerializeField] private GameObject controller_UI;

    [SerializeField] private GameObject buttonInteraction;

    protected override void Awake()
    {
        base.Awake();
        typeChar = "Player";
        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(RegisterKidWhenReady());
        myCollider = GetComponent<Collider2D>();
        isAuthor = IsOwner;
    }

    private IEnumerator RegisterKidWhenReady()
    {
        while (GameManager.instance == null)
        {
            yield return null; // Wait until PlayerManager is initialized
        }

        PlayerManager.instance.RegisterKid(this);
        Debug.Log("Player manager berhasil diintansiasi");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PlayerManager.instance.UnregisterKid(this); // Unregister when destroyed
    }

    protected override void Update()
    {
        if (!IsOwner) { return; }
        base.Update();
        HandleAnimations();
        // HandleLocationChanged();
        HandlePlayerCollision();
        HandleMovement();
        // PlayerManager.instance.UpdateKidPositionServerRpc(NetworkObjectId, transform.position);

        // HandleButtonInteraction();
        // controller_UI.SetActive(IsOwner);
        // Debug.Log("location of kid " + transform.position);
    }

    // private void HandleLocationChanged()
    // {
    //     if (Input.GetKeyDown(KeyCode.L))
    //     {
    //         PlayerManager.instance.UpdateKidPosition(this, transform.position);
    //     }
    // }

    private void HandlePlayerCollision()
    {
        detectedSpirits = Physics2D.OverlapCircleAll(spiritCheck.position, spiritCheckRadius, whatIsPlayerSpirit);

        if (detectedSpirits.Length > 0)
        {
            foreach (Collider2D spirit in detectedSpirits)
            {
                PlayerSpirit sprt = spirit.GetComponent<PlayerSpirit>();
                if (sprt != null)
                {
                    Physics2D.IgnoreCollision(myCollider, spirit);
                }
            }
        }
    }

    protected override void HandleMovement()
    {
        base.HandleMovement();

        // Check if the character is dashing
        if (dashTime > 0)
        {
            // If the character is dashing, send its position to the PlayerManager
            if (PlayerManager.instance != null)
            {
                Debug.Log("Di sini update position!");
                // Update position on the server (if this is the host or owner)
                PlayerManager.instance.UpdateKidPositionServerRpc(NetworkObjectId, transform.position);
            }
        }
        else
        {
            // If the character is no longer dashing, remove its position from the PlayerManager
            if (PlayerManager.instance != null)
            {
                Debug.Log("Di sini remove position!");

                PlayerManager.instance.RemoveKidPositionServerRpc(NetworkObjectId);
            }
        }
    }

    // private void HandleButtonInteraction()
    // {
    //     // Debug.Log("handle button : " + currentItem != null);
    //     if (currentItem != null && !currentItem.isActivated)
    //     {
    //         // Debug.Log("ITEM DETECTED");
    //         buttonInteraction.SetActive(true);
    //     }
    //     else
    //     {
    //         // Debug.Log("ITEM NOT DETECTED");
    //         buttonInteraction.SetActive(false);
    //     }
    // }

    // public void InteractedWithItem()
    // {
    //     if (!currentItem.isActivated)
    //     {
    //         // GameManager.instance.KidTurnedOnItem(item);
    //         UI_InGame.instance.OpenMiniGame();
    //         UI_MiniGame.instance.CurrentItem(currentItem);
    //     }
    // }

    private void GettingKilled()
    {
        // Debug.Log("the player has been killed");
        // Transform kidTransform = transform;
        sfxPocongKill.loop = false;
        sfxPocongKill.Play();
        // if (isAuthor)
        // {
        // CameraManager.instance.CameraShake();
        GameManager.instance.UpdateKilledKids();

        KidKilledServerRpc(OwnerClientId);
        // spirit.GetComponentInChildren<PlayerSpirit>().SetAuthor(isAuthor);
        // }

    }

    public void Knocked(float sourceOfDamage)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceOfDamage)
        {
            knockbackDir = -1;
        }

        if (isKnocked)
        {
            return;
        }
        StartCoroutine(KnockedRoutine());
        rb.velocity = new Vector2(knockBackPower.x * knockbackDir, knockBackPower.y);
    }

    private IEnumerator KnockedRoutine()
    {
        isKnocked = true;
        anim.SetBool("isHit", isKnocked);

        yield return new WaitForSeconds(knockBackDuration);

        isKnocked = false;
        anim.SetBool("isHit", isKnocked);
        GettingKilled();
    }

    private void HandleAnimations()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            anim.SetFloat("moving", 1);
        }
        else
        {
            anim.SetFloat("moving", 0);
        }
    }

    private void OnDrawGizmos()
    {
        DrawItemDetector();
        Gizmos.DrawWireSphere(spiritCheck.position, spiritCheckRadius);
    }

    // Client meminta Server untuk mendestroy object PlayerKid dan menspawn object PlayerSpirit
    [ServerRpc(RequireOwnership = false)]
    private void KidKilledServerRpc(ulong clientId)
    {
        // Cari NetworkObject berdasarkan Client ID
        if (NetworkManager.Singleton.ConnectedClients.ContainsKey(clientId))
        {
            NetworkObject mySelfNetworkObject = NetworkManager.Singleton.ConnectedClients[clientId].PlayerObject;
            if (mySelfNetworkObject != null)
            {
                mySelfNetworkObject.Despawn();
                Destroy(mySelfNetworkObject.gameObject);

                GameObject deadBody = Instantiate(deadBodyPrefab, transform.position, transform.rotation);
                deadBody.GetComponent<NetworkObject>().Spawn();

                GameObject spirit = Instantiate(spiritPrefab, transform.position, Quaternion.identity);
                PlayerSpirit newSpirit = spiritPrefab.GetComponent<PlayerSpirit>();
                spirit.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);
                // UI_NoiseButton uiNoiseButton = FindObjectOfType<UI_NoiseButton>();  // Assuming you have only one UI_NoiseButton

                // if (uiNoiseButton != null)
                // {
                //     uiNoiseButton.AssignPlayerSpirit(newSpirit); // Call a method to assign the spirit
                // }
                // CameraManager.instance.ChangeCameraFollow(spirit.transform);
            }
        }
    }

    [ClientRpc]
    private void KidKilledClientRpc()
    {

    }
}
