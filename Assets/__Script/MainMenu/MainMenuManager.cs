using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void Exit()
    {
        Application.Quit();
    }
}