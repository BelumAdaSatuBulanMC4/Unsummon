using UnityEngine;
using TMPro;
using System;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;
    public TMP_InputField usernameInputField;

    private string username;
    private bool isFirstTime;

    private string yourRole;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Destroy(gameObject);
        }
    }
    private void Start()
    {
        FirstTimeUserName();
        // username = DataPersistence.LoadUsername();
        // isFirstTime = DataPersistence.GetIsFirstTime();

        // TMP_Text placeholderText = usernameInputField.placeholder.GetComponent<TMP_Text>();
        // placeholderText.text = username;

        // currentUsernameText.text = $"Current Username: {username}";
        // Debug.Log("initial isFirst? " + isFirstTime);
        Debug.Log("Location : " + DataPersistence.GetFilePath());
    }

    private void FirstTimeUserName()
    {
        username = DataPersistence.LoadUsername();
        isFirstTime = DataPersistence.GetIsFirstTime();

        if (isFirstTime)
        {
            TMP_Text placeholderText = usernameInputField.placeholder.GetComponent<TMP_Text>();
            placeholderText.text = username;
        }
    }
    public void SetYourRole(string role)
    {
        yourRole = role;
    }

    public string getYourRole() => yourRole;

    public void OnSaveButtonClicked()
    {
        string newUsername = usernameInputField.text;
        DataPersistence.EditUsername(newUsername);

        // currentUsernameText.text = $"Current Username: {newUsername}";
        Debug.Log("change the isFirst? " + DataPersistence.GetIsFirstTime());
        usernameInputField.text = "";

        username = DataPersistence.LoadUsername();
        isFirstTime = DataPersistence.GetIsFirstTime();

        TMP_Text placeholderText = usernameInputField.placeholder.GetComponent<TMP_Text>();
        placeholderText.text = username;
    }

    public bool CheckFirstTime()
    {
        return isFirstTime;
    }
}
