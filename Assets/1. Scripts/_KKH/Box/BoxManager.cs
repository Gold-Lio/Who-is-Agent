using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BoxManager : MonoBehaviour
{
    public Box[] boxs;
    public Box[] Boxs => boxs;

    public NPC[] npcs;
    public NPC[] NPCs => npcs;

    public WeaponBox[] weaponboxs;
    public WeaponBox[] Weaponboxs => weaponboxs;

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
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int rand = Random.Range(0, NPCs.Length);
            LogManager.Log($"{rand}��° NPC�� ������Ʈ��");
            npcs[rand].SetAgent();

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
}
