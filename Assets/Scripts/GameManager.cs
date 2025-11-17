using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Hierarchy;
using UnityEngine;
using System;
using Debug = UnityEngine.Debug;
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

    public bool comboActive = false;
    public int comboMultiplier = 1;
    [SerializeField] float comboDuration;
    float comboTimer = 0;
    int blocksInCombo = 0;

    public void BlockDestroyed() 
    {
        blocksInCombo++;
        comboTimer = comboDuration;
        if (blocksInCombo >= 2)
        {
            comboActive = true;
            comboMultiplier = blocksInCombo;
            FindObjectOfType<UIController>().UpdateCombo(comboMultiplier, comboActive);
        }
    }

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
                // Game Over
                GameObject ball = GameObject.Find("Ball");
                if (ball != null) Destroy(ball);

                FindObjectOfType<UIController>().ActivateLosePanel();
            }
            else
            {
                // RESETEO DE LA PELOTA (ESTO FALTABA)
                FindObjectOfType<Ball>().ResetBall();
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
            FindObjectOfType<UIController>().UpdateGameTimeUI(timer);
        }
        if (comboActive)
        {
            comboTimer -= Time.deltaTime;
            FindObjectOfType<UIController>().UpdateComboTimer(comboTimer);
            if (comboTimer <= 0f)
            {
                comboActive = false;
                comboMultiplier = 1;
                blocksInCombo = 0;
                FindObjectOfType<UIController>().UpdateCombo(1, false);
                FindObjectOfType<UIController>().ClearComboTimer();
            }
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