using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pocong : Character
{
    private Animator anim;

    [Header("Kid Check")]
    [SerializeField] private Transform kidCheck;
    [SerializeField] private float kidCheckRadius;
    [SerializeField] private LayerMask whatIsKid;
    private bool isKidDetected;
    private Collider2D[] detectedKids;

    [Header("Attack Info")]
    [SerializeField] protected float attackCooldown; // Attack cooldown duration
    protected float attackCooldownTimer;

    [Header("Teleport Info")]
    [SerializeField] protected float teleportCooldown; // Teleport cooldown duration
    protected float teleportCooldownTimer;


    protected override void Awake()
    {
        base.Awake();
        typeChar = "Pocong";
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        attackCooldownTimer -= Time.deltaTime;
        teleportCooldownTimer -= Time.deltaTime;

        HandleAnimations();
        // HandleTeleport();
        HandleKidInteraction();
        GetKidsPosition();
    }

    private void HandleKidInteraction()
    {
        detectedKids = Physics2D.OverlapCircleAll(kidCheck.position, kidCheckRadius, whatIsKid);
        // Debug.Log("Detected kids " + detectedKids.Length);
        // foreach (Collider2D kidCollider in detectedKids)
        // {
        //     PlayerKid kid = kidCollider.GetComponent<PlayerKid>(); // Assuming the kids have a script named 'PlayerKid'
        //     if (kid != null)
        //     {
        //         killTheKid(kid);
        //     }
        // }

    }

    void killTheKid(PlayerKid kid)
    {
        // if (Input.GetKeyDown(KeyCode.R))
        // {
        kid.Knocked(transform.position.x);
        // }
    }

    private void GetKidsPosition()
    {
        Dictionary<PlayerKid, Vector3> kidPositions = PlayerManager.instance.GetKidPositions();
    }

    public void AttackButton()
    {
        AttackAbility();
    }

    public void TeleportButton()
    {
        TeleportAbility();
    }

    public float GetAttackCooldown()
    {
        return attackCooldownTimer;
    }

    public float GetTeleportCooldown()
    {
        return teleportCooldownTimer;
    }

    private void AttackAbility()
    {
        if (attackCooldownTimer < 0)
        {
            Debug.Log("masuk attack ability");
            foreach (Collider2D kidCollider in detectedKids)
            {
                PlayerKid kid = kidCollider.GetComponent<PlayerKid>(); // Assuming the kids have a script named 'PlayerKid'
                if (kid != null)
                {
                    Debug.Log("before kill");
                    killTheKid(kid);
                }
            }
            // Perform attack
            // Debug.Log("Attacking...");

            // Reset the cooldown timer
            attackCooldownTimer = attackCooldown;
        }
        else
        {
            Debug.Log("Attack on cooldown. Time remaining: " + attackCooldownTimer);
        }
    }

    private void TeleportAbility()
    {
        if (teleportCooldownTimer < 0)
        {
            List<PlayerKid> allKids = PlayerManager.instance.GetAllKids();
            PlayerKid kidToSwap = ChooseKidToSwap(allKids);

            if (kidToSwap != null)
            {
                SwapPositions(kidToSwap);
            }
            else
            {
                Debug.Log("No Kid selected for swapping.");
            }
            // Perform teleport
            // Debug.Log("Teleporting...");

            // Reset the cooldown timer
            teleportCooldownTimer = teleportCooldown;
        }
        else
        {
            Debug.Log("Teleport on cooldown. Time remaining: " + teleportCooldownTimer);
        }
    }

    private void HandleTeleport()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            List<PlayerKid> allKids = PlayerManager.instance.GetAllKids();
            PlayerKid kidToSwap = ChooseKidToSwap(allKids);

            if (kidToSwap != null)
            {
                SwapPositions(kidToSwap);
            }
            else
            {
                Debug.Log("No Kid selected for swapping.");
            }
        }
    }

    private PlayerKid ChooseKidToSwap(List<PlayerKid> kids)
    {
        if (kids.Count > 0)
        {
            int randomIndex = Random.Range(0, kids.Count);
            return kids[randomIndex];
        }
        return null;
    }

    private void SwapPositions(PlayerKid kid)
    {
        Vector3 tempPocongPosition = transform.position;
        transform.position = kid.transform.position;
        kid.transform.position = tempPocongPosition;
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
