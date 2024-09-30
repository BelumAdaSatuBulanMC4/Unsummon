using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    // Dictionary to store each Kid's position and time
    public static PlayerManager instance;
    private Dictionary<PlayerKid, Vector3> kidPositions = new Dictionary<PlayerKid, Vector3>();
    private Dictionary<PlayerSpirit, Vector3> spiritPositions = new Dictionary<PlayerSpirit, Vector3>();
    private List<PlayerKid> allKids = new List<PlayerKid>();

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
            allKids.Add(kid);
        }
    }

    // Unregister a Kid (e.g., if it gets destroyed or exits)
    public void UnregisterKid(PlayerKid kid)
    {
        if (kidPositions.ContainsKey(kid))
        {
            kidPositions.Remove(kid);
            allKids.Remove(kid);
        }
    }

    public List<PlayerKid> GetAllKids()
    {
        return new List<PlayerKid>(allKids); // Return a copy of the list
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

    public void UpdateSpiritPosition(PlayerSpirit spirit, Vector3 position)
    {
        if (spiritPositions.ContainsKey(spirit))
        {
            spiritPositions[spirit] = position;
        }
        else
        {
            spiritPositions.Add(spirit, position);
        }
        StartCoroutine(RemoveSpiritPositionAfterTime(spirit, 7f));
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

    private IEnumerator RemoveSpiritPositionAfterTime(PlayerSpirit spirit, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (spiritPositions.ContainsKey(spirit))
        {
            Debug.Log($"Spirit {spirit.gameObject.name}'s position removed from Manager after {delay} seconds.");
            spiritPositions.Remove(spirit);
        }
    }

    // Provide Pocong with the positions of all Kid characters
    public Dictionary<PlayerKid, Vector3> GetKidPositions()
    {
        return new Dictionary<PlayerKid, Vector3>(kidPositions); // Return a copy to avoid direct manipulation
    }
}
