using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UIManager;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngineInternal;
using System.Runtime.InteropServices;

public enum PlayerType
{
    SPY,
}
public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public static NetworkManager NM;

    public static NetworkManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<NetworkManager>();
                //m_instance.Initialize();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static NetworkManager m_instance; // 싱글톤이 할당될 static 변수


    public void Awake() => NM = this;

    public List<Player> Players = new List<Player>();
    public Player MyPlayer;

    public PlayerType playerType;

    public bool isWaitingRoom = false;
    public bool isGameStart = false;
    public bool isResiWin;

    public Transform spawnPoint;


    public float baseTime;
    private float selectCountdown;

    public Text timeText;

    [Header("Job-Information")]
    //직업 정보 패널
    public GameObject infoPanel;
    
    [Header("InGamePanel")]
    public GameObject gamePanel;
    //직업정보 제공 Text
    public GameObject Resi_InfoText, Spy_InfoText;
    [Header("Win Panel")]
    //승리 패널
    public GameObject Resi_WinPanel, SPY_WinPanel;

    /// <summary>
    ////MAP
    /// </summary>
    //public GameObject WaitingMap, MainMap;

    //- - 씬초기화ㅡ 

    private void Start()
    {
        PV = photonView;

        MyPlayer = PhotonNetwork.Instantiate("Player", new Vector2(Random.Range(-12f, -8f), Random.Range(-38f, -41f)), Quaternion.identity).GetComponent<Player>();
        SetRandColor();
        isWaitingRoom = true;
    }

    //// 생성할 랜덤 위치 지정
    //Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
    //// 위치 y값은 0으로 변경
    //randomSpawnPos.y = 0f;

    public void SetRandColor()
    {
        List<int> PlayerColors = new List<int>();
        for (int i = 0; i < Players.Count; i++)
            PlayerColors.Add(Players[i].colorIndex);

        while (true)
        {
            int rand = Random.Range(0, 5);
            if (!PlayerColors.Contains(rand))
            {
                MyPlayer.GetComponent<PhotonView>().RPC("SetColor", RpcTarget.AllBuffered, rand);
                break;
            }
        }
    }

    public void SortPlayers() => Players.Sort((p1, p2) => p1.actor.CompareTo(p2.actor));

    public Color GetColor(int colorIndex)
    {
        return UM.colors[colorIndex];
    }

    //패널을 관리하는 ShowPanel
    public void ShowPanel(GameObject curPanel)
    {
        infoPanel.SetActive(false);
        gamePanel.SetActive(false);

        Resi_WinPanel.SetActive(false);
        SPY_WinPanel.SetActive(false);

        curPanel.SetActive(true);
    }

    //맵을 관리하는 ShowMap
    public void ShowMap(GameObject curMap)
    {
        //WaitingMap.SetActive(false);
        //MainMap.SetActive(false);

        curMap.SetActive(true);
    }

    public void GameStart()
    {
        // 방장이 게임시작
        SetSPY();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //왜 런타임 NULL?? 스타트가 안되는것....
        PV.RPC("GameStartRPC", RpcTarget.AllViaServer);
    }


    void SetSPY()
    {
        List<Player> GachaList = new List<Player>(Players);

        if (playerType == PlayerType.SPY)
        {
            for (int i = 0; i < 1; i++) //  스파이 1명
            {
                int rand = Random.Range(0, GachaList.Count); // 플레이어 에서 끌어와 랜덤
                Players[rand].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
                GachaList.RemoveAt(rand);
            }
        }
        //나중에 스파이 1명 더 추가 ㅡ 플레이어들이 자유롭게 선택할 수 있도록 조정
    }

    //------------------------------------------------------------------------------------------------------------------
    [PunRPC]
    void GameStartRPC()
    {
        StartCoroutine(GameStartCo());
    }


    IEnumerator GameStartCo()
    {
        ShowPanel(infoPanel);
        //ShowMap(MainMap);
        //WaitingMap.SetActive(false);

        if (MyPlayer.isSpy)
        {
            Spy_InfoText.SetActive(true);
        }
        else
        {
            Resi_InfoText.SetActive(true);
        }

        yield return new WaitForSeconds(3);
        isWaitingRoom = false;
        isGameStart = true; //게임 시작

        MyPlayer.SetPos(spawnPoint.position);

        MyPlayer.SetNickColor(); //스파이 닉네임 색깔 조정

        ShowPanel(gamePanel); //게임패널안에 들어있는 모든 HUD를 연다. 

        //여기 안에 나의 인벤토리나   해당 직업에 맞게 미션 목적의 프리팹또한 이곳에 들어가야한다. 
        //StartCoroutine(LightCheckCo());  //빛 조절

        selectCountdown = baseTime;
    }

    private void Update()
    {
        PlayTime();
    }

    //총 플레이 타임은 600sec = 10min 이다. 
    public void PlayTime()
    {
        if (Mathf.Floor(selectCountdown) <= 0)
        {
            Winner(false);
            // Count 0일때 동작할 함수 삽입
        }
        else
        {
            selectCountdown -= Time.deltaTime;
            timeText.text = Mathf.Floor(selectCountdown).ToString();
        }
    }


    public int GetCrewCount()
    {
        int crewCount = 0;
        for (int i = 0; i < Players.Count; i++)
            if (!Players[i].isSpy) ++crewCount;
        return crewCount;
    }

    [PunRPC]
    public void WinCheck()
    {
        int crewCount = 0;
        int impoCount = 0;

        for (int i = 0; i < Players.Count; i++)
        {
            var Player = Players[i];
            if (Players[i].isDie) continue;
            if (Player.isSpy)
                ++impoCount;
            else
                ++crewCount;
        }

        if (impoCount == 0 && crewCount > 0) // 모든 임포가 죽음
            Winner(true);
        else if (impoCount != 0 && impoCount > crewCount) // 임포가 크루보다 많음
            Winner(false);
    }

    public void Winner(bool isCrewWin)
    {
        if (!isGameStart) return;

        if (isCrewWin)
        {
            print("레지스탕스 승리");
            ShowPanel(Resi_WinPanel);
            Invoke("WinnerDelay", 3);
        }
        else
        {
            print("스파이 승리");
            ShowPanel(SPY_WinPanel);
            Invoke("WinnerDelay", 3);
        }
    }


    void WinnerDelay()
    {
        Application.Quit();
    }
}





