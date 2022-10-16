using UnityEngine;
using Photon.Pun;

public class WeaponBoxItemSlotUI : ItemSlotUI
{
    // 박스 구분 ID
    private int boxId = 0;
    // 박스 구분 ID 프로퍼티
    public int BoxID => boxId;

    protected override void Awake()  // 오버라이드 가능하도록 virtual 추가
    {
        base.Awake();
    }

    /// <summary>
    /// ItemSlotUI의 초기화 작업
    /// </summary>
    /// <param name="newID">이 슬롯의 ID</param>
    /// <param name="targetSlot">이 슬롯이랑 연결된 ItemSlot</param>
    /// <param name="type">슬롯 타입 (Inven, Box, Temp)</param>
    /// <param name="boxid">박스 구분 ID</param>
    public void Initialize(uint newID, ItemSlot targetSlot, SlotType type, int boxid)
    {
        slotType = type;
        boxId = boxid;
        //invenUI = GameManager.instance.Boxs[BoxID].BoxInvenUI; // 미리 찾아놓기
        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot에 아이템이 변경될 경우 실행될 델리게이트에 함수 등록
    }

    /// <summary>
    /// 슬롯 갱신
    /// </summary>
    public override void Refresh()
    {
        if (itemSlot.SlotItemData != null)
        {
            // 이 슬롯에 아이템이 들어있을 때
            itemImage.sprite = itemSlot.SlotItemData.itemIcon;  // 아이콘 이미지 설정하고
            itemImage.color = Color.white;  // 불투명하게 만들기
        }
        else
        {
            // 이 슬롯에 아이템이 없을 때
            itemImage.sprite = null;        // 아이콘 이미지 제거하고
            itemImage.color = Color.clear;  // 투명하게 만들기
        }
    }
}
