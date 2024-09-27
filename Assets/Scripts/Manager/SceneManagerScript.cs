using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void GoToLobbies()
    {
        SceneManager.LoadScene("LobbyRoom");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

