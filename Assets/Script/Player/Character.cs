using System;
using System.Collections;
using System.Collections.Generic;
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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputPlayer = new InputActions();
        // isAuthor = IsOwner;
        // FindFirstObjectByType<UI_DashButton>().UpdatePlayersRef(this);
    }

    private void Start() {
        isAuthor = IsOwner;
    }

    private void OnEnable()
    {
        inputPlayer.Enable();
        // inputPlayer.Kid.Dash.performed += ctx => DashAbility();
        inputPlayer.Kid.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputPlayer.Kid.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        inputPlayer.Disable();
        // inputPlayer.Kid.Dash.performed -= ctx => DashAbility();
        inputPlayer.Kid.Move.performed -= ctx => moveInput = ctx.ReadValue<Vector2>();
        inputPlayer.Kid.Move.canceled -= ctx => moveInput = Vector2.zero;
    }

    public string GetCurrentLocation() => currentlocation;
    public void CurrentLocationChanged(string loc)
    {
        currentlocation = loc;
    }
    public void DashButton() => DashAbility();

    protected virtual void Update()
    {
        if (!IsOwner) { return; }
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;
        }

        // HandleInput();
        HandleMovement();
        HandleItemInteraction();
        HandleFlip();
    }

    public float GetDashCooldown()
    {
        return dashCooldownTimer;
    }
    private void HandleInput()
    {
        // xInput = Input.GetAxisRaw("Horizontal");
        // yInput = Input.GetAxisRaw("Vertical");

        // if (typeChar == "Player" || typeChar == "Pocong")
        // {
        //     if (Input.GetKeyDown(KeyCode.LeftShift))
        //     {
        //         DashAbility();
        //     }
        // }
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
                    InteractWithItem(item);
                }
            }
        }
    }

    void InteractWithItem(Item item)
    {
        if (item == null) return;
        if (Input.GetKeyDown(KeyCode.E) && typeChar == "Player")
        {
            Debug.Log("cek!");
            if (!item.isActivated)
                GameManager.instance.KidTurnedOnItem(item); // Notify GameManager when a Kid turns on an item
        }
        else if (Input.GetKeyDown(KeyCode.R) && typeChar == "Pocong")
        {
            if (item.isActivated)
                GameManager.instance.PocongTurnedOffItem(item); // Notify GameManager when Pocong turns off an item
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
}
