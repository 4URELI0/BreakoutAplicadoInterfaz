using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] byte bricksOnLevel;
    [SerializeField] byte playerLives = 3;

    [SerializeField] bool gameStarted;
    [SerializeField] bool ballOnPlay;

    public bool bigSize = false;
    public bool bigSpeed = false;

    public int score = 0;
    public int highScore = 0;

    private bool timerStarted = false;
    private bool gameFinished = false;
    private float startTime;

    // Propiedad para controlar los bloques restantes
    public byte BricksOnLevel
    {
        get => bricksOnLevel;
        set
        {
            bricksOnLevel = value;

            if (bricksOnLevel == 0 && !gameFinished)
            {
                gameFinished = true;
                int finalTime = Mathf.RoundToInt(Time.timeSinceLevelLoad - startTime);
                int bestTime = PlayerPrefs.GetInt("BestTime", int.MaxValue);
                if (finalTime < bestTime)
                {
                    bestTime = finalTime;
                    PlayerPrefs.SetInt("BestTime", bestTime); 
                }
                PlayerPrefs.SetInt("HasPlayedBeforeTime", 1);
                FindObjectOfType<UIController>().ActivateWinnerPanel(finalTime, bestTime);
                GameObject ball = GameObject.Find("Ball");
                if (ball != null) 
                {
                   Destroy(ball);
                } 
            }
        }
    }

    public byte PlayerLives
    {
        get => playerLives;
        set
        {
            playerLives = value;
            FindObjectOfType<UIController>().UpdateUILives(playerLives);

            if (playerLives == 0)
            {
                gameFinished = true;
                GameObject ball = GameObject.Find("Ball");
                if (ball != null) Destroy(ball);
                FindObjectOfType<UIController>().ActivateLosePanel();
                PlayerPrefs.SetInt("HasPlayedBefore", 1);
            }
            else
            {
                FindAnyObjectByType<Ball>()?.ResetBall();
            }
        }
    }
    public bool GameStarted
    {
        get => gameStarted;
        set
        {
            gameStarted = value;
            if (!timerStarted && !gameFinished)
            {
                startTime = Time.timeSinceLevelLoad;
                timerStarted = true;
            }
        }
    }
    public bool BallOnPlay
    {
        get => ballOnPlay;
        set
        {
            ballOnPlay = value;
            if (ballOnPlay && !timerStarted)
            {
                startTime = Time.timeSinceLevelLoad;
                timerStarted = true;
            }

            if (ballOnPlay)
            {
                FindObjectOfType<Ball>().LaunchBall();
            }
        }
    }

    private void Start()
    {
        // Iniciar timer
        startTime = Time.timeSinceLevelLoad;
        timerStarted = true;

        // Cargar mejor tiempo guardado
        int hasPlayedTime = PlayerPrefs.GetInt("HasPlayedBeforeTime", 0);
        if (hasPlayedTime == 0)
        {
            FindObjectOfType<UIController>().UpdateBestTimeTextNull();
        }
        else
        {
            // Obtener mejor tiempo como entero
            int bestTime = PlayerPrefs.GetInt("BestTime", int.MaxValue);
            FindObjectOfType<UIController>().UpdateBestTime(bestTime);
        }

        // Cargar high score
        int hasPlayed = PlayerPrefs.GetInt("HasPlayedBefore", 0);
        if (hasPlayed == 1)
        {
            highScore = PlayerPrefs.GetInt("HighScore", 0);
            FindObjectOfType<UIController>().UpdateHighScore(highScore);
        }
        else
        {
            FindObjectOfType<UIController>().UpdateHighScoreTextNull();
        }
    }

    private void Update()
    {
        if (timerStarted && !gameFinished)
        {
            float elapsed = Time.timeSinceLevelLoad - startTime;
            FindObjectOfType<UIController>().UpdateGameTimeUI(elapsed);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        FindObjectOfType<UIController>().UpdateScore(score);

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            FindObjectOfType<UIController>().UpdateHighScore(highScore);
        }
    }
}
