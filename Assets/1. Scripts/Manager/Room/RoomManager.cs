using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // 룸 이름
    public InputField roomNameText;
    public TMP_Dropdown dropdown;
    // 룸 표시할 프리팹
    public GameObject roomPrefab;
    // 룸 프리팹의 부모 객체
    public Transform scrollContent;

    // 룸 목록 저장용 딕셔너리
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();


    private static RoomManager m_instance; // 싱글톤이 할당될 static 변수
    public static RoomManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<RoomManager>();
            }

            // 싱글톤 오브젝트를 반환
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

    public void CreateRoom()
    {
        roomName = roomNameText.text;
        int rand = Random.Range(0, int.MaxValue);

        if (roomName == string.Empty)
        {
            roomName = "RandRoom_" + rand.ToString();
        }

        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = byte.Parse(dropdown.captionText.text) }, null);
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
