using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    
    private string gameVersion = "1"; // 게임 버전
    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼

    private string roomName = string.Empty;
    private int randRoomNum = 0;

    // 유저 닉네임
    public InputField userIdText;
    // 룸 이름
    public InputField roomNameText;

    // 룸 목록 저장용 딕셔너리
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // 룸 표시할 프리팹
    public GameObject roomPrefab;
    // 룸 프리팹의 부모 객체
    public Transform scrollContent;

    public CanvasGroup NickNameCanvas;
    public CanvasGroup RoomCanvas;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;
        PV = photonView;
    }

    public void JoinLobby()
    {
        if (string.IsNullOrWhiteSpace(userIdText.text)) return;
        PhotonNetwork.LocalPlayer.NickName = userIdText.text;
        PhotonNetwork.ConnectUsingSettings();

        NickNameCanvas.alpha = 0;
        NickNameCanvas.blocksRaycasts = false;
        NickNameCanvas.interactable = false;

        RoomCanvas.alpha = 1;
        RoomCanvas.blocksRaycasts = true;
        RoomCanvas.interactable = true;
    }

    public void Connect()
    {       
        this.roomName = roomNameText.text;

        if (roomName == string.Empty)
        {
            roomName = "RandRoom_" + randRoomNum.ToString();
            randRoomNum++;
        }

        PhotonNetwork.JoinOrCreateRoom(roomName, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        connectionInfoText.text = "Connected to Master - Online";
        LogManager.Log("Connected to Master - Online");
    }

    public override void OnJoinedLobby()
    {
        connectionInfoText.text = "Lobby Join Success - Online";
        LogManager.Log("Lobby Join Success - Online");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "Room Join Failed";
        LogManager.Log("Room Join Failed");
        if (roomName == string.Empty)
        {
            roomName = "RandRoom_" + randRoomNum.ToString();
            randRoomNum++;
        }
         PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnCreatedRoom()
    {
        connectionInfoText.text = "Room Create Success";
        LogManager.Log("Room Create Success");
    }

    public override void OnJoinedRoom()
    {
        // 접속 상태 표시
        connectionInfoText.text = "Room Joined";
        LogManager.Log("Room Joined");
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
        PhotonNetwork.LoadLevel("Game");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        GameObject tempRoom = null;

        foreach(var room in roomList)
        {
            if(room.RemovedFromList == true)
            {
                roomDict.TryGetValue(room.Name, out tempRoom);
                Destroy(tempRoom);
                roomDict.Remove(room.Name);
            }
            else
            {
                if(roomDict.ContainsKey(room.Name) == false)
                {
                    GameObject roomobj = Instantiate(roomPrefab, scrollContent);
                    roomobj.GetComponent<RoomManager>().RoomInfo = room;
                    roomDict.Add(room.Name, roomobj);
                }
                else
                {
                    roomDict.TryGetValue(room.Name, out tempRoom);
                    tempRoom.GetComponent<RoomManager>().RoomInfo = room;
                }
            }
        }
    }
}
