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
    private Vector3 pocongPosition = new Vector3();
    private List<PlayerKid> allKids = new List<PlayerKid>();

    private Dictionary<ulong, Vector3> kidPositionsNET = new Dictionary<ulong, Vector3>();

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
            Debug.Log("alreadyn containing " + kid.ToString());
            kidPositions[kid] = position;
        }
        else
        {
            kidPositions.Add(kid, position);
            Debug.Log("adding kid position " + kidPositions[kid]);
        }

        // Start coroutine to remove the position after 7 seconds
        // StartCoroutine(RemoveKidPositionAfterTime(kid, 5f));
    }

    public void RemoveKidPosition(PlayerKid kid)
    {
        if (kidPositions.ContainsKey(kid))
        {
            Debug.Log($"Kid {kid.gameObject.name}'s position removed from Manager.");
            kidPositions.Remove(kid);
        }
    }

    public void UpdatePocongPosition(Pocong pocong, Vector3 position)
    {
        pocongPosition = position;

        // Start coroutine to remove the position after 7 seconds
        StartCoroutine(RemovePocongPositionAfterTime(pocong, 5f));
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
        StartCoroutine(RemoveSpiritPositionAfterTime(spirit, 5f));
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

    private IEnumerator RemovePocongPositionAfterTime(Pocong pocong, float delay)
    {
        yield return new WaitForSeconds(delay);
        pocongPosition = Vector3.zero;
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
        Debug.Log("is get the kids positions " + kidPositions.ToString());
        // return new Dictionary<PlayerKid, Vector3>(kidPositions); // Return a copy to avoid direct manipulation
        return kidPositions;
    }

    public bool IsContainingKid(PlayerKid kid)
    {
        return kidPositions.ContainsKey(kid);
    }

    // Server RPC for Kids to update their position
    [ServerRpc(RequireOwnership = false)]
    public void UpdateKidPositionServerRpc(ulong kidId, Vector3 newPosition)
    {
        if (IsServer)
        {
            // Update the position in the dictionary
            if (kidPositionsNET.ContainsKey(kidId))
            {
                kidPositionsNET[kidId] = newPosition;
            }
            else
            {
                kidPositionsNET.Add(kidId, newPosition);
            }

            // Broadcast to all clients
            UpdateKidPositionClientRpc(kidId, newPosition);
        }
    }

    // Client RPC to broadcast position update to all clients
    [ClientRpc]
    public void UpdateKidPositionClientRpc(ulong kidId, Vector3 newPosition)
    {
        // Update the kid's position on all clients
        if (kidPositionsNET.ContainsKey(kidId))
        {
            kidPositionsNET[kidId] = newPosition;
        }
        else
        {
            kidPositionsNET.Add(kidId, newPosition);
        }
    }

    // Server RPC to update Pocong's position (sent by Pocong client)
    [ServerRpc(RequireOwnership = false)]
    public void UpdatePocongPositionServerRpc(Vector3 newPosition)
    {
        if (IsServer)
        {
            pocongPosition = newPosition;

            // Broadcast to all clients
            UpdatePocongPositionClientRpc(newPosition);
        }
    }

    // Client RPC to broadcast Pocong's position to all clients
    [ClientRpc]
    public void UpdatePocongPositionClientRpc(Vector3 newPosition)
    {
        pocongPosition = newPosition;
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveKidPositionServerRpc(ulong kidId)
    {
        if (IsServer)
        {
            // Remove from the server's kidPositions dictionary
            if (kidPositionsNET.ContainsKey(kidId))
            {
                kidPositionsNET.Remove(kidId);
                Debug.Log($"Kid {kidId}'s position removed from server.");

                // Notify all clients to remove this position
                RemoveKidPositionClientRpc(kidId);
            }
        }
    }

    [ClientRpc]
    private void RemoveKidPositionClientRpc(ulong kidId)
    {
        // Remove from each client's kidPositions dictionary
        if (kidPositionsNET.ContainsKey(kidId))
        {
            kidPositionsNET.Remove(kidId);
            Debug.Log($"Kid {kidId}'s position removed on client.");
        }
    }


    public Dictionary<ulong, Vector3> GetKidPositionsNET()
    {
        return kidPositionsNET;
    }
}
