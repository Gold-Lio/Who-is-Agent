using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System;
using Photon.Pun;

/// <summary>
/// 임시로 한번씩만 보이는 슬롯
/// </summary>
public class TempItemSlotUI : ItemSlotUI
{
    // TempSlot용 아이디
    public const uint TempSlotID = 99999;   //숫자는 의미가 없다. Slot Index로 적절하지 않은 값이면 된다.

    private ItemSlot tempSlot;
    public ItemSlot TempSlot
    {
        get { return tempSlot; }
        set { tempSlot = value; }
    }

    // 임시 슬롯(아이템 드래그나 아이템 분리할 때 사용)
    private static TempItemSlotUI tempItemSlotUI;
    public static TempItemSlotUI TempSlotUI => tempItemSlotUI;

    private PlayerInputAction inputActions;

    protected override void Awake()
    {
        if (tempItemSlotUI == null)
        {
            tempItemSlotUI = this;
            itemImage = GetComponent<Image>();  // 이미지 찾아오기
            tempSlot = new ItemSlot(SlotType.Temp);      // 임시 용도로 사용하는 슬롯은 따로 생성
            inputActions = new PlayerInputAction();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            if (tempItemSlotUI)
            {
                Destroy(this.gameObject);
            }
        }

    }

    private void Start()
    {
        // TempSlot 초기화
        Initialize(TempSlotID, TempSlot, SlotType.Temp);    // TempItemSlotUI와 TempSlot 연결
        inputActions.PlayerUI.Drop.canceled += OnDrop;
        Close(); // tempItemSlotUI 닫은채로 시작하기
    }

    private void OnEnable()
    {
        inputActions.PlayerUI.Enable();
    }

    private void OnDisable()
    {
        inputActions.PlayerUI.Disable();
    }

    private void Update()
    {
        transform.position = Mouse.current.position.ReadValue();    // 마우스 위치에 맞춰서 임시 슬롯 이동        
    }

    /// <summary>
    /// 임시 슬롯을 보이도록 열기
    /// </summary>
    public void Open()
    {
        if (!ItemSlot.IsEmpty())    // 슬롯에 아이템이 들어있을 때만 열기
        {
            transform.position = Mouse.current.position.ReadValue();    // 보이기 전에 위치 조정
            gameObject.SetActive(true); // 실제로 보이게 만들기(활성화시키기)
        }
    }

    /// <summary>
    /// 임시 슬롯이 보이지 않게 닫기
    /// </summary>
    public void Close()
    {
        itemSlot.ClearSlotItem();       // 슬롯에 들어있는 아이템과 갯수 비우기
        gameObject.SetActive(false);    // 실제로 보이지 않게 만들기(비활성화시키기)
    }

    /// <summary>
    /// 슬롯이 비었는지 확인
    /// </summary>
    /// <returns>true면 슬롯이 비어있다.</returns>e
    public bool IsEmpty() => itemSlot.IsEmpty();

    public void Drop()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();      // 마우스 위치 받아오기

        if (!IsEmpty())
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            //Vector3 pos = GameManager.instance.MainPlayer.OnItemDropPosition(GameManager.instance.MainPlayer.transform.position);

            Vector3 pos = NetworkManager.instance.MyPlayer.OnItemDropPosition(NetworkManager.instance.MyPlayer.transform.position);
            ItemFactory.MakeItem(ItemSlot.SlotItemData.id, pos);   // 임시 슬롯에 들어있는 모든 아이템을 생성
                        
            Close();    // 임시슬롯UI 닫고 클리어하기
        }
    }

    /// <summary>
    /// 인벤토리 바깥쪽에 아이템을 떨구는 함수. 임시 슬롯에 아이템이 들어있고 마우스 왼쪽 버튼이 떨어질 때 실행.
    /// </summary>
    /// <param name="_"></param>
    public void OnDrop(InputAction.CallbackContext _)
    {
        Drop();
    }
}
