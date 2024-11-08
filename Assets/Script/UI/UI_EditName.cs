using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_EditName : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputUsername;
    [SerializeField] private Button saveButton;
    [SerializeField] private GameObject UI_EditNameObject;
    [SerializeField] private Button closeButton;

    // Start is called before the first frame update
    void Start()
    {
        saveButton.onClick.AddListener(SaveUsername);
        closeButton.onClick.AddListener(() => UI_EditNameObject.SetActive(false));
    }

    private void SaveUsername()
    {
        DataPersistence.EditUsername(inputUsername.text);
        UI_EditNameObject.SetActive(false);
    }
}
