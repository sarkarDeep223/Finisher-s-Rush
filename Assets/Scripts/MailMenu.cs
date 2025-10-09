using UnityEngine;
using UnityEngine.SceneManagement;

public class MailMenu : MonoBehaviour
{

    public void QuitGame()
    {
        Debug.Log("Quit Game"); // for testing in the Editor
        Application.Quit();
    }

    public void PlayGame()
    {
        Debug.Log("Play Game");
        SceneManager.LoadScene("MainGame");
    }

}
