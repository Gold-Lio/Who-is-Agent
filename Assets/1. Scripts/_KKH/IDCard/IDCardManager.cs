using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDCardManager : MonoBehaviour
{
    public int IDCardCount = 7;
    public Sprite[] npcImage;
    public NPCIDCard[] npcIDCard;

    private static IDCardManager m_instance; // �̱����� �Ҵ�� static ����
    public static IDCardManager instance
    {
        get
        {
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<IDCardManager>();
            }

            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private void Awake()
    {
        npcIDCard = new NPCIDCard[IDCardCount];
    }
}
