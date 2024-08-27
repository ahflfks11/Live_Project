using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackndChat;

public class ChatManager : MonoBehaviour, BackndChat.IChatClientListener
{
    private BackndChat.ChatClient _ChatClient = null;
    MessageInfo _msgInfo;
    ChannelInfo _channelInfo;
    // Start is called before the first frame update
    void Start()
    {
        _ChatClient = new ChatClient(this, new ChatClientArguments
        {
            Avatar = "default"
        });

        _msgInfo = new MessageInfo();
    }

    // Update is called once per frame
    void Update()
    {
        _ChatClient?.Update();
        if (Input.GetKeyDown(KeyCode.A))
        {
            Test();
        }
    }

    public void Test()
    {
        SendMessages("테스트중");
    }

    public void SendMessages(string _msg)
    {
        _ChatClient.SendChatMessage(_channelInfo.ChannelGroup, _channelInfo.ChannelName, _channelInfo.ChannelNumber, _msg);
    }

    public void OnChatMessage(MessageInfo messageInfo)
    {
        Debug.Log(messageInfo.GamerName + " : " + messageInfo.Message);
    }

    public void OnDeleteMessage(MessageInfo messageInfo)
    {
    }

    public void OnError(ERROR_MESSAGE error, object param)
    {
        switch (error)
        {
            default:
                break;
        }
    }

    public void OnHideMessage(MessageInfo messageInfo)
    {
    }

    public void OnJoinChannel(ChannelInfo channelInfo)
    {
        _channelInfo = channelInfo;
        for (int i = 0; i < _channelInfo.Messages.Count; i++)
        {
            Debug.Log(_channelInfo.Messages[i].GamerName + " : " + _channelInfo.Messages[i].Message);
        }
    }

    public void OnJoinChannelPlayer(string channelGroup, string channelName, ulong channelNumber, string gamerName, string avatar)
    {
    }

    public void OnLeaveChannel(ChannelInfo channelInfo)
    {
    }

    public void OnLeaveChannelPlayer(string channelGroup, string channelName, ulong channelNumber, string gamerName, string avatar)
    {
    }

    public void OnSuccess(SUCCESS_MESSAGE success, object param)
    {
        switch (success)
        {
            default:
                break;
        }
    }

    public void OnTranslateMessage(List<MessageInfo> messages)
    {
        foreach(MessageInfo _info in messages)
        {
            Debug.Log(_info.GamerName + " : " + _info.Message);
        }
    }

    public void OnWhisperMessage(WhisperMessageInfo messageInfo)
    {
    }

    private void OnApplicationQuit()
    {
        _ChatClient?.SendLeaveChannel(_channelInfo.ChannelGroup, _channelInfo.ChannelName, _channelInfo.ChannelNumber);
        _ChatClient?.Dispose();
    }
}
