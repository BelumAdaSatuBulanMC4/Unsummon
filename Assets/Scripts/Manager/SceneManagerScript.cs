using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript instance;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }
    public void GoToLobbies()
    {
        SceneManager.LoadScene("LobbyRoom");
    }
    public void GoToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void GoToGamePlay()
    {
        SceneManager.LoadScene("GamePlay");
    }
}

