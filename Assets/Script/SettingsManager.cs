using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;
    private string username;
    private bool isFirstTime = true;

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

    private void Update()
    {
        if (isFirstTime)
            CheckStatus();
    }

    public bool CheckStatus()
    {
        // isFirstTime = DataPersistenceManager.instance.CheckFirstTime();
        Debug.Log("first time!");
        return isFirstTime;
    }

    // public saveUserName()
    // {
    //     DataPersistenceManager.instance.gameData.username = username;
    // }

}
