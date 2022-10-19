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
    // �� �̸�
    public InputField roomNameText;
    public TMP_Dropdown dropdown;
    // �� ǥ���� ������
    public GameObject roomPrefab;
    // �� �������� �θ� ��ü
    public Transform scrollContent;

    // �� ��� ����� ��ųʸ�
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    private static RoomManager m_instance = null; // �̱����� �Ҵ�� static ����
    public static RoomManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<RoomManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

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
            // ���Ӱ� ������� �̱���
            m_instance = this;
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
