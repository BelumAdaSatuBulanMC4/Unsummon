using UnityEngine;

public class ScreenManager : MonoBehaviour
{
    private void Awake()
    {
        // // Cek jika ada instance lain dari KeepScreenAwake
        // if (FindObjectsOfType<ScreenManager>().Length > 1)
        // {
        //     Destroy(gameObject); // Hancurkan instance ini jika sudah ada yang lain
        //     return;
        // }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(gameObject);
    }


    // private void OnDestroy()
    // {
    //     Screen.sleepTimeout = SleepTimeout.SystemSetting;
    // }
}
