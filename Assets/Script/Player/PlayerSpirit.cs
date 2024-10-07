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
    [SerializeField] private float noiseCooldown = 7f; // Cooldown duration
    private float noiseCooldownTimer;

    [SerializeField] private float noiseTime = 5f; // Duration of noise
    private bool isMakingNoise = false;

    private Collider2D[] detectedKids;
    private Collider2D[] detectedPocong;

    private bool isKidDetected = false;
    private bool isPocongDetected = false;

    // private float noiseTime = 5f;

    [SerializeField] private GameObject controller_UI;
    protected override void Awake()
    {
        // if (!IsOwner) { return; };
        base.Awake();

        typeChar = "Spirit";
        anim = GetComponentInChildren<Animator>();
        spiritCollider = GetComponent<Collider2D>();
    }

    protected override void Update()
    {
        if (!IsOwner) { return; };
        base.Update();

        if (noiseCooldownTimer > 0)
        {
            noiseCooldownTimer -= Time.deltaTime;
        }

        // if (isMakingNoise)
        // {
        //     noiseCooldownTimer = noiseCooldown;
        // }

        HandleAnimations();
        // HandleLocationChanged();
        // HandlePlayerCollision();
        // controller_UI.SetActive(IsOwner);
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
        if (noiseCooldownTimer <= 0 && !isMakingNoise)
        {
            StartCoroutine(NoiseCoroutine());
        }
    }

    private IEnumerator NoiseCoroutine()
    {
        isMakingNoise = true;
        // Debug.Log("Making noise at position " + transform.position.x + " " + transform.position.y);
        if (PlayerManager.instance != null)
            PlayerManager.instance.UpdateKidPositionServerRpc(NetworkObjectId, transform.position);

        // Noise lasts for 'noiseTime' seconds
        yield return new WaitForSeconds(noiseTime);

        // Debug.Log("Noise ended");

        if (PlayerManager.instance != null)
            PlayerManager.instance.RemoveKidPositionServerRpc(NetworkObjectId);

        // Start cooldown after noise finishes
        // noiseCooldownTimer = noiseCooldown;
        isMakingNoise = false;
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

    public bool IsMakingNoise()
    {
        return isMakingNoise;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawWireSphere(playerCheck.position, playerCheckRadius);
    // }
}
