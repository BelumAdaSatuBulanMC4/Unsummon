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

    [Header("Prefabs")]
    [SerializeField] private GameObject spiritPrefab;   // Assign the Spirit prefab in the Inspector
    [SerializeField] private GameObject deadBodyPrefab;
    protected override void Awake()
    {
        base.Awake();
        typeChar = "Player";
        // PlayerManager.instance.RegisterKid(this);
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        PlayerManager.instance.RegisterKid(this); // Pindahkan ke Start
    }

    private void OnDestroy()
    {
        PlayerManager.instance.UnregisterKid(this); // Unregister when destroyed
    }

    protected override void Update()
    {
        base.Update();
        HandleAnimations();
        HandleLocationChanged();
    }

    private void HandleLocationChanged()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            CameraManager.instance.CameraShake();
            PlayerManager.instance.UpdateKidPosition(this, transform.position);
        }
    }

    private void GettingKilled()
    {
        Debug.Log("the player has been killed");
        Transform kidTransform = transform;
        Instantiate(deadBodyPrefab, kidTransform.position, kidTransform.rotation);
        GameObject spirit = Instantiate(spiritPrefab, kidTransform.position, kidTransform.rotation);
        if (isAuthor)
        {
            spirit.GetComponent<PlayerSpirit>().SetAuthor(true);
            CameraManager.instance.ChangeCameraFollow(spirit.transform);
        }
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
    }
}
