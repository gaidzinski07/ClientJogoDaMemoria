using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField chatInput;
    [SerializeField]
    private TMP_InputField nicknameInput;
    [SerializeField]
    private TextMeshProUGUI txtChat;
    [SerializeField]
    private TextMeshProUGUI txtPlacar;
    private string chatContent = "";
    private string placar = "";
    public static ChatManager Instance;
    public static string nickname = "";

    private void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }

    public void SetNickname()
    {
        nickname = nicknameInput.text;
    }

    private void Update()
    {
        txtChat.text = chatContent;
        txtPlacar.text = placar;
    }

    public void SendMessage()
    {
        if (chatInput == null) return;
        var client = FindAnyObjectByType<ClientSocketBehaviour>();
        client.SendData(chatInput.text, MessageTypeEnum.CHAT);
    }

    public void ReceiveMessage(string message)
    {
        chatContent += "\n" + message;
    }

    public void ReceivePlacar(string placar)
    {
        this.placar = placar;
    }
}
