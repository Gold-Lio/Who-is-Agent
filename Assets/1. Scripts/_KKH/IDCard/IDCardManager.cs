using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDCardManager : MonoBehaviour
{
    public int IDCardCount = 7;
    public Sprite[] npcImage;
    public NPCIDCard[] npcIDCard;

    private static IDCardManager m_instance; // 싱글톤이 할당될 static 변수
    public static IDCardManager instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<IDCardManager>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private void Awake()
    {
        npcIDCard = new NPCIDCard[IDCardCount];
    }
}
