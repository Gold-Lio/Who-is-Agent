using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UIManager;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;
using UnityEngine.Experimental.Rendering.Universal;

public enum PlayerType
{
    Master_Spy,
    Random_1,
    Random_2,
}

public enum AttackTest
{ 
    Can_Attack,
    Cant_Attack
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
    public AttackTest attackTest;


    public bool isWaitingRoom = false;
    public bool isGameStart = false;
    public bool isAzit = false;
    public bool isWinner = false;
    public bool isResiWin;

    public Transform spawnPoint;

    [SerializeField]
    private float baseTime = 600.0f;
    private float selectCountdown;

    [Header("About-Time")]
    public GameObject timeObj;
    public Text timeText;

    [Header("Job-Information")]
    //직업 정보 패널
    public GameObject infoPanel;

    [Header("ShowText")]
    public GameObject showText;

    [Header("InGamePanel")]
    public GameObject gamePanel;
    //직업정보 제공 Text
    public GameObject Resi_InfoText, Spy_InfoText;

    [Header("Result Panel")]
    //결과 패널
    public GameObject SPY_WinPanel, Resi_WinPanel;

    [Header("Did Panel")]
    public GameObject YouDie;

    public Light2D light2D;
    //- - 씬초기화ㅡ 

    private int count = 0;

    private void Start()
    {
        PV = photonView;

        MyPlayer = PhotonNetwork.Instantiate("Player", new Vector2(Random.Range(-12f, -8f), Random.Range(-38f, -41f)),
            Quaternion.identity).GetComponent<Player>();

        count++;

        SetRandColor();
        isWaitingRoom = true;
        selectCountdown = baseTime;

        if(isWaitingRoom)
        {
            showText.SetActive(true);
        }
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
            int rand = Random.Range(0, 8);
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

  
    public void GameStart()
    {
        // 방장이 게임시작

        //잠시 테스트를 위해 주석처리
        // 지정 인원수 아래이면 게임 시작 불가능하도록 설정
        //if (PhotonNetwork.PlayerList.Length < RoomManager.instance.PNum) return;

        //잠시 테스트를 위해 주석처리
        SetPlayerType();

        SetSPY();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //왜 런타임 NULL?? 스타트가 안되는것....
        PV.RPC("GameStartRPC", RpcTarget.AllViaServer);
        selectCountdown = baseTime;
    }

    void SetAttackPlayer()
    {

    }

    //잠시 테스트를 위해 주석처리
    void SetPlayerType()
    {
        int maxplayer = System.Convert.ToInt32(RoomManager.instance.PNum);
        if (maxplayer <= 5)
        {
            playerType = PlayerType.Random_1;
        }
        else if (maxplayer >= 6)
        {
            playerType = PlayerType.Random_2;
        }
    }

    void SetSPY()
    {
        List<Player> GachaList = new List<Player>(Players);

        //방장만 SPY
        if (playerType == PlayerType.Master_Spy)
        {
            Players[0].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
        }

        //랜덤_스파이 1명 
        else if (playerType == PlayerType.Random_1)
        {
            for (int i = 0; i < 1; i++)
            {
                int rand = Random.Range(0, GachaList.Count); // 랜덤
                Players[rand].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
                GachaList.RemoveAt(rand);
            }
        }

        //랜덤_스파이 2명 
        else if (playerType == PlayerType.Random_2)
        {
            for (int i = 0; i < 2; i++)
            {
                int rand = Random.Range(0, GachaList.Count); // 랜덤
                Players[rand].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
                GachaList.RemoveAt(rand);
            }
        }
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

        if (MyPlayer.isSpy)
        {
            Spy_InfoText.SetActive(true);
        }
        else
        {
            Resi_InfoText.SetActive(true);
        }

        yield return new WaitForSeconds(3.0f);
        isWaitingRoom = false;
        isGameStart = true; //게임 시작

        MyPlayer.SetPos(spawnPoint.position);

        MyPlayer.SetNickColor(); //스파이 닉네임 색깔 조정

        ShowPanel(gamePanel); //게임패널안에 들어있는 모든 HUD를 연다
        
        showText.SetActive(false);
        timeObj.SetActive(true);
        MyPlayer.playerInvenUI.Open();
    }

    private void Update()
    {
        PlayTime();
        OnPlayerCheck();

        //잠시 테스트를 위해 주석처리 --------------------2022.11.02
        if (isGameStart && !isWinner)
            WinCheck();

    }

    //총 플레이 타임은 600sec = 10min 이다. 
    public void PlayTime()
    {
        if (Mathf.Floor(selectCountdown) <= 0)
        {
            Debug.Log("TimeOut???");
            photonView.RPC("Winner", RpcTarget.AllBuffered, false);
            //Winner(false);
            // Count 0일때 동작할 함수 삽입
        }
        else
        {
            selectCountdown -= Time.deltaTime;
            timeText.text = selectCountdown.ToString("F2");
        }
    }


    public int GetCrewCount()
    {
        int crewCount = 0;
        for (int i = 0; i < Players.Count; i++)
            if (!Players[i].isSpy) ++crewCount;
        return crewCount;
    }

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

        int BagCount = 0;

        for (int i = 0; i < BoxManager.instance.Agent.NPCInvenUI.inven.SlotCount; i++)
        {
            if (BoxManager.instance.Agent.NPCInvenUI.NPCSlotUIs[i].ItemSlot.SlotItemData == null) continue;
            if (BoxManager.instance.Agent.NPCInvenUI.NPCSlotUIs[i].ItemSlot.SlotItemData.id == (uint)ItemIDCode.Bag)
            {
                BagCount++;
            }
        }

        if (/*(impoCount == 0 && crewCount > 0) || */BagCount == 3) // 모든 임포가 죽음
        {
            photonView.RPC("Winner", RpcTarget.AllBuffered, true);
        }
        else if ((impoCount != 0 && impoCount > crewCount) || BoxManager.instance.Agent.IsDead == true) // 임포가 크루보다 많음
        {
            photonView.RPC("Winner", RpcTarget.AllBuffered, false);
        }

    }

    [PunRPC]
    public void Winner(bool isCrewWin)
    {
        if (!isGameStart) return;

        if (!isWinner)
        {
            Debug.Log($"isCrewWin : {isCrewWin}");
            if (isCrewWin)
            {
                LogManager.Log("조직원 승리");
                print("조직원 승리");
                ShowPanel(Resi_WinPanel);
                Invoke("WinnerDelay", 3);
            }
            else
            {
                LogManager.Log("스파이 승리");
                print("스파이 승리");
                ShowPanel(SPY_WinPanel);
                Invoke("WinnerDelay", 3);
            }

            isWinner = true;
        }
    }

    void WinnerDelay()
    {
        Application.Quit();
    }

    public void OnRandomSetMasterClient()
    {
        bool isEnd = false;
        while (!isEnd)
        {
            int rand = Random.Range(0, PhotonNetwork.PlayerList.Length + 1);

            Photon.Realtime.Player player = PhotonNetwork.PlayerList[rand];
            if (!player.IsMasterClient)
            {
                PhotonNetwork.SetMasterClient(player);
                isEnd = true;
            }
        }
    }

    private bool IsMasterClientCheck()
    {
        foreach (var player in PhotonNetwork.PlayerList)
        {
            if (player.IsMasterClient)
                return true;
        }

        return false;
    }

    private void OnPlayerCheck()
    {
        foreach (var player in Players)
        {
            bool isCheck = false;
            foreach (var punplayer in PhotonNetwork.PlayerList)
            {
                if(player.actor == punplayer.ActorNumber)
                {
                    isCheck = true;
                    continue;
                }
            }

            if (!isCheck)
            {
                Players.Remove(player);
                if (PhotonNetwork.IsMasterClient)
                {
                    PhotonNetwork.Destroy(player.gameObject);
                }
                Destroy(player.gameObject);
            }
        }
    }
}





