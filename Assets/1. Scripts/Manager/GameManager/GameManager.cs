using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// ���� ���� ����, ���� UI�� �����ϴ� ���� �Ŵ���

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
    /// ������ ������ â
    /// </summary>
    DetailInfoUI detail;
    public DetailInfoUI Detail => detail;
    #endregion ---------------------------------------------------------------------------------

    public bool isGameDone, isSpyWin, isResiWin;

    public Text pingText;


    // �ܺο��� �̱��� ������Ʈ�� �����ö� ����� ������Ƽ
    public static GameManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
                m_instance.Initialize();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static GameManager m_instance; // �̱����� �Ҵ�� static ����

    public bool isGameover { get; private set; } // ���� ���� ����

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �ִٸ�
        if (instance != this)
        {
            // �ڽ��� �ı�
            Destroy(gameObject);
        }
    }

    // ���� ���۰� ���ÿ� �÷��̾ �� ���� ������Ʈ�� ����
    private void Start()
    {
        InvokeRepeating("UpdatePing", 2, 2);
    }

    // ���� ���� ó��
    public void EndGame()
    {
        // ���� ���� ���¸� ������ ����
        isGameover = true;
        // ���� ���� UI�� Ȱ��ȭ
        //UIManager.instance.SetActiveGameoverUI(true);
    }


    // ���� ������ �ڵ� ����Ǵ� �޼���
    public override void OnLeftRoom()
    {
        // ���� ������ �κ� ������ ���ư�
        SceneManager.LoadScene("Lobby");
    }

    //�� üũ
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