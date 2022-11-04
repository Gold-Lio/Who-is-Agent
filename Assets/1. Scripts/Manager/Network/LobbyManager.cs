using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // ����Ƽ�� ���� ������Ʈ��
using Photon.Realtime; // ���� ���� ���� ���̺귯��
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ������(��ġ ����ŷ) ������ �� ������ ���
public class LobbyManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    
    private string gameVersion = "1"; // ���� ����
    public Text connectionInfoText; // ��Ʈ��ũ ������ ǥ���� �ؽ�Ʈ
    public Button joinButton; // �� ���� ��ư

    // ���� �г���
    public InputField userIdText;

    public CanvasGroup NickNameCanvas;
    public CanvasGroup LobbyCanvas;
    public CanvasGroup CreateCanvas;

    // ���� ����� ���ÿ� ������ ���� ���� �õ�
    private void Start()
    {
        PV = photonView;

        // ���ӿ� �ʿ��� ����(���� ����) ����
        PhotonNetwork.GameVersion = gameVersion;

        Screen.SetResolution(1920, 1080,true); // �׻� FullScreen �Դϴ�.
        
        PhotonNetwork.SendRate = 60; // �������� �ְ� �޴� ��Ű���
        PhotonNetwork.SerializationRate = 30; // �����͸� �޾ƾ��� ��

        SwitchCanvas(CanvasType.Nick);


    }

    private void SwitchCanvas(CanvasType type)
    {
        if(type == CanvasType.Nick)
        {
            CanvasOpen(NickNameCanvas);
            CanvasClose(LobbyCanvas);
            CanvasClose(CreateCanvas);
        }
        else if (type == CanvasType.Lobby)
        {
            CanvasOpen(LobbyCanvas);
            CanvasClose(NickNameCanvas);
            CanvasClose(CreateCanvas);
        }
        else if (type == CanvasType.Create)
        {
            CanvasOpen(CreateCanvas);
            CanvasClose(LobbyCanvas);
            CanvasClose(NickNameCanvas);
        }
    }

    private void CanvasOpen(CanvasGroup group)
    {
        group.alpha = 1;
        group.blocksRaycasts = true;
        group.interactable = true;
    }

    private void CanvasClose(CanvasGroup group)
    {
        group.alpha = 0;
        group.blocksRaycasts = false;
        group.interactable = false;
    }

    public void JoinLobby()
    {
        if (string.IsNullOrWhiteSpace(userIdText.text)) return;
        PhotonNetwork.LocalPlayer.NickName = userIdText.text;
        PhotonNetwork.ConnectUsingSettings();

        SwitchCanvas(CanvasType.Lobby);
    }

    public void CreateRoom()
    {
        RoomManager.instance.CreateRoom(false);
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
        RoomManager.instance.CreateRoom(true);
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
    public void OnRoomCreateBtnClick()
    {
        SwitchCanvas(CanvasType.Create);
    }

    public void OnRandomJoinBtnClick()
    {
        PhotonNetwork.NickName = userIdText.text;
        RoomManager.instance.RandomJoinRoom();
    }

    public void OnCancel()
    {
        if(LobbyCanvas.alpha == 1)
        {
            SwitchCanvas(CanvasType.Nick);
        }
        else if(CreateCanvas.alpha == 1)
        {
            SwitchCanvas(CanvasType.Lobby);
        }
    }

    //�ٽ� ó������ ���ư�����. 
    //���α׷��� �����ŵ�ϴ�.
    public void ProgramQuit()
    {
        Application.Quit();
    }
}
