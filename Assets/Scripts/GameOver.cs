using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOver, Board;
    private bool pressEscape = true;
    Score refScore;
    int HighScore;
    public GameObject MusicStop;

    // Start is called before the first frame update
    void Start()
    {
        gameOver.SetActive(false);
        refScore = FindObjectOfType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape) && pressEscape == false)
            {
                Time.timeScale = 1f;
                SceneManager.LoadScene("MainMenu");
            }
    }
    public void GameisOver()
    {
        MusicStop.SetActive(false);
        gameOver.SetActive(true);
        Board.SetActive(false);
        Time.timeScale = 0f;
        pressEscape = false;
    }
    
    public void PlayAgain()
    {
        MusicStop.SetActive(true);
        gameOver.SetActive(false);
        Board.SetActive(true);
        Time.timeScale = 1f;
        pressEscape = true;
        
    }
}
