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
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        typeChar = "Player";
        HandleAnimations();
        HandleLocationChanged();
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
