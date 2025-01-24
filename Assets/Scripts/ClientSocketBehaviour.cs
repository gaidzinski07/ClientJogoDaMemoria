using System;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MessageTypeEnum
{
    private MessageTypeEnum(string value)
    {
        Value = value;
    }

    public string Value { get; private set; }

    public static MessageTypeEnum PLAY { get { return new MessageTypeEnum("/play"); } }
    public static MessageTypeEnum CHAT { get { return new MessageTypeEnum("/chat"); } }
    public static MessageTypeEnum PLACAR { get { return new MessageTypeEnum("/placar"); } }
    public static MessageTypeEnum RESULTADO { get { return new MessageTypeEnum("/resultado"); } }
    public static MessageTypeEnum NONE { get { return new MessageTypeEnum(""); } }
}

public class ClientSocketBehaviour : MonoBehaviour
{
    private TcpClient client;
    private NetworkStream stream;
    private Thread receiveThread;

    // Configuração de conexão
    public string serverAddress = "127.0.0.1"; // Endereço IP do servidor
    public int serverPort = 9876;             // Porta do servidor

    public void ConnectToServer()
    {
        try
        {
            client = new TcpClient(serverAddress, serverPort);
            stream = client.GetStream();

            // Inicializa uma nova thread para receber dados
            receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.IsBackground = true;
            receiveThread.Start();

            Debug.Log("Conectado ao servidor. Enviando nickname...");
            SendData(ChatManager.nickname, MessageTypeEnum.NONE);
        }
        catch (Exception e)
        {
            Debug.LogError("Erro ao conectar ao servidor: " + e.Message);
        }
    }

    // Método para enviar dados ao servidor
    public void SendData(string message, MessageTypeEnum messageType)
    {
        if (stream != null)
        {
            message += "\n";
            try
            {
                message = messageType.Value + message;
                byte[] data = Encoding.UTF8.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Debug.Log("Mensagem enviada: " + message);
            }
            catch (Exception e)
            {
                Debug.LogError("Erro ao enviar mensagem: " + e.Message);
            }
        }
    }

    // Método para receber dados do servidor
    private void ReceiveData()
    {
        while (client != null && client.Connected)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            if (bytesRead > 0)
            {
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                if (response.StartsWith(MessageTypeEnum.CHAT.Value))
                {
                    response = response.Remove(0, MessageTypeEnum.CHAT.Value.Length);
                    Debug.Log("Mensagem recebida:\n" + response);
                    ChatManager.Instance.ReceiveMessage(response);
                }else if (response.StartsWith(MessageTypeEnum.PLAY.Value))
                {
                    response = response.Remove(0, MessageTypeEnum.PLAY.Value.Length);
                    GameManager.Instance.ultimaJogada = response;
                }else if (response.StartsWith(MessageTypeEnum.PLACAR.Value))
                {
                    response = response.Remove(0, MessageTypeEnum.PLACAR.Value.Length);
                    Debug.Log(response);
                    ChatManager.Instance.ReceivePlacar(response);
                }else if (response.StartsWith(MessageTypeEnum.RESULTADO.Value))
                {
                    response = response.Remove(0, MessageTypeEnum.RESULTADO.Value.Length);
                    Debug.Log(response);
                    GameManager.resultado = response;
                }
            }
        }
    }

    // Método para desconectar do servidor
    public void Disconnect()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }

        if (stream != null)
        {
            stream.Close();
        }

        if (client != null)
        {
            client.Close();
            client = null;
        }

        Debug.Log("Desconectado do servidor.");
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }
}
