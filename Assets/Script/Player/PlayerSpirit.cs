using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpirit : Character
{
    private Animator anim;
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        typeChar = "Spirit";
        HandleAnimations();
        HandleLocationChanged();
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

    private void OnDrawGizmos()
    {
        DrawItemDetector();
    }
}
