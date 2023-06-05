using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, Board;
    public AudioSource MusicStop;

    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

    }

    public void PauseGame()
    {
        MusicStop.volume = 0.2f;
        pauseMenu.SetActive(true);
        Board.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void ResumeGame()
    {
        MusicStop.volume = 1f;
        pauseMenu.SetActive(false);
        Board.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }
    public void BacktoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ResetGameEasy()
    {
        SceneManager.LoadScene("Easy");
        pauseMenu.SetActive(false);
        Board.SetActive(true);
        Time.timeScale = 1f;
    }
    public void ResetGameNor()
    {
        SceneManager.LoadScene("Normal");
        pauseMenu.SetActive(false);
        Board.SetActive(true);
        Time.timeScale = 1f;
    }
    public void ResetGameHard()
    {
        SceneManager.LoadScene("Hard");
        pauseMenu.SetActive(false);
        Board.SetActive(true);
        Time.timeScale = 1f;
    }
}
