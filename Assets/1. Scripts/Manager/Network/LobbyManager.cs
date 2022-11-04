using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using Photon.Realtime; // 포톤 서비스 관련 라이브러리
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 마스터(매치 메이킹) 서버와 룸 접속을 담당
public class LobbyManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    
    private string gameVersion = "1"; // 게임 버전
    public Text connectionInfoText; // 네트워크 정보를 표시할 텍스트
    public Button joinButton; // 룸 접속 버튼

    // 유저 닉네임
    public InputField userIdText;

    public CanvasGroup NickNameCanvas;
    public CanvasGroup LobbyCanvas;
    public CanvasGroup CreateCanvas;

    // 게임 실행과 동시에 마스터 서버 접속 시도
    private void Start()
    {
        PV = photonView;

        // 접속에 필요한 정보(게임 버전) 설정
        PhotonNetwork.GameVersion = gameVersion;

        Screen.SetResolution(1920, 1080,true); // 항상 FullScreen 입니다.
        
        PhotonNetwork.SendRate = 60; // 포톤으로 주고 받는 통신간격
        PhotonNetwork.SerializationRate = 30; // 데이터를 받아쓰는 빈도

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
        // 접속 상태 표시
        connectionInfoText.text = "Room Joined";
        LogManager.Log("Room Joined");
        // 모든 룸 참가자들이 Main 씬을 로드하게 함
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

    //다시 처음으로 돌아가도록. 
    //프로그램을 종료시킵니다.
    public void ProgramQuit()
    {
        Application.Quit();
    }
}
