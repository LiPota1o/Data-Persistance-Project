using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NameInputManager : MonoBehaviour
{
    public TMP_InputField nameInputField; // Поле для ввода имени
    public TMP_Text messageText;  // Для отображения подсказки или сообщения

    public Button startButton; // Кнопка для старта игры

    void Start()
    {
        // При нажатии на кнопку вызываем метод для старта игры
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    // Метод, вызываемый при нажатии на кнопку
    void OnStartButtonClicked()
    {
        string playerName = nameInputField.text;

        if (string.IsNullOrEmpty(playerName))
        {
            playerName = "Player"; // Если имя не введено, используем "Player"
        }

        // Сохраняем имя игрока в PlayerPrefs или передаем в основной менеджер
        PlayerPrefs.SetString("PlayerName", playerName);

        // Загружаем основную сцену
        SceneManager.LoadScene("main");
    }
}
