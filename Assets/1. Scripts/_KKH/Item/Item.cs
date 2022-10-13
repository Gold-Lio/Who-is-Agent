using System.Collections;
using System.Collections.Generic;

using Photon.Pun;
using UnityEngine;

/// <summary>
/// 아이템 1개를 나타낼 클래스
/// </summary>
public class Item : MonoBehaviourPun
{
    public ItemData data;   // 이 아이템 종류별로 동일한 데이타
    private void Start()
    {
        //Instantiate(data.prefab, transform.position, transform.rotation, transform);
    }

}
