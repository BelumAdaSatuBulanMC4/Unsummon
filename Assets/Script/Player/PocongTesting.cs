using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PocongTesting : NetworkBehaviour
{
    [Header("Kid Check")]
    [SerializeField] private Transform kidCheck;
    [SerializeField] private float kidCheckRadius;
    [SerializeField] private LayerMask whatIsKid;
    // [SerializeField] private TMP_Text text;
    private bool isKidDetected;
    private Collider2D[] detectedKids;

    protected AudioSource sfxPocongKill;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleKidInteraction();
        AttackAbility();
    }


    private void HandleKidInteraction()
    {
        detectedKids = Physics2D.OverlapCircleAll(kidCheck.position, kidCheckRadius, whatIsKid);
        isKidDetected = detectedKids.Length > 0;
        // text.text = isKidDetected ? "Kid Detected" : "No Kid Detected";
        // if (isKidDetected)
        // {
        //     PlayerKid kid = detectedKids[0].GetComponent<PlayerKid>();
        //     sfxPocongKill.loop = false;
        //     sfxPocongKill.Play();
        //     kid.Knocked(transform.position.x);

        //     // KillTheKid(kid);
        // }

    }

    private void AttackAbility()
    {
        foreach (Collider2D kidCollider in detectedKids)
        {
            PlayerKid kid = kidCollider.GetComponent<PlayerKid>(); // Assuming the kids have a script named 'PlayerKid'
            if (kid != null)
            {
                Debug.Log("before kill");
                KillTheKid(kid);
            }
        }
    }

    void KillTheKid(PlayerKid kid)
    {
        // Debug.Log("BERHASIL NGEKILLL");
        sfxPocongKill.loop = false;
        sfxPocongKill.Play();
        kid.Knocked(transform.position.x);
        // }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(kidCheck.position, kidCheckRadius);

    }
}
