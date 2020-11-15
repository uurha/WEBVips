using System;
using TMPro;
using TwitchLib.Client;
using TwitchLib.Client.Events;
using TwitchLib.Client.Models;
using TwitchLib.Unity;
using UnityEngine;

public class TwitchConnection : MonoBehaviour
{
    [SerializeField] private string _channelToConnectTo = "hellyeahplay";
    [SerializeField] private TMP_InputField debug = default;
    [SerializeField] private string debugLines = default;
    private TwitchClient _client = default;

    private void Start()
    {
        try
        {
            // To keep the Unity application active in the background, you can enable "Run In Background" in the player settings:
            // Unity Editor --> Edit --> Project Settings --> Player --> Resolution and Presentation --> Resolution --> Run In Background
            // This option seems to be enabled by default in more recent versions of Unity. An aditional, less recommended option is to set it in code:
            Application.runInBackground = true;

            //Create Credentials instance
            ConnectionCredentials credentials = new ConnectionCredentials(Secrets.BOT_NAME, Secrets.BOT_ACCESS_TOKEN);
            _client = new TwitchClient();
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
        catch (Exception e)
        {
            AddDebugLine(e.ToString());

            throw;
        }
    }

    private void OnMessageSent(object sender, OnMessageSentArgs e)
    {
        Debug.Log(e.SentMessage.Message);
    }

    private void OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
    {
        AddDebugLine(
            $"Gift subscription received {e.GiftedSubscription.MsgParamRecipientUserName} from {e.GiftedSubscription.Id}");

        AddDebugLine($"{e.GiftedSubscription.Id} " +
                     $"/ {e.GiftedSubscription.Login} " +
                     $"/ {e.GiftedSubscription.DisplayName} " +
                     $"/ {e.GiftedSubscription.MsgParamRecipientId} " +
                     $"/ {e.GiftedSubscription.MsgParamRecipientUserName} " +
                     $"/ {e.GiftedSubscription.MsgParamRecipientDisplayName} " +
                     $"/ {e.GiftedSubscription.MsgParamSubPlan}");
    }

    private void OnConnected(object sender, OnConnectedArgs e)
    {
        try
        {
            AddDebugLine($"The bot {e.BotUsername} successfully connected to Twitch.");

            if (string.IsNullOrWhiteSpace(e.AutoJoinChannel))
            {
                AddDebugLine(
                    $"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
            }

            _client.JoinChannel(_channelToConnectTo);
        }
        catch (Exception exception)
        {
            AddDebugLine(exception.ToString());

            throw;
        }
    }

    private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
    {
        AddDebugLine($"The bot created by @uurh just joined the channel: {e.Channel}");
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

    private void AddDebugLine(string line)
    {
        var bufferLine = string.Empty;

        if (!string.IsNullOrWhiteSpace(debugLines))
            bufferLine += "\n";

        bufferLine += $"[{DateTime.Now}] {line}";

        debugLines += bufferLine;

        Debug.Log(bufferLine);
    }

    private void Update()
    {
        lock (debugLines)
        {
            debug.text = debugLines;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _client.SendMessage(_channelToConnectTo, "hi");
        }
    }
}