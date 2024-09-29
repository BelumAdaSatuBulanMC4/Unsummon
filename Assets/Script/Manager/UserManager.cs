using UnityEngine;
using TMPro;

public class UserManager : MonoBehaviour
{
    public static UserManager instance;
    public TMP_InputField usernameInputField;

    private string username;
    private bool isFirstTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        username = DataPersistence.LoadUsername();
        isFirstTime = DataPersistence.GetIsFirstTime();

        TMP_Text placeholderText = usernameInputField.placeholder.GetComponent<TMP_Text>();
        placeholderText.text = username;

        // currentUsernameText.text = $"Current Username: {username}";
        Debug.Log("initial isFirst? " + isFirstTime);
        Debug.Log("Location : " + DataPersistence.GetFilePath());
    }

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
