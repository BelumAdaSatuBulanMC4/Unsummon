using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetUsername : MonoBehaviour
{
    public GameObject setUsernameUI;  // Reference to the Set Username UI

    private void Start()
    {
        // Load the isFirstTime status from the saved data
        // bool isFirstTime = UsernameManager.instance.CheckFirstTime();
        bool isFirstTime = DataPersistence.GetIsFirstTime();

        Debug.Log("is first time lewat ui setusername : " + isFirstTime);

        // If it's not the first time, deactivate the Set Username UI
        if (!isFirstTime)
        {
            setUsernameUI.SetActive(false);
            Debug.Log("isFirstTime is false, deactivating UI.");
        }
        else
        {
            setUsernameUI.SetActive(true);
            Debug.Log("isFirstTime is true, keeping UI active.");
        }
    }

    private void Update()
    {
        if (DataPersistence.GetIsFirstTime())
        {
            setUsernameUI.SetActive(true);
            // Debug.Log("isFirstTime is true, keeping UI active.");
        }
        else
        {
            setUsernameUI.SetActive(false);
            // Debug.Log("isFirstTime is false, deactivating UI.");
        }
    }
}