using UnityEngine;
using UnityEngine.UI;

public class EndGameUI : MonoBehaviour
{
    [SerializeField] Text finalScoreText;
    [SerializeField] Text finalTimeText;

    private void OnEnable()
    {
        finalScoreText.text = GameManager.finalScore.ToString();
        int t = GameManager.finalTime;
        int minutes = t / 60;
        int seconds = t % 60;
        finalTimeText.text = $"{minutes:00}:{seconds:00}";
    }
}
