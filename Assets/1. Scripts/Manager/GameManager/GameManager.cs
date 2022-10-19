using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// 게임 오버 여부, 게임 UI를 관리하는 게임 매니저

public class GameManager : MonoBehaviourPunCallbacks
{
    #region kkh --------------------------------------------------------------------------------
    private ItemDataManager itemData;
    public ItemDataManager ItemData
    {
        get => itemData;
    }

    private Player player;
    public Player MainPlayer { get => player; }

    /// <summary>
    /// 아이템 상세정보 창
    /// </summary>
    DetailInfoUI detail;
    public DetailInfoUI Detail => detail;
    #endregion ---------------------------------------------------------------------------------

    public bool isGameDone, isSpyWin, isResiWin;

    public Text pingText;


    // 외부에서 싱글톤 오브젝트를 가져올때 사용할 프로퍼티
    public static GameManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<GameManager>();
                m_instance.Initialize();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static GameManager m_instance; // 싱글톤이 할당될 static 변수

    public bool isGameover { get; private set; } // 게임 오버 상태

    private void Awake()
    {
        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
        }
    }

    // 게임 시작과 동시에 플레이어가 될 게임 오브젝트를 생성
    private void Start()
    {
        InvokeRepeating("UpdatePing", 2, 2);
    }

    // 게임 오버 처리
    public void EndGame()
    {
        // 게임 오버 상태를 참으로 변경
        isGameover = true;
        // 게임 오버 UI를 활성화
        //UIManager.instance.SetActiveGameoverUI(true);
    }


    // 룸을 나갈때 자동 실행되는 메서드
    public override void OnLeftRoom()
    {
        // 룸을 나가면 로비 씬으로 돌아감
        SceneManager.LoadScene("Lobby");
    }

    //핑 체크
    void UpdatePing()
    {
        int pingRate = PhotonNetwork.GetPing();
        pingText.text = "Ping : " + pingRate;
    }

    // kkh --------------------------------------------------------------------------------
    private void Initialize()
    {
        itemData = GetComponent<ItemDataManager>();
        detail = GameObject.Find("Detail").GetComponent<DetailInfoUI>();
        LogManager.SetLogPath();
    }
    // ---------------------------------------------------------------------------------
}