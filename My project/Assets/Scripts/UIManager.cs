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

    private const string configFilePath = "config.txt"; // ���� � ����� config.txt

    private bool isMusicEnabled = true;
    private string serverAddress = "";
    private string serverPort = "";
    private float musicVolume = 1f;

    private void Start()
    {
        LoadConfig(); // �������� ������������ �� �����

        // ��������� �������� � ��������� ��������� ����������
        serverAddressText.text = serverAddress;
        serverPortText.text = serverPort;
        volumeSlider.value = musicVolume;

        // ���������/���������� ������ ��� ������� ����������
        backgroundMusic.enabled = isMusicEnabled;
        backgroundMusic.volume = musicVolume;
    }

    private void Update()
    {
        // ���������� ��������� ������� ������ ��� ��������� ��������� ��������
        backgroundMusic.volume = volumeSlider.value;
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;

        // ���������/���������� ������
        backgroundMusic.enabled = isMusicEnabled;
    }

    private void LoadConfig()
    {
        string configData = Resources.Load<TextAsset>(configFilePath).text;

        // �������� ���������������� ������ �� ������
        string[] lines = configData.Split('\n');

        foreach (string line in lines)
        {
            if (line.Contains("����� �������"))
            {
                serverAddress = line.Split(':')[1].Trim();
            }
            else if (line.Contains("����"))
            {
                serverPort = line.Split(':')[1].Trim();
            }
        }
    }
}
