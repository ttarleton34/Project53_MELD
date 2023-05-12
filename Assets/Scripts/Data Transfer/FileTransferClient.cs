using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class FileTransferClient : MonoBehaviour
{
    public Button webSocketButton;

    private void Start()
    {
        //webSocketButton.onClick.AddListener(OnWebSocketButtonClicked);
        Debug.Log("Starting RequestFile.");
        Task.Run(RequestFile);
    }

    private void OnWebSocketButtonClicked()
    {
        Debug.Log("Button clicked. Starting RequestFile.");
        Task.Run(RequestFile);
    }

    public async Task RequestFile()
    {
        Debug.Log("RequestFile started.");
        using (ClientWebSocket ws = new ClientWebSocket())
        {
            Uri serverUri = new Uri("ws://130.39.76.37:13000/");
            //Uri serverUri = new Uri("ws://localhost:13000/");

            Debug.Log("Attempting to connect.");
            await ws.ConnectAsync(serverUri, CancellationToken.None);
            Debug.Log("Connected.");

            ArraySegment<byte> receivedBytes = new ArraySegment<byte>(new byte[8192]);
            WebSocketReceiveResult result;
            string filename = "data.csv";
            string path = Path.Combine(Application.streamingAssetsPath, filename);
            Debug.Log(path);
            //string path = @"C:\Users\ttarl\Documents\GitHub\Project53_MELD\Assets\StreamingAssets\data.csv";

            while (ws.State == WebSocketState.Open)
            {
                result = await ws.ReceiveAsync(receivedBytes, CancellationToken.None);
                Debug.Log("Received response.");

                if (result.MessageType == WebSocketMessageType.Binary)
                {
                    byte[] receivedData = receivedBytes.Array.Take(result.Count).ToArray();
                    byte[] newEntries = receivedData.Skip(8).ToArray();

                    using (FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.Read))
                    {
                        fs.Write(newEntries, 0, newEntries.Length);
                        Debug.Log("Updated file with new entries.");
                    }
                }

                await Task.Delay(1000);
            }

            if (ws.State != WebSocketState.Closed && ws.State != WebSocketState.Aborted)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing connection", CancellationToken.None);
                Debug.Log("Closed connection.");
            }
        }
    }
}