using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IDCardPanel : MonoBehaviour
{
    public GameObject IDCardPrefab;

    private NPCIDCard[] npcIdCard;

    private void Awake()
    {
        npcIdCard = new NPCIDCard[IDCardManager.instance.IDCardCount];
    }

    void Start()
    {
        for (int i = 0; i < IDCardManager.instance.IDCardCount; i++)
        {
            GameObject obj = Instantiate(IDCardPrefab, transform);
            obj.name = $"{IDCardPrefab.name}_{i}";            // 이름 지어주고
            IDCardManager.instance.npcIDCard[i] = obj.GetComponentInChildren<NPCIDCard>();
            IDCardManager.instance.npcIDCard[i].Initialize(i);
        }
    }
}
