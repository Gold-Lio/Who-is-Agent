using System.Collections.Generic;
using UnityEngine;

public class ItemSlot
{
    // 변수 ---------------------------------------------------------------------------------------
    // 슬롯에 있는 아이템(ItemData)
    ItemData slotItemData;
    SlotType slotType = SlotType.Inventory;

    // 프로퍼티 ------------------------------------------------------------------------------------

    /// <summary>
    /// 슬롯에 있는 아이템(ItemData)
    /// </summary>
    public ItemData SlotItemData
    {
        get => slotItemData;
        set
        {
            if (slotItemData != value)
            {
                slotItemData = value;
                onSlotItemChange?.Invoke();
            }
        }
    }

    // 델리게이트 ----------------------------------------------------------------------------------
    /// <summary>
    /// 슬롯에 들어있는 아이템의 종류나 갯수가 변경될 때 실행되는 델리게이트
    /// </summary>
    public System.Action onSlotItemChange;

    // 함수 ---------------------------------------------------------------------------------------

    /// <summary>
    /// 생성자들
    /// </summary>
    public ItemSlot(SlotType type) { slotType = type; }
    public ItemSlot(ItemData data, SlotType type)
    {
        slotItemData = data;
        slotType = type;
    }
    public ItemSlot(ItemSlot other, SlotType type)
    {
        slotItemData = other.SlotItemData;
        slotType = type;
    }

    /// <summary>
    /// 슬롯에 아이템을 설정하는 함수 
    /// </summary>
    /// <param name="itemData">슬롯에 설정할 ItemData</param>
    /// /// <param name="count">슬롯에 설정할 아이템 갯수</param>
    public void AssignSlotItem(ItemData itemData)
    {
        SlotItemData = itemData;
    }


    /// <summary>
    /// 슬롯을 비우는 함수
    /// </summary>
    public void ClearSlotItem()
    {
        SlotItemData = null;
    }

    /// <summary>
    /// 아이템을 사용하는 함수
    /// </summary>
    /// <param name="target">아이템의 효과를 받을 대상(보통 플레이어)</param>
    public void UseSlotItem(GameObject target = null)
    {
        IUsable usable = SlotItemData as IUsable;   // 이 아이템이 사용가능한 아이템인지 확인
        if (usable != null)
        {
            // 아이템이 사용가능하면
            usable.Use(target); // 아이템 사용하고
            ClearSlotItem(); // 슬롯비우기
        }
    }


    // 함수(백엔드) --------------------------------------------------------------------------------

    /// <summary>
    /// 슬롯이 비었는지 확인해주는 함수
    /// </summary>
    /// <returns>true면 비어있는 함수</returns>
    public bool IsEmpty()
    {
        return slotItemData == null;
    }
}