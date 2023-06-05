using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("LevelMenu");
        
    }
    public void BacktoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Easy()
    {
        SceneManager.LoadScene("Easy");
    }
    public void Normal()
    {
        SceneManager.LoadScene("Normal");
    }
    public void Hard()
    {
        SceneManager.LoadScene("Hard");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}
