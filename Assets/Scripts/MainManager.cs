using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    [System.Serializable]
    public class HighScoreData
    {
        public string playerName;
        public int highScore;

        public HighScoreData(string playerName, int highScore)
        {
            this.playerName = playerName;
            this.highScore = highScore;
        }
    }

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;  // Стандартный Text для отображения счета
    public GameObject GameOverText;
    public Text HighScoreText;  // Стандартный Text для отображения рекорда

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;

    private string playerName;
    private int highScore;

    private string filePath;

    // Start is called before the first frame update
    void Start()
    {
        filePath = Application.persistentDataPath + "/highscore.json";
        LoadHighScore();
        HighScoreText.text = $"High Score: {playerName}: {highScore}";

        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("Menu");
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        // Сохраняем рекорд, если новый счёт выше текущего рекорда
        if (m_Points > highScore)
        {
            highScore = m_Points;

            // Получаем имя игрока из PlayerPrefs
            playerName = PlayerPrefs.GetString("PlayerName", "No name");

            // Сохраняем новое имя и рекорд в файл, только если рекорд был побит
            HighScoreData highScoreData = new HighScoreData(playerName, highScore);
            string json = JsonUtility.ToJson(highScoreData);
            File.WriteAllText(filePath, json);

            // Обновляем отображение рекорда
            HighScoreText.text = $"High Score: {playerName}: {highScore}";
        }
    }

    void LoadHighScore()
    {
        // Загружаем имя игрока из PlayerPrefs
        playerName = PlayerPrefs.GetString("PlayerName", "No name");

        // Загружаем рекорд из файла, если он существует
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(json);
            highScore = highScoreData.highScore;
            playerName = highScoreData.playerName;
        }
        else
        {
            highScore = 0;  // Если файл рекордов не найден, устанавливаем 0
        }
    }
}
