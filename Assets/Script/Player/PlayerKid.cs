using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKid : Character
{
    private Animator anim;
    private Collider2D myCollider;

    [Header("KnockBack")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockBackPower;
    private bool isKnocked;

    [Header("Prefabs")]
    [SerializeField] private GameObject spiritPrefab;   // Assign the Spirit prefab in the Inspector
    [SerializeField] private GameObject deadBodyPrefab;

    [Header("Collision Spirit")]
    [SerializeField] private Transform spiritCheck;
    [SerializeField] private float spiritCheckRadius;
    [SerializeField] private LayerMask whatIsPlayerSpirit;
    private Collider2D[] detectedSpirits;

    [SerializeField] private GameObject controller_UI;

    protected override void Awake()
    {
        base.Awake();
        typeChar = "Player";
        myCollider = GetComponent<Collider2D>();
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        StartCoroutine(RegisterKidWhenReady());
        myCollider = GetComponent<Collider2D>();
    }

    private IEnumerator RegisterKidWhenReady()
    {
        while (PlayerManager.instance == null)
        {
            yield return null; // Wait until PlayerManager is initialized
        }

        PlayerManager.instance.RegisterKid(this);
        Debug.Log("Player manager berhasil diintansiasi");
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        PlayerManager.instance.UnregisterKid(this); // Unregister when destroyed
    }

    protected override void Update()
    {
        if (!IsOwner) { return; }
        base.Update();
        HandleAnimations();
        HandleLocationChanged();
        HandlePlayerCollision();
        controller_UI.SetActive(true);
    }

    private void HandleLocationChanged()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerManager.instance.UpdateKidPosition(this, transform.position);

        }
    }

    private void HandlePlayerCollision()
    {
        detectedSpirits = Physics2D.OverlapCircleAll(spiritCheck.position, spiritCheckRadius, whatIsPlayerSpirit);

        if (detectedSpirits.Length > 0)
        {
            foreach (Collider2D spirit in detectedSpirits)
            {
                PlayerSpirit sprt = spirit.GetComponent<PlayerSpirit>();
                if (sprt != null)
                {
                    Physics2D.IgnoreCollision(myCollider, spirit);
                }
            }
        }
    }

    private void GettingKilled()
    {
        Debug.Log("the player has been killed");
        // Transform kidTransform = transform;
        Instantiate(deadBodyPrefab, transform.position, transform.rotation);
        // if (isAuthor)
        // {
        CameraManager.instance.CameraShake();
        GameObject spirit = Instantiate(spiritPrefab, transform.position, transform.rotation);
        spirit.GetComponentInChildren<PlayerSpirit>().SetAuthor(isAuthor);
        // CameraManager.instance.ChangeCameraFollow(spirit.transform);
        // }
        Destroy(gameObject);
    }

    public void Knocked(float sourceOfDamage)
    {
        float knockbackDir = 1;

        if (transform.position.x < sourceOfDamage)
        {
            knockbackDir = -1;
        }

        if (isKnocked)
        {
            return;
        }
        StartCoroutine(KnockedRoutine());
        rb.velocity = new Vector2(knockBackPower.x * knockbackDir, knockBackPower.y);
    }

    private IEnumerator KnockedRoutine()
    {
        isKnocked = true;
        anim.SetBool("isHit", isKnocked);

        yield return new WaitForSeconds(knockBackDuration);

        isKnocked = false;
        anim.SetBool("isHit", isKnocked);
        GettingKilled();
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
        Gizmos.DrawWireSphere(spiritCheck.position, spiritCheckRadius);
    }
}
