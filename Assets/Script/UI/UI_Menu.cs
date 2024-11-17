using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    // UI PopUp
    [SerializeField] private GameObject UI_SetUsername;
    [Header("Error UI")]
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;

    [Header("GameObject UI")]
    [SerializeField] private GameObject UI_JoinRoom;
    [SerializeField] private GameObject UI_LobbiesList;
    [SerializeField] private GameObject UI_Settings;
    [SerializeField] private GameObject UI_EnterCode;
    // [SerializeField] private GameObject UI_HomeSettings;
    // [SerializeField] private GameObject UI_EditName;
    [Header("Open UI")]
    [SerializeField] private Button open_UI_JoinRoom;
    [SerializeField] private Button open_UI_LobbiesList;
    [SerializeField] private Button open_UI_Settings;
    [SerializeField] private Button open_UI_EnterCode;
    // Close Button
    [Header("Close UI")]
    [SerializeField] private Button close_UI_JoinRoom;
    [SerializeField] private Button close_UI_LobbiesList;
    [SerializeField] private Button close_UI_EnterCode;
    [SerializeField] private Button close_UI_Settings;

    [Header("Close Error UI")]
    [SerializeField] private Button close_UI_PopUpFull;
    [SerializeField] private Button close_UI_PopUpRoomNotFound;
    [SerializeField] private Button close_UI_PopUpLostConnection;
    // [SerializeField] private Button close_UI_EditName;
    private UI_FadeEffect fadeEffect;

    private void Awake()
    {
        fadeEffect = GetComponentInChildren<UI_FadeEffect>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start - UI_Menu Fungsi berhasil dijalankan");
        // OPEN UI
        open_UI_JoinRoom.onClick.AddListener(() => OpenUI(UI_JoinRoom));
        open_UI_LobbiesList.onClick.AddListener(() => OpenUI(UI_LobbiesList));
        open_UI_EnterCode.onClick.AddListener(() => OpenUI(UI_EnterCode));
        open_UI_Settings.onClick.AddListener(() => OpenUI(UI_Settings));
        // CLOSE UI ERROR
        close_UI_PopUpFull.onClick.AddListener(() => CloseUI(UI_PopUpFull));
        close_UI_PopUpRoomNotFound.onClick.AddListener(() => CloseUI(UI_PopUpRoomNotFound));
        close_UI_PopUpLostConnection.onClick.AddListener(() => CloseUI(UI_PopUpLostConnection));
        // CLOSE UI 
        close_UI_JoinRoom.onClick.AddListener(() => CloseUI(UI_JoinRoom));
        close_UI_LobbiesList.onClick.AddListener(() => CloseUI(UI_LobbiesList));
        close_UI_EnterCode.onClick.AddListener(() => CloseUI(UI_EnterCode));
        close_UI_Settings.onClick.AddListener(() => CloseUI(UI_Settings));

        fadeEffect.ScreenFade(0, 4f);
    }


    public void OpenUI(GameObject UI)
    {
        UI.SetActive(true);
    }

    public void CloseUI(GameObject UI)
    {
        Debug.Log("CloseUI - berhasil dijalankan");
        UI.SetActive(false);
    }
}
