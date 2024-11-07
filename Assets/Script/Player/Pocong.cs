using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Pocong : Character
{
    private Animator anim;
    private Collider2D myCollider;

    [Header("Kid Check")]
    [SerializeField] private Transform kidCheck;
    [SerializeField] private float kidCheckRadius;
    [SerializeField] private LayerMask whatIsKid;
    private bool isKidDetected;
    private Collider2D[] detectedKids;

    [Header("Mirror Check")]
    [SerializeField] private Transform mirrorCheck;
    [SerializeField] private float mirrorCheckRadius;
    [SerializeField] private LayerMask whatIsMirror;
    private bool isMirrorDetected;
    private Collider2D[] detectedMirrors;

    [Header("Collision Spirit")]
    [SerializeField] private Transform spiritCheck;
    [SerializeField] private float spiritCheckRadius;
    [SerializeField] private LayerMask whatIsPlayerSpirit;
    private Collider2D[] detectedSpirits;

    [Header("Attack Info")]
    [SerializeField] protected float attackCooldown; // Attack cooldown duration
    protected float attackCooldownTimer;

    [Header("Teleport Info")]
    [SerializeField] protected float teleportCooldown; // Teleport cooldown duration
    protected float teleportCooldownTimer;
    private GameObject mirrorSelected;
    private Vector3 nextMirrorPosition;


    [SerializeField] private GameObject controller_UI;

    [SerializeField] private GameObject buttonInteraction;

    private bool isTeleported = false;
    // [SerializeField] private GameObject characterLight2D;


    protected override void Awake()
    {
        base.Awake();
        typeChar = "Pocong";
        anim = GetComponentInChildren<Animator>();
        myCollider = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        isAuthor = IsOwner;
        // if (IsOwner) characterLight2D.SetActive(true);
    }

    protected override void Update()
    {
        if (!IsOwner) { Debug.Log("KEnAPA???"); return; }
        Debug.Log("COBA POCONG");
        base.Update();

        attackCooldownTimer -= Time.deltaTime;
        teleportCooldownTimer -= Time.deltaTime;

        // Debug.Log("location of pocong " + transform.position);

        HandleAnimations();
        // HandleTeleport();
        HandlePlayerCollision();
        // HandleLocationChanged();
        HandleKidInteraction();
        HandleMirrorInteraction();
        GetKidsPosition();
        // controller_UI.SetActive(IsOwner);
        HandleButtonInteraction();
    }
    // private void HandleLocationChanged()
    // {
    //     if (dashTime > 0)
    //     {
    //         PlayerManager.instance.UpdatePocongPosition(this, transform.position);
    //     }
    // }

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

    private void HandleButtonInteraction()
    {
        // Debug.Log("POCONG BUTTON ");
        if (currentItem != null && currentItem.isActivated)
        {
            Debug.LogWarning("is item null? " + currentItem == null);
            buttonInteraction.SetActive(true);
        }
        else
        {
            Debug.LogWarning("is item null? " + currentItem == null);
            buttonInteraction.SetActive(false);
        }
    }

    private void HandleKidInteraction()
    {
        detectedKids = Physics2D.OverlapCircleAll(kidCheck.position, kidCheckRadius, whatIsKid);
        isKidDetected = detectedKids.Length > 0 ? true : false;

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

    private void HandleMirrorInteraction()
    {
        detectedMirrors = Physics2D.OverlapCircleAll(mirrorCheck.position, mirrorCheckRadius, whatIsMirror);

        foreach (Collider2D mirror in detectedMirrors)
        {
            mirrorSelected = mirror.gameObject;
            Debug.Log($"HandleMirrorInteraction - Ini mirror: {mirrorSelected}");
        }

        isMirrorDetected = Physics2D.OverlapCircle(mirrorCheck.position, mirrorCheckRadius, whatIsMirror);
        // if (isMirrorDetected) mirrorSelectedPosition = mirrorCheck.position;
        // else mirrorSelectedPosition = new Vector3(0, 0);
    }

    void killTheKid(PlayerKid kid)
    {
        Debug.Log("BERHASIL NGEKILLL");
        sfxPocongKill.loop = false;
        sfxPocongKill.Play();
        kid.Knocked(transform.position.x);
        // }
    }

    private IEnumerator SetPocongAnimKill()
    {
        anim.SetBool("isKilling", true);
        yield return new WaitForSeconds(0.7f);
        anim.SetBool("isKilling", false);
    }

    private void GetKidsPosition()
    {
        // Dictionary<PlayerKid, Vector3> kidPositions = PlayerManager.instance.GetKidPositions();
        // foreach (KeyValuePair<PlayerKid, Vector3> entry in kidPositions)
        // {
        //     Debug.Log("Kid: " + entry.Key.name + ", Position: " + entry.Value);
        // }
        if (PlayerManager.instance != null)
        {
            Dictionary<ulong, Vector3> kidPositions = PlayerManager.instance.GetKidPositionsNET();
            Debug.Log("isi dari kid positions: " + kidPositions.Count);

            foreach (var kidPosition in kidPositions)
            {
                ulong kidId = kidPosition.Key;
                Vector3 position = kidPosition.Value;

                Debug.Log($"Kid {kidId} is at position {position}");
            }
        }
    }

    public void AttackButton()
    {
        RequestPlayAnimationKillServerRpc();
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

    public bool GetIsTeleported()
    {
        return isTeleported;
    }

    public bool GetIsKidDetected()
    {
        return isKidDetected;
    }

    public bool GetIsMirrorDetected()
    {
        return isMirrorDetected;
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

    public void TeleportAnimation(PlayerKid kid)
    {
        StartCoroutine(TeleportRoutine(kid));
    }

    public void TeleportAnimationMirror(Vector3 mirror)
    {
        StartCoroutine(TeleportRoutineMirror(mirror));
    }

    private IEnumerator TeleportRoutineMirror(Vector3 mirror)
    {
        anim.SetBool("isTeleport", true);
        yield return new WaitForSeconds(.4f);

        // Vector3 tempPocongPosition = mirror;
        transform.position = mirror;
        // kid.ChangeLocation(tempPocongPosition);
        // kid.transform.position = tempPocongPosition;

        // Once the blink animation is done, set isBlink to false
        anim.SetBool("isTeleport", false);
        // yield return new WaitForSeconds(5f);
        isTeleported = false;
    }

    private IEnumerator TeleportRoutine(PlayerKid kid)
    {
        anim.SetBool("isTeleport", true);
        yield return new WaitForSeconds(.4f);

        Vector3 tempPocongPosition = transform.position;
        transform.position = kid.transform.position;
        kid.ChangeLocation(tempPocongPosition);
        kid.transform.position = tempPocongPosition;

        // Once the blink animation is done, set isBlink to false
        anim.SetBool("isTeleport", false);
        // yield return new WaitForSeconds(5f);
        isTeleported = false;
    }


    private void TeleportAbility()
    {
        isTeleported = true;
        if (teleportCooldownTimer < 0 && isMirrorDetected)
        {

            GameObject[] allMirrorPosition = GameManager.instance.GetAllMirrors();
            Debug.Log($"TeleportAblity - mirrorSelectedPostition: {mirrorSelected}");
            for (int i = 0; i < allMirrorPosition.Length; i++)
            {
                Debug.Log($"TeleportAblity - for di mirror: {allMirrorPosition[i]}");
                if (allMirrorPosition[i] == mirrorSelected)
                {
                    Debug.Log("TeleportAbility - Berhasil mendapatkan mirror yang sama dengan yang diselect");
                    Debug.Log($"TeleportAbility - i:{i} dan allMirrorPosition.Length: {allMirrorPosition.Length - 1}");
                    if (i == (allMirrorPosition.Length - 1)) nextMirrorPosition = allMirrorPosition[0].transform.position;
                    else nextMirrorPosition = allMirrorPosition[i + 1].transform.position;
                    break;
                }
            }
            // int randomIndex = Random.Range(0, allMirrorPosition.Length);
            // TeleportAnimationMirror(allMirrorPosition[randomIndex]);

            Debug.Log($"TeleportAblity - nextMirror: {nextMirrorPosition}");
            if (nextMirrorPosition != null) TeleportAnimationMirror(nextMirrorPosition);
            // if (nextMirror != null) TeleportAnimationMirror(allMirrorPosition[0]);

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
        TeleportAnimation(kid);
        // Vector3 tempPocongPosition = transform.position;
        // transform.position = kid.transform.position;
        // kid.transform.position = tempPocongPosition;
    }

    private void HandleAnimations()
    {
        if (rb.velocity.x != 0 || rb.velocity.y != 0)
        {
            RequestPlayAnimationServerRpc("moving");
            // anim.SetFloat("moving", 1);
        }
        else
        {
            RequestStopAnimationServerRpc("moving");
            // anim.SetFloat("moving", 0);
        }
    }

    private void OnDrawGizmos()
    {
        DrawItemDetector();
        Gizmos.DrawWireSphere(kidCheck.position, kidCheckRadius);
        Gizmos.DrawWireSphere(mirrorCheck.position, mirrorCheckRadius);
        Gizmos.DrawWireSphere(spiritCheck.position, spiritCheckRadius);

    }

    [ServerRpc]
    private void RequestPlayAnimationServerRpc(string animationTrigger, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server: Playing animation '{animationTrigger}'");
        PlayAnimationClientRpc(animationTrigger);
    }

    [ClientRpc]
    private void PlayAnimationClientRpc(string animationTrigger)
    {
        Debug.Log($"Client: Playing animation '{animationTrigger}'");
        anim.SetFloat(animationTrigger, 1);
    }

    [ServerRpc]
    private void RequestStopAnimationServerRpc(string animationTrigger, ServerRpcParams rpcParams = default)
    {
        Debug.Log($"Server: Playing animation '{animationTrigger}'");
        StopAnimationClientRpc(animationTrigger);
    }

    [ClientRpc]
    private void StopAnimationClientRpc(string animationTrigger)
    {
        Debug.Log($"Client: Playing animation '{animationTrigger}'");
        anim.SetFloat(animationTrigger, 0);
    }

    [ServerRpc]
    private void RequestPlayAnimationKillServerRpc()
    {
        PlayAnimationKillClientRpc();
    }

    [ClientRpc]
    private void PlayAnimationKillClientRpc()
    {
        StartCoroutine(SetPocongAnimKill());
    }

}
