using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    /// 인벤토리가 가지는 각 아이템 칸.
    private ItemSlot[] slots = null;
    /// 인벤토리 기본 크기
    public const int Default_Inventory_Size = 1;

    /// 인벤토리의 크기
    public int SlotCount => slots.Length;

    /// <summary>
    /// 인덱서. 인벤토리에서 슬롯 가져오기
    /// </summary>
    /// <param name="index">가져올 슬롯의 인덱스</param>
    /// <returns>index번째의 아이템 슬롯</returns>
    public ItemSlot this[int index] => slots[index];


    // 함수(주요기능) ------------------------------------------------------------------------------    

    /// <summary>
    /// 인벤토리 생성자
    /// </summary>
    /// <param name="size">인벤토리의 슬롯 수. 기본 값으로 Default_Inventory_Size(6) 사용 </param>
    public Inventory(SlotType type, int size = Default_Inventory_Size)
    {
        slots = new ItemSlot[size];     // 입력받은 갯수로 슬롯만들기
        for (int i = 0; i < size; i++)
        {
            slots[i] = new ItemSlot(type);
        }
    }

    /// <summary>
    /// 아이템 추가하기 (적절한 빈칸에 넣기)
    /// </summary>
    /// <param name="code">추가할 아이템의 코드</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemIDCode code)
    {
        return AddItem(GameManager.instance.ItemData[code]);
    }

    /// <summary>
    /// 아이템 추가하기 (적절한 빈칸에 넣기)
    /// </summary>
    /// <param name="data">추가할 아이템의 아이템 데이터</param>
    /// <returns>아이템 추가 성공 여부(true면 인벤토리에 아이템이 추가됨)</returns>
    public bool AddItem(ItemData data)
    {        
        bool result = false;
        //Debug.Log($"인벤토리에 {data.itemName}을 추가합니다");

        ItemSlot target = FindSameItem(data);   // 같은 종류의 아이템이 인벤토리에 있는지 찾기
        if (target != null)
        {
            result = true;
            //Debug.Log($"{data.itemName}을 하나 증가시킵니다.");
        }
        else
        {
            // 같은 종류의 아이템이 없다.
            ItemSlot empty = FindEmptySlot();    // 적절한 빈 슬롯 찾기
            if (empty != null)
            {
                empty.AssignSlotItem(data);      // 아이템 할당
                result = true;
                //Debug.Log($"아이템 슬롯에 {data.itemName}을 할당합니다.");
            }
            else
            {
                // 모든 슬롯에 아이템이 들어있다.(인벤토리가 가득찼다.)
                //Debug.Log($"실패 : 인벤토리가 가득찼습니다.");
            }
        }

        return result;
    }

    /// <summary>
    /// 아이템 추가하기 (특정한 슬롯에 넣기)
    /// </summary>
    /// <param name="id">추가할 아이템의 아이디</param>
    /// <param name="index">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>아이템을 추가하는데 성공하면 true. 아니면 false</returns>
    public bool AddItem(uint id, uint index)
    {
        return AddItem(GameManager.instance.ItemData[id], index);
    }

    /// <summary>
    /// 아이템 추가하기 (특정한 슬롯에 넣기)
    /// </summary>
    /// <param name="data">추가할 아이템의 아이템 데이터</param>
    /// <param name="index">아이템을 추가할 슬롯의 인덱스</param>
    /// <returns>아이템을 추가하는데 성공하면 true. 아니면 false</returns>
    public bool AddItem(ItemData data, uint index)
    {
        bool result = false;

        //Debug.Log($"인벤토리의 {index} 슬롯에  {data.itemName}을 추가합니다");
        ItemSlot slot = slots[index];   // index번째의 슬롯 가져오기

        if (slot.IsEmpty())              // 찾은 슬롯이 비었는지 확인
        {
            slot.AssignSlotItem(data);  // 비어있으면 아이템 추가
            result = true;
            //Debug.Log($"추가에 성공했습니다.");
        }
        else
        {
            if (slot.SlotItemData == data)  // 같은 종류의 아이템인가?
            {
                result = true;
            }
            else
            {
                //Debug.Log($"실패 : {index} 슬롯에는 다른 아이템이 들어있습니다.");
            }
        }

        return result;
    }

    /// <summary>
    /// 특정 슬롯의 아이템을 버리는 함수
    /// </summary>
    /// <param name="slotIndex">아이템을 버릴 슬롯의 인덱스</param>
    /// <param name="decreaseCount">버리는 아이템 갯수</param>
    /// <returns>버리는데 성공하면 true, 아니면 false</returns>
    public bool RemoveItem(uint slotIndex, uint decreaseCount = 1)
    {
        bool result = false;

        //Debug.Log($"인벤토리에서 {slotIndex} 슬롯의 아이템을 {decreaseCount}개 비웁니다.");
        if (IsValidSlotIndex(slotIndex))        // slotIndex가 적절한 범위인지 확인
        {
            ItemSlot slot = slots[slotIndex];
            //Debug.Log($"삭제에 성공했습니다.");
            slot.ClearSlotItem();
            result = true;
        }
        else
        {
            //Debug.Log($"실패 : 잘못된 인덱스입니다.");
        }

        return result;
    }

    /// <summary>
    /// 아이템 이동시키기
    /// </summary>
    /// <param name="from">시작 슬롯의 ID</param>
    /// <param name="to">도착 슬롯의 ID</param>
    public virtual void MoveItem(ItemSlotUI fromSlot, ItemSlotUI toSlot)
    {
        // 다른 종류의 아이템이다. => 아이템과 아이템 갯수를 서로 스왑한다.
        ItemData tempItemData = toSlot.ItemSlot.SlotItemData;    // 임시 저장
        toSlot.ItemSlot.AssignSlotItem(fromSlot.ItemSlot.SlotItemData);   // to에다 from의 정보 넣기
        fromSlot.ItemSlot.AssignSlotItem(tempItemData);               // from에다가 임시로 저장한 to의 정보 넣기 
    }

    /// <summary>
    /// 아이템을 나눠서 임시 슬롯에 저장
    /// </summary>
    /// <param name="from">아이템을 나눌 슬롯</param>
    public void TempRemoveItem(uint from)
    {
        if (IsValidAndNotEmptySlot(from))  // from이 절절한 슬롯이면
        {
            ItemSlot slot = slots[from];
            TempItemSlotUI.TempSlotUI.TempSlot.AssignSlotItem(slot.SlotItemData);  // temp 슬롯에 지정된 갯수의 아이템 할당
            slot.ClearSlotItem();
        }
    }

    // 함수(백엔드) --------------------------------------------------------------------------------
    /// <summary>
    /// 빈 슬롯을 찾아주는 함수
    /// </summary>
    /// <returns>빈 슬롯</returns>
    private ItemSlot FindEmptySlot()
    {
        ItemSlot result = null;

        foreach (var slot in slots)  // slots를 전부 순회하면서
        {
            if (slot.IsEmpty())     // 빈 슬롯인지 확인
            {
                result = slot;      // 빈 슬롯이면 foreach break하고 리턴
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 같은 종류의 아이템이 들어있고 슬롯에 여유도 있는 슬롯을 찾아주는 함수
    /// </summary>
    /// <param name="itemData">찾을 아이템</param>
    /// <returns>찾을 아이템이 들어있는 슬롯</returns>
    private ItemSlot FindSameItem(ItemData itemData)
    {
        ItemSlot slot = null;
        for (int i = 0; i < SlotCount; i++)
        {
            // 같은 종류의 아이템이 있고 슬롯에 아이템이 들어갈 여유가 있음
            if (slots[i].SlotItemData == itemData)
            {
                slot = slots[i];
                break;      // 찾으면 break로 종료
            }
        }
        return slot;
    }

    /// <summary>
    /// index값이 적절한 범위인지 확인해주는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 범위. 아니면 false</returns>
    private bool IsValidSlotIndex(uint index) => (index < SlotCount) || (index == TempItemSlotUI.TempSlotID);
    //{
    //    return index < SlotCount;
    //}

    /// <summary>
    /// index값이 적절한 범위이면서 아이템이 들어있는지도 확인해주는 함수
    /// </summary>
    /// <param name="index">확인할 인덱스</param>
    /// <returns>true면 적절한 범위이면서 아이템도 들어있음.</returns>
    private bool IsValidAndNotEmptySlot(uint index)
    {
        ItemSlot testSlot = null;
        if (index != TempItemSlotUI.TempSlotID)
        {
            testSlot = slots[index];    // index가 tempSlot이 아니면 인덱스로 찾기
        }
        else
        {
            //Debug.Log(TempItemSlotUI.TempSlotUI.TempSlot.SlotItemData);
            testSlot = TempItemSlotUI.TempSlotUI.TempSlot;    // index가 tempSlot인 경우 TempSlot 저장
        }

        return (IsValidSlotIndex(index) && !testSlot.IsEmpty());
    }
}