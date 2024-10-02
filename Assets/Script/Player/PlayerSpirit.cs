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

    [Header("Noise Info")]
    [SerializeField] protected float noiseCooldown; // Teleport cooldown duration
    protected float noiseCooldownTimer;

    private Collider2D[] detectedKids;
    private Collider2D[] detectedPocong;

    private bool isKidDetected = false;
    private bool isPocongDetected = false;

    [SerializeField] private GameObject controller_UI;
    protected override void Awake()
    {
        base.Awake();
        typeChar = "Spirit";
        anim = GetComponentInChildren<Animator>();
        spiritCollider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        base.Update();

        noiseCooldownTimer -= Time.deltaTime;

        HandleAnimations();
        // HandleLocationChanged();
        HandlePlayerCollision();

        if (isAuthor)
        {
            controller_UI.SetActive(true);
        }
        else
        {
            controller_UI.SetActive(false);
        }
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

    private void MakeNoise()
    {
        // if (Input.GetKeyDown(KeyCode.Y))
        // {
        if (noiseCooldownTimer < 0)
        {
            PlayerManager.instance.UpdateSpiritPosition(this, transform.position);
            noiseCooldownTimer = noiseCooldown;
        }
        // }
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

    public void NoiseButton()
    {
        MakeNoise();
    }

    public void SetAuthor(bool setBool)
    {
        isAuthor = setBool;
    }

    public float GetNoiseCooldown()
    {
        return noiseCooldownTimer;
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
