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

    [Header("Dash info")]
    [SerializeField] protected float dashSpeed;
    [SerializeField] protected float dashDuration;
    protected float dashTime;
    [SerializeField] protected float dashCooldown;
    protected float dashCooldownTimer;

    protected string currentlocation;

    protected Item currentItem;

    [SerializeField] protected GameObject mySelf;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputPlayer = new InputActions();
        // FindFirstObjectByType<UI_DashButton>().UpdatePlayersRef(this);
    }

    private void Start()
    {
        UserManager.instance.SetYourRole(typeChar);
    }

    private void OnEnable()
    {
        inputPlayer.Enable();
        // inputPlayer.Kid.Dash.performed += ctx => DashAbility();
        inputPlayer.Kid.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); };
        inputPlayer.Kid.Move.canceled += ctx => moveInput = Vector2.zero;
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

    protected virtual void Update()
    {
        if (!IsOwner) { return; }
        Debug.Log($"PlayerID: {OwnerClientId} adalah {typeChar}");
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;
        }

        // isAuthor = IsOwner;
        // Terapkan input langsung di client tanpa menunggu server
        HandleMovement();
        HandleItemInteraction();
        HandleFlip();
        SendPositionToServerServerRpc();
    }

    public Item GetCurrentItem()
    {
        return currentItem;
    }

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
            if (detectedItems.Length == 0)
            {
                currentItem = null;
            }
        }
    }

    void InteractWithItem(Item item)
    {
        if (item == null) return;
        if (Input.GetKeyDown(KeyCode.E) && typeChar == "Player")
        {
            // Debug.Log("cek!");
            if (!item.isActivated)
            {
                // GameManager.instance.KidTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(item);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R) && typeChar == "Pocong")
        {
            if (item.isActivated)
            {
                // GameManager.instance.PocongTurnedOnItem(item);
                UI_InGame.instance.OpenMiniGame();
                UI_MiniGame.instance.CurrentItem(item);
            }
            // GameManager.instance.PocongTurnedOffItem(item);
        }
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

    private void HandleMovement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(moveInput.x * moveSpeed * dashSpeed, moveInput.y * moveSpeed * dashSpeed);
        }
        else
        {
            // Debug.Log($"Movement: {moveInput} for {IsOwner}");
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

    [ServerRpc]
    void SendPositionToServerServerRpc()
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

}
