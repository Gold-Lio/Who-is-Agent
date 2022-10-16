using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Item Data Weapon", menuName = "Scriptable Object/Item Data Weapon", order = 2)]
public class ItemData_Weapon : ItemData, IUsable
{
    public void Use(GameObject target = null)
    {
        NPC npc = target.GetComponent<NPC>();
        if(npc != null)
        {
            npc.NPCDead();
        }
    }
}
