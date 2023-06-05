using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Text ScoreText;
    public Text ScoreText2;
    public int CountScore = 0, HighScore = 0;
    public void SetScoreText(int ScoreNum)
    {
        if (ScoreText)
        {
            ScoreText.text = "" + ScoreNum;
        }

    }
    public void HighScoreText(int HighScore)
    {
        if (ScoreText2)
        {
            ScoreText2.text = "Score: " + HighScore;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
        SetScoreText(CountScore);
        HighScoreText(HighScore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
