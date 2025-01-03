using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public GameObject offScreenIndicatorPrefab;
    public GameObject currentCharacter; // The character that this script is attached to
    public Camera mainCamera;
    public float edgeBuffer = 0.1f;

    // Dictionary to store the indicator for each Kid (using NetworkId as the key)
    private Dictionary<ulong, GameObject> kidIndicators = new Dictionary<ulong, GameObject>();

    private SpriteRenderer sr;
    private float srWidth;
    private float srHeight;

    private bool isFirstRender = true;

    private Character authorCharacter;

    void Start()
    {
        sr = offScreenIndicatorPrefab.GetComponentInChildren<SpriteRenderer>();

        var bounds = sr.bounds;
        srHeight = bounds.size.y / 2f;
        srWidth = bounds.size.x / 2f;

        // Debug.Log("PlayerManager.instance: " + GameManager.instance);
        // Debug.Log("offScreenIndicatorPrefab: " + offScreenIndicatorPrefab);

        // if (GameManager.instance == null)
        // {
        //     Debug.LogError("PlayerManager instance is null!");
        //     return;
        // }

        // if (offScreenIndicatorPrefab == null)
        // {
        //     Debug.LogError("offScreenIndicatorPrefab is null!");
        //     return;
        // }

        // InitializeIndicators();

        // Initialize indicators based on the initial state of kidPositions
        InitializeIndicators();
        authorCharacter = FindAuthorCharacter();
    }

    void Update()
    {
        // if (isFirstRender)
        // {
        //     InitializeIndicators();
        //     isFirstRender = false;
        // }
        // InitializeIndicators();
        // Dynamically handle additions and removals in kidPositions
        SyncIndicatorsWithKidPositions();
        // Update existing indicators
        if (GameManager.instance.GetTypeCharacter() == "Pocong")
        {
            UpdateIndicators();
        }
    }

    // Method to initialize indicators based on kidPositions in PlayerManager
    void InitializeIndicators()
    {
        if (PlayerManager.instance != null)
        {
            Dictionary<ulong, Vector3> kidPositions = PlayerManager.instance.GetKidPositionsNET();

            if (kidPositions == null)
            {
                Debug.LogError("kidPositionsNET is null!");
                return;
            }

            foreach (var entry in kidPositions)
            {
                ulong networkId = entry.Key;

                // Instantiate an indicator for each kid
                if (!kidIndicators.ContainsKey(networkId))
                {
                    GameObject indicator = Instantiate(offScreenIndicatorPrefab);
                    indicator.SetActive(false); // Start with indicators hidden
                    kidIndicators.Add(networkId, indicator);
                }
            }
        }
    }

    private Character FindAuthorCharacter()
    {
        Character[] allCharacters = FindObjectsOfType<Character>();
        // Debug.Log("jumlah author " + allCharacters.Length);
        foreach (Character character in allCharacters)
        {
            if (character.GetIsAuthor())
            {
                return character;
            }
        }
        return null;
    }


    // Synchronize indicators with changes in kidPositions
    void SyncIndicatorsWithKidPositions()
    {
        if (PlayerManager.instance != null)
        {
            Dictionary<ulong, Vector3> kidPositions = PlayerManager.instance.GetKidPositionsNET();

            // Add indicators for new kids
            foreach (var entry in kidPositions)
            {
                ulong networkId = entry.Key;

                if (!kidIndicators.ContainsKey(networkId))
                {
                    // Create a new indicator for the new kid
                    GameObject indicator = Instantiate(offScreenIndicatorPrefab);
                    indicator.SetActive(false);
                    kidIndicators.Add(networkId, indicator);
                }
            }

            // Remove indicators for kids who have been removed
            List<ulong> idsToRemove = new List<ulong>();

            foreach (var networkId in kidIndicators.Keys)
            {
                if (!kidPositions.ContainsKey(networkId))
                {
                    // Kid has been removed from the game, so remove their indicator
                    Destroy(kidIndicators[networkId]);
                    idsToRemove.Add(networkId);
                }
            }

            // Remove the entries from the dictionary
            foreach (ulong id in idsToRemove)
            {
                kidIndicators.Remove(id);
            }
        }
    }

    // Update all indicators for the current kids
    void UpdateIndicators()
    {
        if (PlayerManager.instance != null)
        {
            Dictionary<ulong, Vector3> kidPositions = PlayerManager.instance.GetKidPositionsNET();

            foreach (KeyValuePair<ulong, Vector3> entry in kidPositions)
            {
                ulong networkId = entry.Key;
                Vector3 kidPosition = entry.Value;

                // Ensure the indicator for this NetworkId exists
                if (kidIndicators.ContainsKey(networkId))
                {
                    GameObject indicator = kidIndicators[networkId];
                    if (authorCharacter is PlayerKid || authorCharacter is PlayerSpirit)
                    {
                        indicator.SetActive(false);
                    }

                    // Update the indicator for this kid's position
                    UpdateOffScreenIndicator(kidPosition, indicator);
                }
            }
        }
    }

    // Method to update the position and visibility of an off-screen indicator
    // void UpdateOffScreenIndicator(Vector3 kidPosition, GameObject indicator)
    // {
    //     Vector3 screenPosition = mainCamera.WorldToViewportPoint(kidPosition);

    //     bool isOffScreen = screenPosition.x <= 0 || screenPosition.x >= 1 || screenPosition.y <= 0 || screenPosition.y >= 1;

    //     if (isOffScreen)
    //     {
    //         indicator.SetActive(true);

    //         // Adjust the position of the indicator within the viewport bounds
    //         var spriteSizeInViewPort = mainCamera.WorldToViewportPoint(new Vector3(srWidth, srHeight, 0)) - mainCamera.WorldToViewportPoint(Vector3.zero);
    //         screenPosition.x = Mathf.Clamp(screenPosition.x, edgeBuffer + spriteSizeInViewPort.x, 1 - (edgeBuffer + spriteSizeInViewPort.x));
    //         screenPosition.y = Mathf.Clamp(screenPosition.y, edgeBuffer + spriteSizeInViewPort.y, 1 - (edgeBuffer + spriteSizeInViewPort.y));

    //         Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPosition);
    //         worldPosition.z = 0; // Keep z as 0 to stay on the 2D plane
    //         indicator.transform.position = worldPosition;

    //         Vector3 direction = kidPosition - indicator.transform.position;
    //         float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    //         indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    //     }
    //     else
    //     {
    //         indicator.SetActive(false); // Hide indicator when the kid is on-screen
    //     }
    // }

    void UpdateOffScreenIndicator(Vector3 kidPosition, GameObject indicator)
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(kidPosition);

        // Center of the circular area in viewport coordinates (e.g., (0.5, 0.5) for screen center)
        Vector2 center = new Vector2(0.5f, 0.5f);

        // Radius of the circular area in viewport space
        float radius = 0.4f; // 40% of the screen width/height

        // Calculate the offset from the center
        Vector2 offset = new Vector2(screenPosition.x - center.x, screenPosition.y - center.y);

        // Check if the position is outside the circle
        if (offset.magnitude > radius)
        {
            indicator.SetActive(true);

            // Clamp the position to the circle's edge
            offset = offset.normalized * radius;

            // Adjust the screen position to stay within the circle
            screenPosition.x = center.x + offset.x;
            screenPosition.y = center.y + offset.y;

            // Convert the adjusted viewport position back to world position
            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPosition);
            worldPosition.z = 0; // Keep z at 0 to stay on the 2D plane
            indicator.transform.position = worldPosition;

            // Rotate the indicator to point toward the kid
            Vector3 direction = kidPosition - indicator.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            indicator.transform.rotation = Quaternion.Euler(0, 0, angle - 90); // Adjust angle as needed
        }
        else
        {
            indicator.SetActive(false); // Hide indicator if within the circular area
        }
    }


}
