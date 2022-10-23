using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // ·ë ÀÌ¸§
    public InputField roomNameText;
    public TMP_Dropdown dropdown;
    // ·ë Ç¥½ÃÇÒ ÇÁ¸®ÆÕ
    public GameObject roomPrefab;
    // ·ë ÇÁ¸®ÆÕÀÇ ºÎ¸ð °´Ã¼
    public Transform scrollContent;

    // ·ë ¸ñ·Ï ÀúÀå¿ë µñ¼Å³Ê¸®
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    private static RoomManager m_instance = null; // ½Ì±ÛÅæÀÌ ÇÒ´çµÉ static º¯¼ö
    public static RoomManager instance => m_instance;

    private RoomInfo roomInfo;
    public RoomInfo RoomInfo
    {
        get 
        { 
            return roomInfo; 
        }
        set 
        { 
            roomInfo = value; 
        }
    }

    private string roomName = string.Empty;
    private byte pNum = 0;
    public byte PNum => pNum;

    private void Awake()
    {
        if (m_instance == null)
        {
            // »õ·Ó°Ô ¸¸µé¾îÁø ½Ì±ÛÅæ
            m_instance = this;
            //m_instance = FindObjectOfType<RoomManager>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (m_instance != this)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void CreateRoom(bool israndom)
    {
        roomName = roomNameText.text;
        int roomrand = UnityEngine.Random.Range(0, int.MaxValue);

        if (roomName == string.Empty)
        {
            roomName = "RandRoom_" + roomrand.ToString();
        }

        if (israndom)
        {
            int pnumrand = UnityEngine.Random.Range(4, 7);
            pNum = Convert.ToByte(pnumrand);
        }
        else
        {
            pNum = byte.Parse(dropdown.captionText.text);
        }
        //PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = pNum}, null);
        PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = pNum, CleanupCacheOnLeave = false}, null);
    }

    public void JoinRoom()
    {
        if (roomInfo == null) return;
        PhotonNetwork.JoinRoom(roomInfo.Name);
    }

    public void RandomJoinRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;

        foreach (var room in roomList)
        {
            if (room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            else
            {
                if (roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject roomobj = Instantiate(roomPrefab, scrollContent);
                    roomobj.GetComponent<RoomData>().RoomInfo = room;
                    roomDict.Add(room.Name, roomobj);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomData>().RoomInfo = room;
                }
            }
        }
    }
}
