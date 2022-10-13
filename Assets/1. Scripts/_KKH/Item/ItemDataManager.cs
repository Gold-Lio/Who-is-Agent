using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameManager에서 관리할 ItemDataManager. 아이템 종류별 데이터만 가지고 있음.
/// </summary>
public class ItemDataManager : MonoBehaviour
{
    public ItemData[] itemDatas;    // 아이템 종류별 데이터

    public ItemData this[uint i]     // 인덱서.
    {
        get => itemDatas[i];
    }

    public ItemData this[ItemIDCode code]   // 인덱서를 통행 편리하게 아이템 종류별 데이터에 접근
    {
        get => itemDatas[(uint)code];
    }

    public int Length
    {
        get => itemDatas.Length;
    }
}
