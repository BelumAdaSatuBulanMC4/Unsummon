using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    public static ScreenManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }
    // private void Awake()
    // {
    //     Screen.sleepTimeout = SleepTimeout.NeverSleep;
    //     DontDestroyOnLoad(gameObject);
    // }

}
