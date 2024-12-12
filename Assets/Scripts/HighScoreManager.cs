using UnityEngine;
using UnityEngine.UI;

public class HighScoreManager : MonoBehaviour
{
    public Text highScoreText;

    public void SaveHighScore(string playerName, int score)
    {
        PlayerPrefs.SetString("HighScoreName", playerName);
        PlayerPrefs.SetInt("HighScore", score);
        PlayerPrefs.Save();
    }

    public (string playerName, int score) LoadHighScore()
    {
        string playerName = PlayerPrefs.GetString("HighScoreName", "Unknown");
        int score = PlayerPrefs.GetInt("HighScore", 0);
        return (playerName, score);
    }

    public void CheckAndUpdateHighScore(string playerName, int newScore)
    {
        var (currentHighName, currentHighScore) = LoadHighScore();

        if (newScore > currentHighScore)
        {
            SaveHighScore(playerName, newScore);
            UpdateHighScoreUI(playerName, newScore);
        }
        else
        {
            UpdateHighScoreUI(currentHighName, currentHighScore);
        }
    }

    private void UpdateHighScoreUI(string playerName, int score)
    {
        highScoreText.text = $"High Score: {playerName} - {score}";
    }

    private void Start()
    {
        var (playerName, score) = LoadHighScore();
        UpdateHighScoreUI(playerName, score);

        CheckAndUpdateHighScore("Player1", 10);
    }
}