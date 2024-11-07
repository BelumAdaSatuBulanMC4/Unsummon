using System.IO;
using UnityEngine;

public class DataPersistence : MonoBehaviour
{
    private static string fileName = "userdata.json";

    private static UserData userData = new UserData();

    public static string GetFilePath()
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }

    private static void LoadUserData()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log($"Data loaded: Username: {userData.username}, isFirstTime: {userData.isFirstTime}");
            Debug.Log(path);
        }
        else
        {
            // Set defaults if file doesn't exist
            userData.username = "username";
            userData.isFirstTime = true;
            SaveUserData(userData.username);
            Debug.Log("Default user data created and saved.");
        }
    }

    public static string LoadUsername()
    {
        LoadUserData(); // Centralized loading logic
        return userData.username;
    }

    public static bool GetIsFirstTime()
    {
        LoadUserData(); // Centralized loading logic
        return userData.isFirstTime;
    }

    public static void SaveUserData(string newUsername)
    {
        userData.username = newUsername;
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log($"UserData saved: Username: {userData.username}, isFirstTime: {userData.isFirstTime}");
    }

    public static void EditUsername(string newUsername)
    {
        userData.username = newUsername;
        userData.isFirstTime = false; // Ensure it's no longer the first time
        SaveUserData(newUsername);
        Debug.Log($"Username changed to: {newUsername}, isFirstTime: {userData.isFirstTime}");
    }

    public static string GetUsername()
    {
        return userData.username;
    }
}
