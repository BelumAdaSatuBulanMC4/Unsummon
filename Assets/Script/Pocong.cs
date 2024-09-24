using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocong : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Movement")]
    [SerializeField] private float moveSpeed;

    private float xInput;
    private float yInput;

    private bool facingRight = true;
    private int facingDir = 1;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    [Header("Item Check")]
    [SerializeField] private Transform itemCheck;
    [SerializeField] private float itemCheckRadius;
    [SerializeField] private LayerMask whatIsItem;
    private bool isItemDetected;
    private Collider2D[] detectedItems;

    [Header("Kid Check")]
    [SerializeField] private Transform kidCheck;
    [SerializeField] private float kidCheckRadius;
    [SerializeField] private LayerMask whatIsKid;
    private bool isKidDetected;
    private Collider2D[] detectedKids;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        HandleMovement();
        HandleInput();
        HandleFlip();
        HandleAnimations();
        HandleItemInteraction();
        HandleKidInteraction();
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
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
        // isItemDetected = Physics2D.OverlapCircle(itemCheck.position, itemCheckRadius, whatIsItem);
        // if (isItemDetected)
        // {
        //     Debug.Log("item ada!");
        // }
        detectedItems = Physics2D.OverlapCircleAll(itemCheck.position, itemCheckRadius, whatIsItem);

        // If any items are detected, interact with them
        foreach (Collider2D itemCollider in detectedItems)
        {
            Item item = itemCollider.GetComponent<Item>(); // Assuming the squares have a script named 'Item'
            if (item != null)
            {
                InteractWithItem(item);
            }
        }
    }

    private void HandleKidInteraction()
    {
        // isKidDetected = Physics2D.OverlapCircle(kidCheck.position, kidCheckRadius, whatIsKid);
        // if (isKidDetected)
        // {
        //     Debug.Log("Kid detected!");
        // }
        detectedKids = Physics2D.OverlapCircleAll(kidCheck.position, kidCheckRadius, whatIsKid);

        // If any kids are detected, perform specific actions
        foreach (Collider2D kidCollider in detectedKids)
        {
            PlayerKid kid = kidCollider.GetComponent<PlayerKid>(); // Assuming the kids have a script named 'PlayerKid'
            if (kid != null)
            {
                // PerformSpecificAction(kid);
                killTheKid(kid);
            }
        }

    }

    void InteractWithItem(Item item)
    {
        if (Input.GetKeyDown(KeyCode.E)) // Assuming "E" is the key to interact
        {
            Debug.Log("Interacting with: " + item.name);
            item.DisplayInteraction(); // Change the variable inside the item
        }
    }

    void killTheKid(PlayerKid kid)
    {
        if (Input.GetKeyDown(KeyCode.R)) // Assuming "E" is the key to interact
        {
            Debug.Log("Interacting with: " + kid.name);
            kid.GettingKilled(); // Change the variable inside the item
        }
    }

    private void HandleMovement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * dashSpeed, yInput * dashSpeed);
        }
        else
        {
            rb.velocity = new Vector2(xInput * moveSpeed, yInput * moveSpeed);
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(itemCheck.position, itemCheckRadius);
        Gizmos.DrawWireSphere(kidCheck.position, kidCheckRadius);
    }
}
