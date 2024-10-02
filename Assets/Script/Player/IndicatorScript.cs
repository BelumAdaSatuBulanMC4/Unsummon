using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorScript : MonoBehaviour
{
    public GameObject offScreenIndicatorPrefab;
    public GameObject currentCharacter;
    public Camera mainCamera;
    public float edgeBuffer = 0.1f;
    public float detectionRange = 8f;
    private List<GameObject> players;
    private List<GameObject> activeIndicators = new List<GameObject>();

    private SpriteRenderer sr;
    private float srWidth;
    private float srHeight;

    void Start()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Kids"));
        sr = offScreenIndicatorPrefab.GetComponent<SpriteRenderer>();

        var bounds = sr.bounds;
        srHeight = bounds.size.y / 2f;
        srWidth = bounds.size.x / 2f;

        foreach (GameObject player in players)
        {
            if (player != gameObject)
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
                float distanceToPlayer = Vector3.Distance(players[i].transform.position, currentCharacter.transform.position);
                Debug.Log("distance to player : " + distanceToPlayer);

                if (distanceToPlayer <= detectionRange)
                {
                    UpdateOffScreenIndicator(players[i], activeIndicators[i]);
                }
                else
                {
                    activeIndicators[i].SetActive(false);
                }
            }
        }
    }

    void UpdateOffScreenIndicator(GameObject player, GameObject indicator)
    {
        Vector3 screenPosition = mainCamera.WorldToViewportPoint(player.transform.position);

        bool isOffScreen = screenPosition.x <= 0 || screenPosition.x >= 1 || screenPosition.y <= 0 || screenPosition.y >= 1;

        if (isOffScreen)
        {
            indicator.SetActive(true);

            // Adjust the position of the indicator within the viewport bounds
            var spriteSizeInViewPort = mainCamera.WorldToViewportPoint(new Vector3(srWidth, srHeight, 0)) - mainCamera.WorldToViewportPoint(Vector3.zero);
            screenPosition.x = Mathf.Clamp(screenPosition.x, spriteSizeInViewPort.x, 1 - spriteSizeInViewPort.x);
            screenPosition.y = Mathf.Clamp(screenPosition.y, spriteSizeInViewPort.y, 1 - spriteSizeInViewPort.y);

            Vector3 worldPosition = mainCamera.ViewportToWorldPoint(screenPosition);
            worldPosition.z = 0; // Keep z as 0 to stay on the 2D plane
            indicator.transform.position = worldPosition;
        }
        else
        {
            indicator.SetActive(false); // Hide indicator when the player is on-screen
        }
    }
}
