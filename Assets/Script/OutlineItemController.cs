using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OutlineItemController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private int objectsInsideTrigger = 0; // Counter to keep track of objects inside the trigger

    void Start()
    {
        // Get the SpriteRenderer component attached to this GameObject
        // spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Make sure the sprite is initially invisible
        // if (spriteRenderer != null)
        // {
        //     spriteRenderer.enabled = false;
        // }
    }

    // Called when another collider enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Increase the count of objects inside the trigger
        objectsInsideTrigger++;
        // anim.SetBool("isTriggered", true);

        // Enable the sprite if it's not already enabled
        if (spriteRenderer != null && !spriteRenderer.enabled)
        {
            // spriteRenderer.enabled = true;
            anim.SetBool("isTriggered", true); // Play the animation if it's not already playing
        }
    }

    // Called when another collider exits the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Decrease the count of objects inside the trigger
        objectsInsideTrigger--;
        // anim.SetBool("isTriggered", false);

        // If no objects are left inside the trigger, disable the sprite
        if (objectsInsideTrigger <= 0)
        {
            anim.SetBool("isTriggered", false); // Play the animation if it's not already playing
            // spriteRenderer.enabled = false;

            objectsInsideTrigger = 0; // Make sure it doesn't go below zero
        }
    }
}
