//using Photon.Pun; // ����Ƽ�� ���� ������Ʈ��
//using Photon.Realtime; // ���� ���� ���� ���̺귯��
//using UnityEngine;
//using UnityEngine.UI;

//// ������(��ġ ����ŷ) ������ �� ������ ���
//public class LobbyManager : MonoBehaviourPunCallbacks
//{

//    private string gameVersion = "1"; // ���� ����

//    public Text connectionInfoText; // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
//    public Button joinButton; // �� ���� ��ư

//    // ���� ����� ���ÿ� ������ ���� ���� �õ�
//    private void Start()
//    {
//        // ���ӿ� �ʿ��� ����(���� ����) ����
//        PhotonNetwork.GameVersion = gameVersion;
//        Screen.SetResolution(800, 400, false);
//        // ������ ������ ������ ������ ���� ���� �õ�
//        PhotonNetwork.ConnectUsingSettings();

//        // �� ���� ��ư�� ��� ��Ȱ��ȭ
//        joinButton.interactable = false;
//        // ������ �õ� ������ �ؽ�Ʈ�� ǥ��
//        connectionInfoText.text = "Connection Status: Connect to Master";
//    }


//    // ������ ���� ���� ������ �ڵ� ����
//    public override void OnConnectedToMaster()
//    {
//        // �� ���� ��ư�� Ȱ��ȭ
//        joinButton.interactable = true;
//        // ���� ���� ǥ��
//        connectionInfoText.text = "Connected to Master - Online";
//    }


//    // ������ ���� ���� ���н� �ڵ� ����
//    public override void OnDisconnected(DisconnectCause cause)
//    {
//        // �� ���� ��ư�� ��Ȱ��ȭ
//        joinButton.interactable = false;
//        // ���� ���� ǥ��
//        connectionInfoText.text = "Not Connected to Master - OffLine\n Retry....";

//        // ������ �������� ������ �õ�
//        PhotonNetwork.ConnectUsingSettings();
//    }



//    // �� ���� �õ�
//    public void Connect()
//    {
//        // �ߺ� ���� �õ��� ���� ����, ���� ��ư ��� ��Ȱ��ȭ
//        joinButton.interactable = false;

//        // ������ ������ �������̶��
//        if (PhotonNetwork.IsConnected)
//        {
//            // �� ���� ����
//            connectionInfoText.text = "Connection Status : Connecting...";
//            PhotonNetwork.JoinRandomRoom();
//        }
//        else
//        {
//            // ������ ������ �������� �ƴ϶��, ������ ������ ���� �õ�
//            connectionInfoText.text = "Not Connected to Master - OffLine\n Retry....";
//            // ������ �������� ������ �õ�
//            PhotonNetwork.ConnectUsingSettings();
//        }
//    }


//    // (�� ���� ����)���� �� ������ ������ ��� �ڵ� ����
//    public override void OnJoinRandomFailed(short returnCode, string message)
//    {
//        // ���� ���� ǥ��
//        connectionInfoText.text = "No EMPTY Room, Let's Create Room...";
//        // �ִ� 4���� ���� ������ ����� ����
//        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
//    }


//    // �뿡 ���� �Ϸ�� ��� �ڵ� ����
//    public override void OnJoinedRoom()
//    {
//        // ���� ���� ǥ��
//        connectionInfoText.text = "Joined";
//        // ��� �� �����ڵ��� Main ���� �ε��ϰ� ��
//        PhotonNetwork.LoadLevel("Game");
//    }

//}

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

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Start()
    {
        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;
        Screen.SetResolution(1000, 800, false);
        PV = photonView;
    }

    public void Connect(InputField NickInput)
    {
        if (string.IsNullOrWhiteSpace(NickInput.text)) return;
        PhotonNetwork.LocalPlayer.NickName = NickInput.text;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        connectionInfoText.text = "Connected to Master - Online";
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom("Room2", new RoomOptions { MaxPlayers = 4 }, null);
    }

    public override void OnJoinedRoom()
    {
        // ���� ���� ǥ��
        connectionInfoText.text = "Joined";
        // ��� �� �����ڵ��� Main ���� �ε��ϰ� ��
        PhotonNetwork.LoadLevel("Game");
    }
}
