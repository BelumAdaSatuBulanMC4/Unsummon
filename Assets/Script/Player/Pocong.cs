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


    protected override void Awake()
    {
        base.Awake();
        typeChar = "Pocong";
        anim = GetComponentInChildren<Animator>();
    }

    protected override void Update()
    {
        base.Update();
        HandleAnimations();
        HandleTeleport();
        HandleKidInteraction();
        GetKidsPosition();
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
