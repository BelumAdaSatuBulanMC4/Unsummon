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
    [SerializeField] private float knockBackDuration = 1.5f;
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

    private Vector3 pocongPosition;
    public float maxDistance = 12f;
    public float minIntensity = 0f;
    public float maxIntensity = 1f;
    private bool isHiding = false;

    protected override void Awake()
    {
        base.Awake();
        typeChar = "Player";
        myCollider = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
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

    protected void HidingCharacter(bool hiding)
    {
        Debug.Log("Di dalem Hidecharacter SERVER anak" + HidingNow());

        // Debug.Log("sekarang dia hide HidingCharacter " + currentCloset.isUsed);
        // Debug.Log("sekarang dia UI() HidingCharacter " + hiding);
        // Debug.Log("sekarang dia Hiding() HidingCharacter " + UI_InGame.instance.GetIsHiding());

        if (!isHidingNow)
        {
            PlayerManager.instance.RegisterKid(this);
        }
        else
        {
            PlayerManager.instance.UnregisterKid(this);
        }
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
        hidingCoolDownTimer -= Time.deltaTime;
        HandleAnimations();
        HidingCharacter(UI_InGame.instance.GetIsHiding());
        // HandleLocationChanged();
        // HandlePlayerCollision();
        HandleMovement();
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            HandlePocongFear();
            Debug.Log("Update Character.cs: If iOS berhasil dipanggil");
        }

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

    private void HandlePocongFear()
    {
        pocongPosition = PlayerManager.instance.GetPocongPosition();
        float distance = Vector3.Distance(pocongPosition, transform.position);
        Debug.Log("Distance with pocong: " + distance);

        float intensity = Mathf.Lerp(maxIntensity, minIntensity, distance / maxDistance);
        intensity = Mathf.Clamp(intensity, minIntensity, maxIntensity);

        if (distance < maxDistance)
        {
            GameManager.instance.StartConHapticFeedback(intensity);
        }
        // else if (distance > 4 && distance < 8)
        // {
        //     GameManager.instance.StartConHapticFeedback(0.5f);

        // }
        // else if (distance < 4)
        // {
        //     GameManager.instance.StartConHapticFeedback(0.8f);
        // }
        else
        {
            GameManager.instance.StopConHapticFeedback();
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
        rb.constraints = RigidbodyConstraints2D.FreezePosition;

        sfxPocongKill.loop = false;
        sfxPocongKill.Play();
        // if (isAuthor)
        // {
        // CameraManager.instance.CameraShake();

        KidKilledServerRpc(OwnerClientId);
        // StartCoroutine(WaitForPlayersUpdate());
        GameManager.instance.UpdateKilledKids();
        // UI_InGame.instance.SetKilledScreen(true);

        // GamePlayManager.instance.UpdatePlayerCharacterListServerRpc(OwnerClientId, CharacterType.Spirit);
        // GameManager.instance.UpdateKilledKids();
        // GamePlayManager.instance.debugOutput.text += "\nGettingKilled -  berhasil dijalankan";

        // spirit.GetComponentInChildren<PlayerSpirit>().SetAuthor(isAuthor);
        // }
        GamePlayManager.instance.DebugTesting();

    }

    private IEnumerator WaitForPlayersUpdate()
    {
        yield return new WaitForSeconds(2.5f);
        KidKilledServerRpc(OwnerClientId);
    }

    public void Knocked(float sourceOfDamage)
    {
        GettingKilled();
        // float knockbackDir = 1;
        // // rb.velocity = new Vector2(0, 0);

        // if (transform.position.x < sourceOfDamage)
        // {
        //     knockbackDir = -1;
        // }

        // if (isKnocked)
        // {
        //     return;
        // }
        // StartCoroutine(KnockedRoutine());
        // rb.velocity = new Vector2(knockBackPower.x * knockbackDir, knockBackPower.y);
    }

    private IEnumerator KnockedRoutine()
    {
        isKnocked = true;
        isNowKilled = true;
        anim.SetBool("isHit", isKnocked);

        yield return new WaitForSeconds(knockBackDuration);

        isKnocked = false;
        isNowKilled = false;
        anim.SetBool("isHit", isKnocked);
        GettingKilled();
    }

    private void HandleAnimations()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            RequestPlayAnimationServerRpc("moving");
            // anim.SetFloat("moving", 1);
        }
        else
        {
            RequestStopAnimationServerRpc("moving");
            // anim.SetFloat("moving", 0);
        }
    }

    private void OnDrawGizmos()
    {
        DrawItemDetector();
        DrawClosetDetector();
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
                // UI_InGame.instance.SetKilledScreen(true);
                // StartCoroutine(WaitAndSpawnDeadBody());

                // UI_InGame.instance.SwitchToSpirit();

                UI_InGame.instance.SetAuthorCharacter(null);

                // UI_InGame.instance.InstantiateUIForCharacter(spirit.GetComponentInChildren<Character>());

                PlayerSpirit newSpirit = spiritPrefab.GetComponent<PlayerSpirit>();
                spirit.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId);


                // UI_NoiseButton uiNoiseButton = FindObjectOfType<UI_NoiseButton>();  // Assuming you have only one UI_NoiseButton

                GamePlayManager.instance.UpdatePlayerCharacterListClientRpc(clientId, CharacterType.Spirit);
                // GamePlayManager.instance.debugOutput.text += "\nKidKilledServerRpc -  berhasil dijalankan";

                // if (uiNoiseButton != null)
                // {
                //     uiNoiseButton.AssignPlayerSpirit(newSpirit); // Call a method to assign the spirit
                // }
                // CameraManager.instance.ChangeCameraFollow(spirit.transform);
            }
        }
    }

    private IEnumerator WaitAndSpawnDeadBody()
    {
        UI_InGame.instance.SetKilledScreen(true);
        // Wait for 2 seconds
        yield return new WaitForSeconds(1f);

        UI_InGame.instance.SetKilledScreen(false);
    }

    // [ClientRpc]
    // private void KidKilledClientRpc()
    // {

    // }

    // Client mengirimkan permintaan animasi ke server
    [ServerRpc]
    private void RequestPlayAnimationServerRpc(string animationTrigger, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server: Playing animation '{animationTrigger}'");
        PlayAnimationClientRpc(animationTrigger);
    }

    // Server mengirimkan perintah animasi ke semua client
    [ClientRpc]
    private void PlayAnimationClientRpc(string animationTrigger)
    {
        Debug.Log($"Client: Playing animation '{animationTrigger}'");
        anim.SetFloat(animationTrigger, 1);
    }

    // Client mengirimkan permintaan animasi ke server
    [ServerRpc]
    private void RequestStopAnimationServerRpc(string animationTrigger, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server: Playing animation '{animationTrigger}'");
        StopAnimationClientRpc(animationTrigger);
    }

    // Server mengirimkan perintah animasi ke semua client
    [ClientRpc]
    private void StopAnimationClientRpc(string animationTrigger)
    {
        Debug.Log($"Client: Playing animation '{animationTrigger}'");
        anim.SetFloat(animationTrigger, 0);
    }

    public void ChangeLocation(Vector3 loc)
    {
        transform.position = loc;
        Debug.Log("Swapped to : " + transform.position);
        // currentlocation = loc.ToString();
    }

    [ClientRpc]
    private void SwapPositionsClientRpc(ulong kidNetworkId, Vector3 newPocongPosition, Vector3 newKidPosition, ClientRpcParams rpcParams = default)
    {
        // Update Pocong's position on all clients
        transform.position = newPocongPosition;

        // Find the PlayerKid object and update its position on all clients
        NetworkObject kidNetworkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[kidNetworkId];
        if (kidNetworkObject != null)
        {
            PlayerKid kid = kidNetworkObject.GetComponent<PlayerKid>();
            if (kid != null)
            {
                kid.ChangeLocation(newKidPosition);
            }
        }
    }

    // [ServerRpc]
    // private void SwapPositionsServerRpc(ulong kidNetworkId, Vector3 pocongPosition, Vector3 kidPosition)
    // {
    //     Debug.Log($"Swapping Pocong position {pocongPosition} with Kid {kidNetworkId} position {kidPosition}");

    //     // Get the PlayerKid's NetworkObject using the kid's Network ID
    //     NetworkObject kidNetworkObject = NetworkManager.Singleton.SpawnManager.SpawnedObjects[kidNetworkId];

    //     if (kidNetworkObject != null)
    //     {
    //         PlayerKid kid = kidNetworkObject.GetComponent<PlayerKid>();

    //         if (kid != null)
    //         {
    //             // Perform the position swap
    //             Vector3 tempPocongPosition = pocongPosition;
    //             transform.position = kidPosition;
    //             kid.ChangeLocation(tempPocongPosition);

    //             // Notify all clients to update their positions via ClientRpc
    //             Debug.Log($"Swapping on server successful. New Pocong position: {transform.position}, New Kid position: {kid.transform.position}");
    //             SwapPositionsClientRpc(kidNetworkId, transform.position, kid.transform.position);
    //         }
    //     }
    // }


    public void RegisterTheKid()
    {
        PlayerManager.instance.RegisterKid(this);
    }

    public void UnRegisterTheKid()
    {
        PlayerManager.instance.UnregisterKid(this);
    }

}
