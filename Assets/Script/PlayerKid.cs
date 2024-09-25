using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKid : Character
{
    private Animator anim;

    [Header("KnockBack")]
    [SerializeField] private float knockBackDuration = 1;
    [SerializeField] private Vector2 knockBackPower;
    private bool isKnocked;

    [Header("Dash info")]
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashDuration;
    private float dashTime;
    [SerializeField] private float dashCooldown;
    private float dashCooldownTimer;

    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        dashTime -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;

        base.Update();
        HandleSpesificInput();
        HandleMovement();
        HandleAnimations();
        HandleLocationChanged();
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
        if (dashCooldownTimer < 0)
        {
            dashCooldownTimer = dashCooldown;
            dashTime = dashDuration;
        }
    }

    private void HandleLocationChanged()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            PlayerManager.instance.UpdateKidPosition(this, transform.position);
        }
    }

    public void GettingKilled()
    {
        Debug.Log("the player has been killed");
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

        //startroutine
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
    }

    private void HandleMovement()
    {
        if (dashTime > 0)
        {
            rb.velocity = new Vector2(xInput * moveSpeed * dashSpeed, yInput * moveSpeed * dashSpeed);
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
    }
}
