using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public GameObject offScreenIndicatorPrefab; // The indicator prefab
    public Camera mainCamera; // The camera to check positions relative to
    public float edgeBuffer = 0.1f; // Buffer to keep indicators inside screen edges (can be adjusted)

    private List<GameObject> players;
    private List<GameObject> activeIndicators = new List<GameObject>();

    private SpriteRenderer sr;
    private float srWidth;
    private float srHeight;

    void Start()
    {
        // Find all players in the scene and add them to the players list
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Kids"));
        sr = offScreenIndicatorPrefab.GetComponent<SpriteRenderer>();

        var bounds = sr.bounds;
        srHeight = bounds.size.y / 2f;
        srWidth = bounds.size.x / 2f;

        // Create off-screen indicators for each player
        foreach (GameObject player in players)
        {
            if (player != gameObject) // Ignore the local player
            {
                GameObject indicator = Instantiate(offScreenIndicatorPrefab);
                indicator.SetActive(false);
                activeIndicators.Add(indicator);
            }
        }
    }

    void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != gameObject && players[i] != null)
            {
                UpdateOffScreenIndicator(players[i], activeIndicators[i]);
                Debug.Log("location of player " + i + " " + players[i].transform.position);
            }
        }
    }

    void UpdateOffScreenIndicator(GameObject player, GameObject indicator)
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(player.transform.position);

        // Determine if the player is off-screen
        bool isOffScreen = screenPosition.x <= 0 || screenPosition.x >= 1 || screenPosition.y <= 0 || screenPosition.y >= 1;

        if (isOffScreen)
        {
            // Vector2 direction = player.transform.position - transform.position;
            // RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);
            // indicator.transform.position = ray.point;
            // If a raycast hit the player, use the hit point as the indicator position
            indicator.SetActive(true);
            var spriteSizeInViewPort = mainCamera.WorldToViewportPoint(new Vector3(srWidth, srHeight, 0)) - mainCamera.WorldToViewportPoint(Vector3.zero);
            screenPosition.x = Mathf.Clamp(screenPosition.x, spriteSizeInViewPort.x, 1 - spriteSizeInViewPort.x);
            screenPosition.y = Mathf.Clamp(screenPosition.y, spriteSizeInViewPort.y, 1 - spriteSizeInViewPort.y);
            // Clamp the position to screen bounds with a small buffer to prevent the indicator from going off the edge
            // screenPosition.x = Mathf.Clamp(screenPosition.x, edgeBuffer * Screen.width, (1 - edgeBuffer) * Screen.width);
            // screenPosition.y = Mathf.Clamp(screenPosition.y, edgeBuffer * Screen.height, (1 - edgeBuffer) * Screen.height);

            // Convert screen position to world space
            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPosition);
            worldPosition.z = 0;
            indicator.transform.position = worldPosition;
            Debug.Log("world position : " + indicator.transform.position);

            // Adjust the rotation of the indicator to point toward the player
            // Vector3 directionToPlayer = player.transform.position - mainCamera.transform.position;
            // directionToPlayer.z = 0; // Keep the rotation in 2D
            // Debug.Log("directionToPlayer: " + directionToPlayer);
            // indicator.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToPlayer);
        }
        else
        {
            indicator.SetActive(false); // Hide indicator when the player is on-screen
        }
    }
    // public GameObject indicator;
    // public GameObject target;

    // Renderer rd;
    // // Start is called before the first frame update
    // void Start()
    // {
    //     rd = GetComponent<Renderer>();
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (!rd.isVisible)
    //     {
    //         if (!indicator.activeSelf)
    //         {
    //             indicator.SetActive(true);
    //         }

    //         Vector2 direction = target.transform.position - transform.position;
    //         RaycastHit2D ray = Physics2D.Raycast(transform.position, direction);

    //         if (ray.collider != null)
    //         {
    //             indicator.transform.position = ray.point;
    //         }
    //     }
    //     else
    //     {
    //         if (indicator.activeSelf)
    //         {
    //             indicator.SetActive(false);
    //         }
    //     }
    // }
}
