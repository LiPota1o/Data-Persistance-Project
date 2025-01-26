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

    public Text ScoreText;  // ����������� Text ��� ����������� �����
    public GameObject GameOverText;
    public Text HighScoreText;  // ����������� Text ��� ����������� �������

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

        // ��������� ������, ���� ����� ���� ���� �������� �������
        if (m_Points > highScore)
        {
            highScore = m_Points;

            // �������� ��� ������ �� PlayerPrefs
            playerName = PlayerPrefs.GetString("PlayerName", "No name");

            // ��������� ����� ��� � ������ � ����, ������ ���� ������ ��� �����
            HighScoreData highScoreData = new HighScoreData(playerName, highScore);
            string json = JsonUtility.ToJson(highScoreData);
            File.WriteAllText(filePath, json);

            // ��������� ����������� �������
            HighScoreText.text = $"High Score: {playerName}: {highScore}";
        }
    }

    void LoadHighScore()
    {
        // ��������� ��� ������ �� PlayerPrefs
        playerName = PlayerPrefs.GetString("PlayerName", "No name");

        // ��������� ������ �� �����, ���� �� ����������
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            HighScoreData highScoreData = JsonUtility.FromJson<HighScoreData>(json);
            highScore = highScoreData.highScore;
            playerName = highScoreData.playerName;
        }
        else
        {
            highScore = 0;  // ���� ���� �������� �� ������, ������������� 0
        }
    }
}
