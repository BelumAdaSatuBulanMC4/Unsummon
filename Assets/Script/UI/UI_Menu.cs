using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Menu : MonoBehaviour
{
    [SerializeField] private GameObject UI_SetUsername;
    [SerializeField] private GameObject UI_HomeSetting;
    [SerializeField] private GameObject UI_JoinLobby;
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonJoin(){
        UI_JoinLobby.SetActive(true);
    }

    public void CancelJoin(){
        UI_JoinLobby.SetActive(false);
    }
}
