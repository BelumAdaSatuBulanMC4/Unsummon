using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpirit : Character
{
    private Animator anim;
    private Collider2D spiritCollider;

    [Header("Collision Enabler")]
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckRadius;
    [SerializeField] private LayerMask whatIsPlayerKid;
    [SerializeField] private LayerMask whatIsPlayerPocong;

    private Collider2D[] detectedKids;
    private Collider2D[] detectedPocong;

    private bool isKidDetected = false;
    private bool isPocongDetected = false;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
        spiritCollider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();
        typeChar = "Spirit";
        HandleAnimations();
        HandleLocationChanged();
        HandlePlayerCollision();
    }

    private void HandlePlayerCollision()
    {
        detectedKids = Physics2D.OverlapCircleAll(playerCheck.position, playerCheckRadius, whatIsPlayerKid);
        detectedPocong = Physics2D.OverlapCircleAll(playerCheck.position, playerCheckRadius, whatIsPlayerPocong);

        if (detectedKids.Length > 0)
            foreach (Collider2D kidCollider in detectedKids)
            {
                PlayerKid kid = kidCollider.GetComponent<PlayerKid>();
                if (kid != null)
                {
                    Physics2D.IgnoreCollision(spiritCollider, kidCollider);
                }
            }

        if (detectedPocong.Length > 0)
            foreach (Collider2D pocongCollider in detectedPocong)
            {
                Pocong pocong = pocongCollider.GetComponent<Pocong>();  // Assuming you have a PlayerPocong script
                if (pocong != null)
                {
                    Physics2D.IgnoreCollision(spiritCollider, pocongCollider);
                }
            }
    }

    private void HandleLocationChanged()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            PlayerManager.instance.UpdateSpiritPosition(this, transform.position);
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

    // private void OnDrawGizmos()
    // {
    //     DrawItemDetector();
    // }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     // Disable collision with Pocong or Kid
    //     if (collision.gameObject.CompareTag("Pocong") || collision.gameObject.CompareTag("Kid"))
    //     {
    //         Physics2D.IgnoreCollision(spiritCollider, collision.collider);
    //     }
    // }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    }
}
