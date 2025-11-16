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
    private float timer = 0f;

    public static int finalScore;
    public static int finalTime;

    public byte BricksOnLevel
    {
        get => bricksOnLevel;
        set
        {
            bricksOnLevel = value;

            if (bricksOnLevel == 0 && !gameFinished)
            {
                gameFinished = true;
                finalScore = score;
                finalTime = Mathf.RoundToInt(timer);

                int bestTime = PlayerPrefs.GetInt("BestTime", int.MaxValue);

                if (finalTime < bestTime)
                {
                    bestTime = finalTime;
                    PlayerPrefs.SetInt("BestTime", bestTime);
                }

                PlayerPrefs.SetInt("HasPlayedBeforeTime", 1);

                FindObjectOfType<UIController>().ActivateWinnerPanel(finalTime, bestTime);

                GameObject ball = GameObject.Find("Ball");
                if (ball != null) Destroy(ball);
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

                finalScore = score;
                finalTime = Mathf.RoundToInt(timer);

                GameObject ball = GameObject.Find("Ball");
                if (ball != null) Destroy(ball);

                FindObjectOfType<UIController>().ActivateLosePanel();
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
        timerStarted = true;

        int hasPlayedTime = PlayerPrefs.GetInt("HasPlayedBeforeTime", 0);
        if (hasPlayedTime == 0)
        {
            FindObjectOfType<UIController>().UpdateBestTimeTextNull();
        }
        else
        {
            int bestTime = PlayerPrefs.GetInt("BestTime", int.MaxValue);
            FindObjectOfType<UIController>().UpdateBestTime(bestTime);
        }

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
            timer += Time.deltaTime;
            Debug.Log("Timer: " + timer);
            FindObjectOfType<UIController>().UpdateGameTimeUI(timer);
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
