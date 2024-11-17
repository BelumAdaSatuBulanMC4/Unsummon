using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : MonoBehaviour
{
    // UI PopUp
    [SerializeField] private GameObject UI_SetUsername;
    [SerializeField] private GameObject UI_JoinLobby;
    [SerializeField] private GameObject UI_PopUpFull;
    [SerializeField] private GameObject UI_PopUpRoomNotFound;
    [SerializeField] private GameObject UI_PopUpLostConnection;
    [SerializeField] private GameObject UI_LobbiesList;
    // [SerializeField] private GameObject UI_HomeSettings;
    // [SerializeField] private GameObject UI_EditName;
    // Close Button
    [SerializeField] private Button close_UI_JoinLobby;
    [SerializeField] private Button close_UI_PopUpFull;
    [SerializeField] private Button close_UI_PopUpRoomNotFound;
    [SerializeField] private Button close_UI_PopUpLostConnection;
    [SerializeField] private Button open_UI_LobbiesList;
    [SerializeField] private Button close_UI_LobbiesList;
    // [SerializeField] private Button close_UI_HomeSettings;
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
        // CLOSE UI
        close_UI_JoinLobby.onClick.AddListener(() => CloseUI(UI_JoinLobby));
        close_UI_PopUpFull.onClick.AddListener(() => CloseUI(UI_PopUpFull));
        close_UI_PopUpRoomNotFound.onClick.AddListener(() => CloseUI(UI_PopUpRoomNotFound));
        close_UI_PopUpLostConnection.onClick.AddListener(() => CloseUI(UI_PopUpLostConnection));
        open_UI_LobbiesList.onClick.AddListener(() => OpenUI(UI_LobbiesList));
        close_UI_LobbiesList.onClick.AddListener(() => CloseUI(UI_LobbiesList));

        fadeEffect.ScreenFade(0, 4f);
    }

    public void ButtonJoin()
    {
        UI_JoinLobby.SetActive(true);
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
