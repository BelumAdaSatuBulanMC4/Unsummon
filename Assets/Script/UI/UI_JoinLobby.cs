using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_JoinLobby : MonoBehaviour
{
    [SerializeField] private TMP_InputField joinCodeInput;
    [SerializeField] private Button joinButton;
    // [SerializeField] private Button closeJoin;

    private void Start() {
        joinButton.onClick.AddListener(JoinClient);
    }

    // Start is called before the first frame update
    public void JoinClient(){
        ClientManager.Instance.StartClient(joinCodeInput.text);
        SceneManagerScript.instance.GoToLobbies();
    }
}
