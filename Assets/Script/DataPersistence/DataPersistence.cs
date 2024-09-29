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

    public static void SaveUserData(string newUsername)
    {
        userData.username = newUsername;
        string json = JsonUtility.ToJson(userData);
        File.WriteAllText(GetFilePath(), json);
        Debug.Log($"Username saved: {newUsername}, isFirstTime: {userData.isFirstTime}");
    }

    public static string LoadUsername()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log($"Username loaded: {userData.username}, isFirstTime: {userData.isFirstTime}");
        }
        else
        {
            userData.username = "username";
            userData.isFirstTime = true;
            SaveUserData(userData.username);
        }
        return userData.username;
    }

    public static string GetUsername()
    {
        return userData.username;
    }

    public static bool GetIsFirstTime()
    {
        string path = GetFilePath();
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log($"Username loaded: {userData.username}, isFirstTime: {userData.isFirstTime}");
        }
        else
        {
            userData.username = "username";
            userData.isFirstTime = true;
            SaveUserData(userData.username);
        }
        return userData.isFirstTime;
    }

    public static void EditUsername(string newUsername)
    {
        userData.isFirstTime = false;
        SaveUserData(newUsername);
        Debug.Log($"Username changed to: {newUsername}, isFirstTime: {userData.isFirstTime}");
    }
}
