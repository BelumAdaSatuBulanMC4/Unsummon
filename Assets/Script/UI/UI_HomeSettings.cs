using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_HomeSettings : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textUsername;
    [SerializeField] private Button editNameButton;
    [SerializeField] private GameObject UI_HomeSettingsObject;
    [SerializeField] private GameObject UI_EditNameObject;
    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        editNameButton.onClick.AddListener(() => UI_EditNameObject.SetActive(true));
        closeButton.onClick.AddListener(() => UI_HomeSettingsObject.SetActive(false));
    }

    void Update()
    {
        textUsername.text = DataPersistence.LoadUsername();
    }

}
