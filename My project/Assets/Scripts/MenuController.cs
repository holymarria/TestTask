using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuController : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public TMP_Text serverAddressText;
    public TMP_Text serverPortText;
    public Slider volumeSlider;
    public Image musicButtonImage;
    public Sprite musicOnSprite;
    public Sprite musicOffSprite;
    public Toggle rememberChoiceToggle;
    public Button returnButton; // ������ ��� ��������

    public GameObject currentCanvas; // ������ �� ������� Canvas
    public GameObject nextCanvas; // ������ �� Canvas, ������� ����� ������������

    private string serverAddress;
    private int serverPort;
    private bool isMusicEnabled = true;
    private bool rememberChoice = true;

    private const string configFile = "config.txt";
    private const string rememberChoiceKey = "RememberChoice";

    private void Start()
    {
        LoadConfig();
        LoadRememberChoice();
        UpdateUI();
        PlayBackgroundMusic();

        returnButton.onClick.AddListener(ReturnToPreviousScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMusic();
        }
    }

    public void ToggleMusic()
    {
        isMusicEnabled = !isMusicEnabled;

        if (isMusicEnabled)
        {
            PlayBackgroundMusic();
            musicButtonImage.sprite = musicOnSprite;
        }
        else
        {
            StopBackgroundMusic();
            musicButtonImage.sprite = musicOffSprite;
        }

        if (rememberChoice)
        {
            SaveRememberChoice();
        }
    }

    private void PlayBackgroundMusic()
    {
        if (isMusicEnabled)
        {
            backgroundMusic.volume = volumeSlider.value;
            backgroundMusic.Play();
        }
    }

    private void StopBackgroundMusic()
    {
        backgroundMusic.Stop();
    }

    public void UpdateVolume()
    {
        if (isMusicEnabled)
        {
            backgroundMusic.volume = volumeSlider.value;
        }
    }

    private void LoadConfig()
    {
        string configPath = Path.Combine(Application.streamingAssetsPath, configFile);

        if (File.Exists(configPath))
        {
            string[] lines = File.ReadAllLines(configPath);

            foreach (string line in lines)
            {
                if (line.StartsWith("����� �������:"))
                {
                    serverAddress = line.Replace("����� �������:", "").Trim();
                }
                else if (line.StartsWith("����:"))
                {
                    int.TryParse(line.Replace("����:", "").Trim(), out serverPort);
                }
            }
        }
    }

    private void UpdateUI()
    {
        serverAddressText.text = serverAddress;
        serverPortText.text = serverPort.ToString();
    }

    private void LoadRememberChoice()
    {
        if (PlayerPrefs.HasKey(rememberChoiceKey))
        {
            rememberChoice = PlayerPrefs.GetInt(rememberChoiceKey) == 1;
        }

        rememberChoiceToggle.isOn = rememberChoice;
    }

    private void SaveRememberChoice()
    {
        PlayerPrefs.SetInt(rememberChoiceKey, rememberChoice ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ReturnToPreviousScene()
    {
        // ������������ ������� ������
        currentCanvas.SetActive(false);

        // ���������� ������ ������
        nextCanvas.SetActive(true);

        Debug.Log("Return button clicked");
    }
}
