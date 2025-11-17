using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject losePanel;
    [SerializeField] GameObject winnerPanel;
    [SerializeField] SpriteRenderer[] lifeSprites;
    [SerializeField] SpriteRenderer[] lifeSprites2;
    [SerializeField] Text gameTimeText;
    [SerializeField] Text scoreText;
    [SerializeField] Text highScoreText;
    [SerializeField] Text bestTimeText;

    [SerializeField] Text comboText;
    [SerializeField] Text comboTimeText;
    
    [SerializeField] AudioClip buttonPress;

    // Actualiza el tiempo de la partida en pantalla
    public void UpdateGameTimeUI(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        gameTimeText.text = $"{minutes:0}:{seconds:00}";
    }

    // Activa panel de derrota
    public void ActivateLosePanel()
    {
        losePanel.SetActive(true);
    }

    // Activa panel de victoria y muestra tiempo final y mejor tiempo
    public void ActivateWinnerPanel(float finalTime, int bestTime)
    {
        winnerPanel.SetActive(true);

        // Mostrar tiempo de la partida
        int minutes = Mathf.FloorToInt(finalTime / 60);
        int seconds = Mathf.FloorToInt(finalTime % 60);
        gameTimeText.text = $"{minutes:0}:{seconds:00}";

        // Mostrar mejor tiempo
        UpdateBestTime(bestTime);
    }

    // Actualiza las vidas del jugador en UI
    public void UpdateUILives(byte currentLives)
    {
        for (int i = 0; i < lifeSprites.Length; i++)
        {
            lifeSprites[i].color = Color.green;
            lifeSprites2[i].color = Color.green;
        }
        for (int i = currentLives; i < lifeSprites.Length; i++)
        {
            lifeSprites[i].color = Color.red;
            lifeSprites2[i].color = Color.red;
        }
    }

    // Actualiza puntaje en UI
    public void UpdateScore(int newScore)
    {
        scoreText.text = $"{newScore}";
    }

    // Actualiza high score en UI
    public void UpdateHighScore(int bestScore)
    {
        highScoreText.text = bestScore.ToString();
    }

    // Muestra null si nunca hubo high score
    public void UpdateHighScoreTextNull()
    {
        highScoreText.text = "null";
    }

    // Actualiza mejor tiempo en UI
    public void UpdateBestTime(int bestTime)
    {
        if (bestTime == int.MaxValue)
        {
            bestTimeText.text = "null";
        }
        else
        {
            int minutes = bestTime / 60;
            int seconds = bestTime % 60;
            bestTimeText.text = $"{minutes:0}:{seconds:00}";
        }
    }

    // Muestra null si nunca hubo mejor tiempo
    public void UpdateBestTimeTextNull()
    {
        bestTimeText.text = "null";
    }

    // Reinicia la escena actual
    public void ResetCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        FindObjectOfType<AudioController>().PlaySfx(buttonPress);
    }

    // Volver al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        FindObjectOfType<AudioController>().PlaySfx(buttonPress);
    }
    public void UpdateCombo(int multiplier, bool isActive) 
    {
        if (isActive)
        {
            comboText.text = "x" + multiplier;
        }
        else
        {
            comboText.text = "";
        }
    }
    public void UpdateComboTimer(float timeLeft) 
    {
        comboTimeText.text = timeLeft.ToString("0.0") + "s";
    }
    public void ClearComboTimer()
    {
        comboTimeText.text = "";
    }
}