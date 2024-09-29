using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using UnityEngine.UI; // For Button

public class Settings : MonoBehaviour, IDataPersistence
{
    // Variables to store username and first-time flag
    private string username;
    private bool isFirstTime;

    // Reference to TMP input field and button
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private Button saveButton;

    // Load data method from IDataPersistence
    public void LoadData(GameData data)
    {
        this.username = data.username;
        this.isFirstTime = data.isFirst;

        // usernameInputField.text = username;

        // Set the input field's value to the loaded username
        // usernameInputField.text = username;
    }

    // Save data method from IDataPersistence
    public void SaveData(ref GameData data)
    {
        data.username = usernameInputField.text;
        data.isFirst = isFirstTime;
    }

    // Method to be called on start
    // private void Start()
    // {
    //     // Add listener to the save button
    //     saveButton.onClick.AddListener(OnClickSaveButton);

    //     // Set the username field to the current value in the input field
    //     username = usernameInputField.text;
    // }

    public void OnClickSaveButton()
    {
        DataPersistenceManager.instance.SaveGame();
        Debug.Log("username now is " + DataPersistenceManager.instance.CheckUsername());
    }
    // Method to handle saving the username
    private void SaveUsername()
    {
        // Update the username with the value from the input field
        username = usernameInputField.text;

        // Trigger saving the data (could be handled elsewhere in a DataManager)
        // For example: DataPersistenceManager.Instance.SaveGame();
    }
}
