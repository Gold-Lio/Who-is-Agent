using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using Photon.Pun;

public class ItemSlotUI : MonoBehaviourPun, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Inventory inventory;
    public Inventory Inventory => inventory;

    // 아이템 슬롯 아이디
    protected uint id;

    // 이 슬롯UI에서 가지고 있을 ItemSlot(inventory클래스가 가지고 있는 ItemSlot중 하나)
    protected ItemSlot itemSlot;

    // 인벤토리 UI
    protected InventoryUI invenUI;

    public SlotType slotType;

    // 아이템의 Icon을 표시할 이미지 컴포넌트
    protected Image itemImage;

    // 아이템 슬롯 아이디(읽기 전용)
    public uint ID { get => id; }

    // 이 슬롯UI에서 가지고 있을 ItemSlot(읽기 전용)
    public ItemSlot ItemSlot { get => itemSlot; }

    // 함수들 --------------------------------------------------------------------------------------
    protected virtual void Awake()  // 오버라이드 가능하도록 virtual 추가
    {
        itemImage = transform.GetChild(0).GetComponent<Image>();    // 아이템 표시용 이미지 컴포넌트 찾아놓기
    }

    /// <summary>
    /// ItemSlotUI의 초기화 작업
    /// </summary>
    /// <param name="newID">이 슬롯의 ID</param>
    /// <param name="targetSlot">이 슬롯이랑 연결된 ItemSlot</param>
    /// <param name="type">슬롯 타입 (Inven, Box, Temp)</param>
    public virtual void Initialize(uint newID, ItemSlot targetSlot, SlotType type)
    {
        slotType = type;
        //invenUI = GameManager.instance.MainPlayer.playerInvenUI; // 미리 찾아놓기

        invenUI = NetworkManager.instance.MyPlayer.playerInvenUI;
        id = newID;
        itemSlot = targetSlot;
        itemSlot.onSlotItemChange = Refresh; // ItemSlot에 아이템이 변경될 경우 실행될 델리게이트에 함수 등록        
    }

    /// <summary>
    /// 슬롯에서 표시되는 아이콘 이미지 갱신용 함수
    /// </summary>
    public virtual void Refresh()
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

    /// <summary>
    /// 슬롯위에 마우스 포인터가 들어왔을 때
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSlot.SlotItemData != null)
        {
            //Debug.Log($"마우스가 {gameObject.name}안으로 들어왔다.");
            GameManager.instance.Detail.Open(itemSlot.SlotItemData);
        }
    }

    /// <summary>
    /// 슬롯위에서 마우스 포인터가 나갔을 때
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log($"마우스가 {gameObject.name}에서 나갔다.");
        GameManager.instance.Detail.Close();
    }

    /// <summary>
    /// 슬롯위에서 마우스 포인터가 움직일 때
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerMove(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 슬롯을 마우스로 클릭했을 때
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
