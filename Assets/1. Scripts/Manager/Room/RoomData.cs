using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RoomData : MonoBehaviour
{
    private TMP_Text RoomInfoText;
    private RoomInfo roomInfo;

    public InputField userIdText;

    public byte MaxPlayers = 4;

    public RoomInfo RoomInfo
    {
        get => roomInfo;
        set
        {
            roomInfo = value;
            RoomInfoText.text = $"{roomInfo.Name} ({roomInfo.PlayerCount}/{roomInfo.MaxPlayers})";
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => OnEnterRoom(roomInfo.Name));
        }
    }

    private void Awake()
    {
        RoomInfoText = GetComponentInChildren<TMP_Text>();
        userIdText = GameObject.Find("NickName_Input").GetComponent<InputField>();
    }

    private void OnEnterRoom(string roomName)
    {
        PhotonNetwork.NickName = userIdText.text;
        RoomManager.instance.RoomName = roomName;
        RoomManager.instance.JoinRoom();
    }
}
