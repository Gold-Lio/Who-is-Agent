using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 데이터를 저장하는 데이터 파일을 만들게 해주는 스크립트
/// </summary>
[CreateAssetMenu(fileName = "New Item Data IDCard", menuName = "Scriptable Object/Item Data IDCard", order = 3)]
public class ItemData_IDCard : ItemData
{
    [Header("NPC 정보")]
    public IDCard NPCID = IDCard.Goat;
}
