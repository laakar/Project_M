using System;
using UnityEngine;
public class SceneManager : MonoBehaviour
{
    private void Start()
    {
        GameEvents.current.OnChangeScene += ReturnToMenu;
    }

    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void PlayGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Test_Scene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
