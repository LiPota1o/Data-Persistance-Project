using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // ���� ��� ����� �����
    public TMP_Text messageText;  // ��� ����������� ��������� ��� ���������

    public Button startButton; // ������ ��� ������ ����

    void Start()
    {
        // ��� ������� �� ������ �������� ����� ��� ������ ����
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    // �����, ���������� ��� ������� �� ������
    void OnStartButtonClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player"; // ���� ��� �� �������, ���������� "Player"
        }

        // ��������� ��� ������ � PlayerPrefs ��� �������� � �������� ��������
        PlayerPrefs.SetString("PlayerName", playerName);

        // ��������� �������� �����
        SceneManager.LoadScene("main");
    }
}
