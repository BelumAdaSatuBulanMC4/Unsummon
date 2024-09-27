using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class LobbyDisplay : MonoBehaviour
{
    public TextMeshProUGUI codeRoomOutput;

    public void UpdateRoomCode(string joinCode) {
        codeRoomOutput.text = joinCode;
    }
}
