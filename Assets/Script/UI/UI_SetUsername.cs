using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SetUsername : MonoBehaviour
{
    public GameObject setUsernameUI;
    public TMP_InputField inputUsernameField;

    private void Start()
    {
        bool isFirstTime = DataPersistence.GetIsFirstTime();
        if (!isFirstTime)
        {
            setUsernameUI.SetActive(false);
        }
        else
        {
            setUsernameUI.SetActive(true);
        }
    }

    public void OnSaveButtonClicked()
    {
        string newUsername = inputUsernameField.text;
        DataPersistence.EditUsername(newUsername);
    }

    private void Update()
    {
        if (DataPersistence.GetIsFirstTime())
        {
            setUsernameUI.SetActive(true);
        }
        else
        {
            setUsernameUI.SetActive(false);
        }
    }
}