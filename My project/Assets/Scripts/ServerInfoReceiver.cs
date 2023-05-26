using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;
using TMPro;

[Serializable]
public class OdometerResponse
{
    public string operation;
    public float odometer;
}

public class ServerInfoReceiver : MonoBehaviour
{
    private WebSocket socket;
    private float targetOdometerValue = 0;
    private string serverAddress;
    private int serverPort;
    private string odometerValueString = "";

    public TextMeshProUGUI odometerText;
    public Image lampImage;
    public Sprite connectedSprite;
    public Sprite disconnectedSprite;
    public float transitionDuration = 0.5f;

    private bool isTransitioning = false;

    private void Start()
    {
       
        LoadConfig();
        ConnectToServer();
        InvokeRepeating(nameof(SendGetCurrentOdometerRequest), 0f, 10f);
    }

    private void OnDestroy()
    {
        DisconnectFromServer();
        CancelInvoke(nameof(SendGetCurrentOdometerRequest));
    }

    private void LoadConfig()
    {
        string configFilePath = Path.Combine(Application.streamingAssetsPath, "config.txt");

        if (File.Exists(configFilePath))
        {
            string[] configLines = File.ReadAllLines(configFilePath);
            foreach (string line in configLines)
            {
                if (line.StartsWith("Адрес сервера:"))
                {
                    serverAddress = line.Replace("Адрес сервера:", "").Trim();
                }
                else if (line.StartsWith("Порт:"))
                {
                    int.TryParse(line.Replace("Порт:", "").Trim(), out serverPort);
                }
            }
        }
        else
        {
            Debug.LogError("Config file not found: " + configFilePath);
        }
    }

    private void ConnectToServer()
    {
        string serverUrl = "ws://" + serverAddress + ":" + serverPort + "/ws";
        socket = new WebSocket(serverUrl);

        socket.OnOpen += OnSocketOpen;
        socket.OnMessage += OnSocketMessage;
        socket.OnError += OnSocketError;
        socket.OnClose += OnSocketClose;

        socket.Connect();
    }

    private void DisconnectFromServer()
    {
        if (socket != null && socket.IsAlive)
            socket.Close();
    }

    private void OnSocketOpen(object sender, EventArgs e)
    {
        Debug.Log("Connected to server");
        StartLampTransition(connectedSprite);
    }

    private void OnSocketMessage(object sender, MessageEventArgs e)
    {
        string message = e.Data;
        Debug.Log("Received message from server: " + message);
        ProcessMessage(message);
    }

    private void OnSocketError(object sender, WebSocketSharp.ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Exception);
    }

    private void OnSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("Disconnected from server");
        StartLampTransition(disconnectedSprite);
    }

    private void ProcessMessage(string message)
    {
        
        if (message.Contains("currentOdometer"))
        {
            OdometerResponse response = JsonUtility.FromJson<OdometerResponse>(message);
            targetOdometerValue = response.odometer;
            odometerValueString = targetOdometerValue.ToString("0.00");
            Debug.Log("Current Odometer Value: " + targetOdometerValue);
        }
    }

    private void SendGetCurrentOdometerRequest()
    {
        if (socket != null && socket.IsAlive)
        {
            string request = "{\"operation\": \"getCurrentOdometer\"}";
            socket.Send(request);
        }
    }

    private void StartLampTransition(Sprite targetSprite)
    {
        if (isTransitioning)
            return;

        StartCoroutine(LampTransitionCoroutine(targetSprite));
    }

    private System.Collections.IEnumerator LampTransitionCoroutine(Sprite targetSprite)
    {
        isTransitioning = true;

        float elapsedTime = 0;
        Color startColor = lampImage.color;
        Color targetColor = targetSprite == connectedSprite ? Color.green : Color.red;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            lampImage.color = Color.Lerp(startColor, targetColor, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        lampImage.sprite = targetSprite;
        lampImage.color = targetColor;
        isTransitioning = false;
    }

    private void Update()
    {
        odometerText.text = odometerValueString;
    }
}
