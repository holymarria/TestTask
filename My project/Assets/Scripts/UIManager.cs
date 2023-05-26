using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public Text serverAddressText;
    public Text serverPortText;
    public Slider volumeSlider;

    private const string configFilePath = "config.txt"; // Путь к файлу config.txt

    private bool isMusicEnabled = true;
    private string serverAddress = "";
    private string serverPort = "";
    private float musicVolume = 1f;

    private void Start()
    {
        LoadConfig(); // Загрузка конфигурации из файла

        // Настройка значений и состояний элементов интерфейса
        serverAddressText.text = serverAddress;
        serverPortText.text = serverPort;
        volumeSlider.value = musicVolume;

        // Включение/выключение музыки при запуске приложения
        backgroundMusic.enabled = isMusicEnabled;
        backgroundMusic.volume = musicVolume;
    }

    private void Update()
    {
        // Обновление громкости фоновой музыки при изменении положения ползунка
        backgroundMusic.volume = volumeSlider.value;
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;

        // Включение/выключение музыки
        backgroundMusic.enabled = isMusicEnabled;
    }

    private void LoadConfig()
    {
        string configData = Resources.Load<TextAsset>(configFilePath).text;

        // Разбивка конфигурационных данных на строки
        string[] lines = configData.Split('\n');

        foreach (string line in lines)
        {
            if (line.Contains("Адрес сервера"))
            {
                serverAddress = line.Split(':')[1].Trim();
            }
            else if (line.Contains("Порт"))
            {
                serverPort = line.Split(':')[1].Trim();
            }
        }
    }
}
