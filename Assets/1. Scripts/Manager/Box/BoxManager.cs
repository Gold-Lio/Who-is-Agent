using UnityEngine;
using Photon.Pun;

public class BoxManager : MonoBehaviourPunCallbacks
{
    public GameObject boxParent;
    private Box[] boxs;
    public Box[] Boxs => boxs;

    public GameObject npcParent;
    private NPC[] npcs;
    public NPC[] NPCs => npcs;

    private NPC agent;
    public NPC Agent => agent;

    public GameObject scannerParent;
    private IDScanner[] scanners;
    public IDScanner[] Scanners => scanners;

    public GameObject weaponboxParent;
    private WeaponBox[] weaponboxs;
    public WeaponBox[] Weaponboxs => weaponboxs;

    private bool[] isBoxArrangement;
    private bool[] isItemArrangement;

    private int itemCount = 0;

    public ItemIDCode ItemID;

    private static BoxManager m_instance; // �̱����� �Ҵ�� static ����
    public static BoxManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<BoxManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private void Awake()
    {
        itemCount = GameManager.instance.ItemData.Length;

        boxs = boxParent.GetComponentsInChildren<Box>();
        if (boxs != null && boxs.Length > 0) 
        {
            for (int i = 0; i < boxs.Length; i++)
            {
                boxs[i].boxNum = i;
            }
        }
        npcs = npcParent.GetComponentsInChildren<NPC>();
        weaponboxs = weaponboxParent.GetComponentsInChildren<WeaponBox>();
        scanners = scannerParent.GetComponentsInChildren<IDScanner>();
        if (scanners != null && scanners.Length > 0)
        {
            for (int i = 0; i < scanners.Length; i++)
            {
                scanners[i].scannerNum = i;
            }
        }

        isItemArrangement = new bool[itemCount];
        isBoxArrangement = new bool[Boxs.Length];
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int rand = Random.Range(0, NPCs.Length);
            photonView.RPC("PunSetAgent", RpcTarget.AllBuffered, rand);

            for (int i = 0; i < itemCount; i++)
            {
                if (GameManager.instance.ItemData[(ItemIDCode)i] == null || i == (int)ItemIDCode.Syringe || i == rand + 11)
                    isItemArrangement[i] = true;
                else
                    isItemArrangement[i] = false;
            }

            bool itemCheck = false;

            while (!itemCheck)
            {
                bool isItem = false;
                while (!isItem)
                {
                    int boxnum = Random.Range(0, boxs.Length);
                    if(BoxArragementCheck())
                    {
                        BoxArragementClear();
                    }

                    if (ItemArragementCheck())
                    {
                        isItem = true;
                        itemCheck = true;
                    }

                    rand = Random.Range(0, itemCount);
                    if (isItemArrangement[rand] == false && isBoxArrangement[boxnum] == false)
                    {                        
                        Boxs[boxnum % Boxs.Length].ItemID = rand;
                        Boxs[boxnum % Boxs.Length].CreateItem();
                        isItem = true;
                        isBoxArrangement[boxnum] = true;
                        isItemArrangement[rand] = true;
                    }
                }
            }
        }
    }

    private bool ItemArragementCheck()
    {
        bool itemCheck = true;
        foreach (bool check in isItemArrangement)
        {
            if (!check)
            {
                itemCheck = false;
                break;
            }
        }

        return itemCheck;
    }

    private bool BoxArragementCheck()
    {
        bool boxCheck = true;
        foreach (bool check in isBoxArrangement)
        {
            if (!check)
            {
                boxCheck = false;
                break;
            }
        }

        return boxCheck;
    }

    private void BoxArragementClear()
    {
        for(int i = 0; i < isBoxArrangement.Length; i++)
        {
            isBoxArrangement[i] = false;
        }
    }

    [PunRPC]
    private void PunSetAgent(int num)
    {
        npcs[num].SetAgent();
        agent = npcs[num];
        LogManager.Log($"{agent.name}(��)�� ������Ʈ��");
    }
}
