using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.XR;

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
        isItemArrangement = new bool[itemCount];

        boxs = boxParent.GetComponentsInChildren<Box>();
        npcs = npcParent.GetComponentsInChildren<NPC>();
        weaponboxs = weaponboxParent.GetComponentsInChildren<WeaponBox>();
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

            int boxnum = 0;
            while (!itemCheck)
            {
                bool isItem = false;
                while (!isItem)
                {
                    if (itemArragementCheck())
                    {
                        isItem = true;
                        itemCheck = true;
                    }

                    rand = Random.Range(0, itemCount);
                    if (isItemArrangement[rand] == false)
                    {
                        LogManager.Log($"Box�� {rand} �Է�");
                        Boxs[boxnum % Boxs.Length].ItemID = rand;
                        Boxs[boxnum % Boxs.Length].CreateItem();
                        isItem = true;
                        boxnum++;
                        isItemArrangement[rand] = true;
                    }
                }
            }
        }
    }

    private bool itemArragementCheck()
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

    [PunRPC]
    private void PunSetAgent(int num)
    {
        npcs[num].SetAgent();
        agent = npcs[num];
        LogManager.Log($"{agent.name}(��)�� ������Ʈ��");
    }
}
