using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // Dictionary to store each Kid's position and time
    public static PlayerManager instance;
    private Dictionary<PlayerKid, Vector3> kidPositions = new Dictionary<PlayerKid, Vector3>();

    // Register a Kid and start tracking its position

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void RegisterKid(PlayerKid kid)
    {
        if (!kidPositions.ContainsKey(kid))
        {
            kidPositions.Add(kid, kid.transform.position);
        }
    }

    // Unregister a Kid (e.g., if it gets destroyed or exits)
    public void UnregisterKid(PlayerKid kid)
    {
        if (kidPositions.ContainsKey(kid))
        {
            kidPositions.Remove(kid);
        }
    }

    // Update the position of a specific Kid and start a coroutine to remove it after 7 seconds
    public void UpdateKidPosition(PlayerKid kid, Vector3 position)
    {
        if (kidPositions.ContainsKey(kid))
        {
            kidPositions[kid] = position;
        }
        else
        {
            kidPositions.Add(kid, position);
        }

        // Start coroutine to remove the position after 7 seconds
        StartCoroutine(RemoveKidPositionAfterTime(kid, 7f));
    }

    // Coroutine to remove the Kid's position after a delay
    private IEnumerator RemoveKidPositionAfterTime(PlayerKid kid, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (kidPositions.ContainsKey(kid))
        {
            Debug.Log($"Kid {kid.gameObject.name}'s position removed from Manager after {delay} seconds.");
            kidPositions.Remove(kid);
        }
    }

    // Provide Pocong with the positions of all Kid characters
    public Dictionary<PlayerKid, Vector3> GetKidPositions()
    {
        return new Dictionary<PlayerKid, Vector3>(kidPositions); // Return a copy to avoid direct manipulation
    }
}
