using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocong : Character
{
    private Animator anim;

    [Header("Hopping info")]
    [SerializeField] private float leapSpeed;
    [SerializeField] private float leapDuration;
    private float leapTime;
    [SerializeField] private float leapCooldown;
    private float leapCooldownTimer;

    [Header("Kid Check")]
    [SerializeField] private Transform kidCheck;
    [SerializeField] private float kidCheckRadius;
    [SerializeField] private LayerMask whatIsKid;
    private bool isKidDetected;
    private Collider2D[] detectedKids;


    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        leapTime -= Time.deltaTime;
        leapCooldownTimer -= Time.deltaTime;

        base.Update();
        HandleSpesificInput();
        HandleMovement();
        HandleAnimations();
        HandleKidInteraction();
        GetKidsPosition();
    }

    private void HandleSpesificInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            DashAbility();
        }
    }

    private void DashAbility()
    {
        if (leapCooldownTimer < 0)
        {
            leapCooldownTimer = leapCooldown;
            leapTime = leapDuration;
        }
    }

    private void HandleKidInteraction()
    {
        detectedKids = Physics2D.OverlapCircleAll(kidCheck.position, kidCheckRadius, whatIsKid);

        foreach (Collider2D kidCollider in detectedKids)
        {
            PlayerKid kid = kidCollider.GetComponent<PlayerKid>(); // Assuming the kids have a script named 'PlayerKid'
            if (kid != null)
            {
                killTheKid(kid);
            }
        }

    }

    void killTheKid(PlayerKid kid)
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            kid.Knocked(transform.position.x);
        }
    }

    private void GetKidsPosition()
    {
        Dictionary<PlayerKid, Vector3> kidPositions = PlayerManager.instance.GetKidPositions();

        foreach (var kid in kidPositions)
        {
            Debug.Log($"Pocong knows Kid {kid.Key.gameObject.name} is at position {kid.Value}");
        }
    }

    private void HandleMovement()
    {
        if (leapTime > 0)
        {
            rb.velocity = new Vector2(xInput * leapSpeed, yInput * leapSpeed);
        }
        else
        {
            Movement();
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

    private void OnDrawGizmos()
    {
        DrawItemDetector();
        Gizmos.DrawWireSphere(kidCheck.position, kidCheckRadius);
    }
}
