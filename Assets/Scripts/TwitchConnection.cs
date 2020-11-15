using TMPro;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchConnection : MonoBehaviour
{
    [SerializeField] 
    private string _channelToConnectTo = "hellyeahplay";
    [SerializeField] private TMP_InputField debug;
    private Client _client;

    private void Start()
    {
        // To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
        // Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
        // This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
        Application.runInBackground = true;

        //Create Credentials instance
        ConnectionCredentials credentials = new ConnectionCredentials(Secrets.BOT_NAME, Secrets.BOT_ACCESS_TOKEN);
        _client = new Client();
        
        // Initialize the client with the credentials instance, and setting a default channel to connect to.
        _client.Initialize(credentials, _channelToConnectTo);

        // Bind callbacks to events
        _client.OnConnected += OnConnected;
        _client.OnJoinedChannel += OnJoinedChannel;
        _client.OnMessageReceived += OnMessageReceived;
        _client.OnChatCommandReceived += OnChatCommandReceived;
        _client.OnGiftedSubscription += OnGiftedSubscription;
        _client.OnMessageSent += OnMessageSent;

        // Connect
        _client.Connect();
    }

    private void OnMessageSent(object sender, OnMessageSentArgs e)
    {
        Debug.Log(e.SentMessage.Message);
    }

    private void OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
    {
        Debug.Log(
            $"Gift subscription recieve {e.GiftedSubscription.MsgParamRecipientUserName} from {e.GiftedSubscription.Id}");

        debug.text +=
            $"{e.GiftedSubscription.Id}" +
            $" / {e.GiftedSubscription.Login} " +
            $"/ {e.GiftedSubscription.DisplayName} " +
            $"/ {e.GiftedSubscription.MsgParamRecipientId} " +
            $"/ {e.GiftedSubscription.MsgParamRecipientUserName} " +
            $"/ {e.GiftedSubscription.MsgParamRecipientDisplayName} " +
            $"/ {e.GiftedSubscription.MsgParamSubPlan}";
       }

    private void OnConnected(object sender, OnConnectedArgs e)
    {
        Debug.Log($"The bot {e.BotUsername} succesfully connected to Twitch.");
        debug.text += $"The bot {e.BotUsername} succesfully connected to Twitch.";

        if (string.IsNullOrWhiteSpace(e.AutoJoinChannel)) return;

        Debug.Log(
            $"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        debug.text += $"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}";
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        debug.text += $"The bot created by @uurh just joined the channel: {e.Channel}";
        Debug.Log($"The bot created by @uurh just joined the channel: {e.Channel}");
        //_client.SendMessage(e.Channel, $"The bot created by @uurh just joined the channel: {e.Channel}");
    }

    private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
    {
        //Debug.Log($"{e.ChatMessage.RawIrcMessage}");
    }

    private void OnChatCommandReceived(object sender, OnChatCommandReceivedArgs e)
    {
        Debug.Log($"{e.Command.CommandText}");
        // switch (e.Command.CommandText)
        // {
        // 	case "hello":
        // 		_client.SendMessage(e.Command.ChatMessage.Channel, $"Hello {e.Command.ChatMessage.DisplayName}!");
        // 		break;
        // 	case "about":
        // 		_client.SendMessage(e.Command.ChatMessage.Channel, "I am a Twitch bot running on TwitchLib!");
        // 		break;
        // 	default:
        // 		_client.SendMessage(e.Command.ChatMessage.Channel, $"Unknown chat command: {e.Command.CommandIdentifier}{e.Command.CommandText}");
        // 		break;
        // }
    }

    private void Update()
    {
        // Don't call the client send message on every Update,
        // this is sample on how to call the client,
        // not an example on how to code.
        if (Input.GetKeyDown(KeyCode.Space))
        {
           // _client.SendRaw(":uurha!<uurha>@<uurha>.tmi.twitch.tv PRIVMSG #hellyeahplay :/vips");
        }
    }
}