using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static UIManager;
using UnityEngine.UIElements;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

public enum PlayerType
{
    Master_Spy,
    Random_1,
    Random_2,
}
public class NetworkManager : MonoBehaviourPunCallbacks
{
    PhotonView PV;

    public static NetworkManager NM;

    public static NetworkManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<NetworkManager>();
                //m_instance.Initialize();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static NetworkManager m_instance; // �̱����� �Ҵ�� static ����

    public void Awake() => NM = this;

    public List<Player> Players = new List<Player>();
    public Player MyPlayer;

    public PlayerType playerType;

    public bool isWaitingRoom = false;
    public bool isGameStart = false;
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
    //���� ���� �г�
    public GameObject infoPanel;

    [Header("InGamePanel")]
    public GameObject gamePanel;
    //�������� ���� Text
    public GameObject Resi_InfoText, Spy_InfoText;

    [Header("Game_Mission")]
    public GameObject Resi_Mission, Spy_Mission;

    [Header("Win Panel")]
    //�¸� �г�
    public GameObject Resi_WinPanel, SPY_WinPanel;

    /// <summary>
    ////MAP
    /// </summary>
    //public GameObject WaitingMap, MainMap;

    //- - ���ʱ�ȭ�� 

    private void Start()
    {
        PV = photonView;

        MyPlayer = PhotonNetwork.Instantiate("Player", new Vector2(Random.Range(-12f, -8f), Random.Range(-38f, -41f)),
            Quaternion.identity).GetComponent<Player>();

        SetRandColor();
        isWaitingRoom = true;
    }

    //// ������ ���� ��ġ ����
    //Vector3 randomSpawnPos = Random.insideUnitSphere * 5f;
    //// ��ġ y���� 0���� ����
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

    //�г��� �����ϴ� ShowPanel
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
        // ������ ���ӽ���
        

        //��� �׽�Ʈ�� ���� �ּ�ó��
        //SetPlayerType();


        SetSPY();
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        //�� ��Ÿ�� NULL?? ��ŸƮ�� �ȵǴ°�....
        PV.RPC("GameStartRPC", RpcTarget.AllViaServer);
    }


    //��� �׽�Ʈ�� ���� �ּ�ó��
    //void SetPlayerType()
    //{
    //    int maxplayer = System.Convert.ToInt32(RoomManager.instance.PNum);
    //    if(maxplayer <= 5)
    //    {
    //        playerType = PlayerType.Random_1;
    //    }
    //    else if(maxplayer >= 6)
    //    {
    //        playerType = PlayerType.Random_2;
    //    }
    //}


    void SetSPY()
    {
        List<Player> GachaList = new List<Player>(Players);

        //���常 SPY
        if (playerType == PlayerType.Master_Spy)
        {
            Players[0].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
        }

        //����_������ 1�� 
        else if (playerType == PlayerType.Random_1)
        {
            for (int i = 0; i < 1; i++)
            {
                int rand = Random.Range(0, GachaList.Count); // ����
                Players[rand].GetComponent<PhotonView>().RPC("SetSpy", RpcTarget.AllViaServer, true);
                GachaList.RemoveAt(rand);
            }
        }

        //����_������ 2�� 
        else if (playerType == PlayerType.Random_2)
        {
            for (int i = 0; i < 2; i++)
            {
                int rand = Random.Range(0, GachaList.Count); // ����
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
            Spy_Mission.SetActive(true);
        }
        else
        {
            Resi_InfoText.SetActive(true);
            Resi_Mission.SetActive(true);
        }


        yield return new WaitForSeconds(3);
        isWaitingRoom = false;
        isGameStart = true; //���� ����

        MyPlayer.SetPos(spawnPoint.position);

        MyPlayer.SetNickColor(); //������ �г��� ���� ����

        ShowPanel(gamePanel); //�����гξȿ� ����ִ� ��� HUD�� ����. 

        //���� �ȿ� ���� �κ��丮��   �ش� ������ �°� �̼� ������ �����ն��� �̰��� �����Ѵ�. 
        //StartCoroutine(LightCheckCo());  //�� ����
        timeObj.SetActive(true);
        selectCountdown = baseTime;
    }

    private void Update()
    {
        PlayTime();
        OnPlayerCheck();
        //if (!IsMasterClientCheck())
        //{
        //    OnRandomSetMasterClient();
        //}

        //��� �׽�Ʈ�� ���� �ּ�ó��
        //if(isGameStart && !isWinner)
        //    photonView.RPC("WinCheck", RpcTarget.AllBuffered);

    }

    //�� �÷��� Ÿ���� 600sec = 10min �̴�. 
    public void PlayTime()
    {
        if (Mathf.Floor(selectCountdown) <= 0)
        {
            Winner(false);
            // Count 0�϶� ������ �Լ� ����
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

        int BagCount = 0;

        for (int i = 0; i < BoxManager.instance.Agent.NPCInvenUI.inven.SlotCount; i++)
        {
            if (BoxManager.instance.Agent.NPCInvenUI.NPCSlotUIs[i].ItemSlot.SlotItemData == null) continue;
            if (BoxManager.instance.Agent.NPCInvenUI.NPCSlotUIs[i].ItemSlot.SlotItemData.id == (uint)ItemIDCode.Bag)
            {
                BagCount++;
            }
        }

        if ((impoCount == 0 && crewCount > 0) || BagCount == 3) // ��� ������ ����
            Winner(true);
        else if ((impoCount != 0 && impoCount > crewCount)|| BoxManager.instance.Agent.IsDead == true) // ������ ũ�纸�� ����
            Winner(false);
    }

    public void Winner(bool isCrewWin)
    {
        if (!isGameStart) return;

        if (isCrewWin)
        {
            LogManager.Log("���������� �¸�");
            print("���������� �¸�");
            ShowPanel(Resi_WinPanel);
            Invoke("WinnerDelay", 3);
        }
        else
        {
            LogManager.Log("������ �¸�");
            print("������ �¸�");
            ShowPanel(SPY_WinPanel);
            Invoke("WinnerDelay", 3);
        }

        isWinner = true;
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
                Destroy(player.gameObject);
            }
        }
    }
}





