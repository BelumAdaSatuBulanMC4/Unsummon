using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class Character : NetworkBehaviour
{
    protected Rigidbody2D rb;
    protected String typeChar;

    [Header("Author")]
    [SerializeField] public bool isAuthor;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed;

    // protected float xInput;
    // protected float yInput;

    protected InputActions inputPlayer;
    protected Vector2 moveInput;

    protected bool facingRight = true;
    protected int facingDir = 1;

    [Header("Item Check")]
    [SerializeField] protected Transform itemCheck;
    [SerializeField] protected float itemCheckRadius;
    [SerializeField] protected LayerMask whatIsItem;
    protected Collider2D[] detectedItems;
    protected Item currentItem;

    [Header("Dash info")]
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashDuration;
    protected float dashTime;
    [SerializeField] protected float dashCooldown;
    protected float dashCooldownTimer;
    protected string currentlocation;

    [SerializeField] protected GameObject mySelf;

    protected AudioSource sfxMovement;
    protected AudioSource sfxPocongKill;

    public AudioClip sfxMovementClip;  // AudioClip untuk AudioSource pertama
    public AudioClip sfxPocongKillClip;  // AudioClip untuk AudioSource kedua

    // BAGIAN CLOSET
    [Header("Closet Check")]
    [SerializeField] protected Transform closetCheck;
    [SerializeField] protected float closetCheckRadius;
    [SerializeField] protected LayerMask whatIsCloset;
    protected Collider2D[] detectedClosets;
    protected Closet currentCloset;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputPlayer = new InputActions();
        AudioSource[] audioSources = GetComponents<AudioSource>();
        sfxMovement = audioSources[0];
        sfxPocongKill = audioSources[1];
        sfxMovement.clip = sfxMovementClip;
        sfxPocongKill.clip = sfxPocongKillClip;
    }

    private void Start()
    {
        UserManager.instance.SetYourRole(typeChar);
    }

    public void MakeANoise()
    {
        Debug.Log("Berhasil Membuat Suara! dengan posisi = " + transform.position.x + "dan " + transform.position.x);
    }

    private void OnEnable()
    {
        inputPlayer.Enable();
        inputPlayer.Kid.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); };
        inputPlayer.Kid.Move.canceled += ctx => moveInput = Vector2.zero;
    }
    protected virtual void Update()
    {
        if (!IsOwner) { return; }
        Debug.Log($"PlayerID: {OwnerClientId} adalah {typeChar}");
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;
        }
        if (typeChar != "Player")
        {
            HandleMovement();
        }
        HandleItemInteraction();
        // HandleClosetInteraction();
        HandleFlip();
        SendPositionToServerServerRpc();
        HandleMovementSound();
    }

    private void OnDisable()
    {
        inputPlayer.Disable();
    }

    public string GetCurrentLocation() => currentlocation;
    public void CurrentLocationChanged(string loc)
    {
        currentlocation = loc;
    }

    public string GetTypeChar()
    {
        return typeChar;
    }
    public void DashButton() => DashAbility();

    public float GetDashCooldown()
    {
        return dashCooldownTimer;
    }

    public bool GetIsAuthor()
    {
        return IsOwner;
    }

    private void DashAbility()
    {
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            if (dashCooldownTimer < 0)
            {
                dashCooldownTimer = dashCooldown;
                dashTime = dashDuration;
            }
        }
    }

    private void HandleItemInteraction()
    {
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            detectedItems = Physics2D.OverlapCircleAll(itemCheck.position, itemCheckRadius, whatIsItem);
            detectedClosets = Physics2D.OverlapCircleAll(closetCheck.position, closetCheckRadius, whatIsCloset);

            // if (detectedClosets != null)
            // {
            //     Debug.Log("Closet is not null");
            // }
            // else
            // {
            //     Debug.Log("Closet is null");
            // }

            if (detectedItems.Length == 0)
            {
                currentItem = null;
            }
            else
            {
                foreach (Collider2D itemCollider in detectedItems)
                {
                    Item item = itemCollider.GetComponent<Item>();
                    if (item != null)
                    {
                        // InteractWithItem(item);
                        currentItem = item;
                        // Debug.Log("item ada " + currentItem.isActivated);
                    }
                }
            }

            // Debug.Log("eaea");
            if (typeChar == "Player")
            {
                // Debug.Log("ini di mau masuk!");
                if (detectedClosets.Length == 0)
                {
                    // Debug.Log("lho kosong!");
                    currentCloset = null;
                }
                else
                {
                    // Debug.Log("Closet detected!");
                    foreach (Collider2D detectedClosets in detectedClosets)
                    {
                        Closet closet = detectedClosets.GetComponent<Closet>();
                        if (closet != null)
                        {
                            // InteractWithItem(item);
                            currentCloset = closet;
                            // Debug.Log("item ada " + currentItem.isActivated);
                        }
                    }
                }
            }
            // Debug.Log("Uhuk");
        }
    }

    public Item GetCurrentItem()
    {
        return currentItem;
    }

    // closet interaction
    private void HandleClosetInteraction()
    {
        Debug.Log("masuk ke HandleClosetInteraction");
        if (typeChar == "Player")
        {
            Debug.Log("typechar " + typeChar);
            detectedClosets = Physics2D.OverlapCircleAll(closetCheck.position, closetCheckRadius, whatIsCloset);
            foreach (Collider2D closetCollider in detectedClosets)
            {
                Debug.Log("masuk ke foreach ");

                Closet closet = closetCollider.GetComponent<Closet>();
                if (closet != null)
                {
                    currentCloset = closet;
                    Debug.Log("Closet detected ");
                }
            }
            if (detectedClosets.Length == 0)
            {
                currentCloset = null;
                Debug.Log("Closet not detected ");
            }
        }
        Debug.Log("Di bagian luar!");
    }

    public Closet GetCurrentCloset()
    {
        return currentCloset;
    }

    public void interactItemButton()
    {
        if (typeChar == "Player")
        {
            // Debug.Log("cek!");
            if (!currentItem.isActivated)
            {
                // GameManager.instance.KidTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(currentItem);
            }
        }
        else if (typeChar == "Pocong")
        {
            if (currentItem.isActivated)
            {
                // GameManager.instance.PocongTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(currentItem);
            }
            // GameManager.instance.PocongTurnedOffItem(item);
        }
    }

    protected virtual void Movement()
    {
        rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
    }

    protected virtual void HandleMovement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed * dashSpeed, moveInput.y * moveSpeed * dashSpeed);
        }
        else
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed, moveInput.y * moveSpeed);
        }
    }
    private void HandleFlip()
    {
        if (moveInput.x < 0 && facingRight || moveInput.x > 0 && !facingRight)
            Flip();
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
    }


    protected void DrawItemDetector()
    {
        Gizmos.DrawWireSphere(itemCheck.position, itemCheckRadius);
    }

    protected void DrawClosetDetector()
    {
        Gizmos.DrawWireSphere(closetCheck.position, closetCheckRadius);
    }

    [ServerRpc]
    public void SendPositionToServerServerRpc()
    {
        // Server memperbarui posisi pemain di server dan mengirimkan ke semua client
        UpdatePositionClientRpc(transform.position);
    }

    [ClientRpc]
    void UpdatePositionClientRpc(Vector2 newPosition)
    {
        if (!IsOwner)
        {
            // Jika bukan owner (client lain), update posisi berdasarkan data dari server
            transform.position = newPosition;
        }
    }

    private void HandleMovementSound()
    {
        if (moveInput != Vector2.zero)
        {
            if (!sfxMovement.isPlaying)
            {
                sfxMovement.Play();  // Mainkan sound effect saat bergerak
            }
        }
        else
        {
            if (sfxMovement.isPlaying)
            {
                sfxMovement.Stop();  // Hentikan sound effect saat berhenti
            }
        }
    }

}
