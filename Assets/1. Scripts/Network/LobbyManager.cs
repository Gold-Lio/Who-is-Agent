using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // ����Ƽ�� ���� ������Ʈ��
using Photon.Realtime; // ���� ���� ���� ���̺귯��
using UnityEngine;
using UnityEngine.UI;

// ������(��ġ ����ŷ) ������ �� ������ ���
public class LobbyManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    
    private string gameVersion = "1"; // ���� ����
    public Text connectionInfoText; // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinButton; // �� ���� ��ư

    private string roomName = string.Empty;
    private int randRoomNum = 0;

    // ���� �г���
    public InputField userIdText;
    // �� �̸�
    public InputField roomNameText;

    // �� ��� ����� ��ųʸ�
    private Dictionary<string, GameObject> roomDict = new Dictionary<string, GameObject>();
    // �� ǥ���� ������
    public GameObject roomPrefab;
    // �� �������� �θ� ��ü
    public Transform scrollContent;

    public CanvasGroup NickNameCanvas;
    public CanvasGroup RoomCanvas;

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Start()
    {
        // ���ӿ� �ʿ��� ����(���� ����) ����
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
        // ���� ���� ǥ��
        connectionInfoText.text = "Room Joined";
        LogManager.Log("Room Joined");
        // ��� �� �����ڵ��� Main ���� �ε��ϰ� ��
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
