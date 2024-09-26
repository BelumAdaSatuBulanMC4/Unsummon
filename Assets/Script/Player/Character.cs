using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected String typeChar;

    [Header("Movement")]
    [SerializeField] protected float moveSpeed;

    protected float xInput;
    protected float yInput;
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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        if (typeChar == "Player" || typeChar == "Pocong")
        {
            dashTime -= Time.deltaTime;
            dashCooldownTimer -= Time.deltaTime;
        }

        HandleInput();
        HandleMovement();
        HandleItemInteraction();
        HandleFlip();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (typeChar == "Player" || typeChar == "Pocong")
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                DashAbility();
            }
        }
    }

    private void DashAbility()
    {
        if (dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
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
        if (item == null) return; // Check if item is null to prevent NullReferenceException

        if (Input.GetKeyDown(KeyCode.E) && typeChar == "Player")
        {
            // item.DisplayInteraction();

            // if (GameManager.instance == null)
            // {
            //     Debug.LogError("GameManager instance is null! Make sure GameManager is in the scene.");
            //     return; // Prevent further execution if GameManager is null
            // }

            if (!item.isActivated)
                GameManager.instance.KidTurnedOnItem(item); // Notify GameManager when a Kid turns on an item
        }
        else if (Input.GetKeyDown(KeyCode.R) && typeChar == "Pocong")
        {
            // item.DisplayInteraction();

            // if (GameManager.instance == null)
            // {
            //     Debug.LogError("GameManager instance is null! Make sure GameManager is in the scene.");
            //     return; // Prevent further execution if GameManager is null
            // }
            if (item.isActivated)
                GameManager.instance.PocongTurnedOffItem(item); // Notify GameManager when Pocong turns off an item
        }
    }

    // void InteractWithItem(Item item)
    // {
    //     if (item == null) return; // Check if item is null to prevent the NullReferenceException

    //     if (Input.GetKeyDown(KeyCode.E) && typeChar == "Player")
    //     {
    //         item.DisplayInteraction();
    //         GameManager.instance.KidTurnedOnItem(item); // Notify GameManager when a Kid turns on an item
    //     }
    //     else if (Input.GetKeyDown(KeyCode.R) && typeChar == "Pocong")
    //     {
    //         item.DisplayInteraction();
    //         GameManager.instance.PocongTurnedOffItem(item); // Notify GameManager when Pocong turns off an item
    //     }
    // }
    // void InteractWithItem(Item item)
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         item.DisplayInteraction();
    //     }
    // }

    // private void InteractWithItem(Item item)
    // {
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         if (typeChar == "Player")
    //         {
    //             Debug.Log("masuk handle item");
    //             GameManager.instance.KidTurnedOnItem(item);
    //         }
    //         else if (typeChar == "Pocong")
    //         {
    //             GameManager.instance.PocongTurnedOffItem(item);
    //         }
    //     }
    // }

    protected virtual void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
    }

    private void HandleMovement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * moveSpeed * dashSpeed, yInput * moveSpeed * dashSpeed);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
        }
    }

    private void HandleFlip()
    {
        if (xInput < 0 && facingRight || xInput > 0 && !facingRight)
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
