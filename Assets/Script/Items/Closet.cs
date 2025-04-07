using Unity.Netcode;
using UnityEngine;

public class Closet : MonoBehaviour
{
    public bool isUsed = false;

    public void ClosetActivated()
    {
        if (!isUsed) UpdateUsedServerRpc(true);
    }

    public void ClosetDeActivated()
    {
        if (isUsed) UpdateUsedServerRpc(false);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateUsedServerRpc(bool state)
    {
        if (isUsed != state)
        {
            isUsed = state;
            UpdateUsedClientRpc(state);
        }
    }

    [ClientRpc]
    private void UpdateUsedClientRpc(bool state)
    {
        isUsed = state;
    }
}
