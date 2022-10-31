using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Photon.Pun;

public class InventoryUI : MonoBehaviourPun, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    // ItemSlotUI가 있는 프리팹. 
    public GameObject slotPrefab;   // 초기화시 새로 생성해야할 경우 사용

    // 이 클래스로 표현하려는 인벤토리
    public Inventory inven;

    // 슬롯 생성시 부모가 될 게임 오브젝트의 트랜스폼
    protected Transform slotParent;

    // 이 인벤토리가 가지고 있는 슬롯UI들
    private ItemSlotUI[] slotUIs;

    // 열고 닫기용 캔버스 그룹
    protected CanvasGroup canvasGroup;


    // 드래그 시작 표시용( 시작 id가 InvalideID면 드래그 시작을 안한 것)
    protected const uint InvalideID = uint.MaxValue;

    // 드래그가 시작된 슬롯의 ID
    protected uint dragStartID = InvalideID;

    public Action OnInventoryOpen;
    public Action OnInventoryClose;

    // 드래그 했을때 오브젝트를 저장할 공간
    protected GameObject startObj;

    protected virtual void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        slotParent = transform.Find("Inventory_Base").Find("Grid Setting");
    }

    protected virtual void Start()
    {
        Close();    // 시작할 때 무조건 닫기
    }

    /// <summary>
    /// 인벤토리를 입력받아 UI를 초기화하는 함수
    /// </summary>
    /// <param name="newInven">이 UI로 표시할 인벤토리</param>
    public void InitializeInventory(Inventory newInven)
    {
        inven = newInven;   //즉시 할당
        if (Inventory.Default_Inventory_Size != newInven.SlotCount)    // 기본 사이즈와 다르면 기본 슬롯UI 삭제
        {
            // 기존 슬롯UI 전부 삭제
            ItemSlotUI[] slots = GetComponentsInChildren<ItemSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // 새로 만들기
            slotUIs = new ItemSlotUI[inven.SlotCount];
            for (int i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                slotUIs[i] = obj.GetComponent<ItemSlotUI>();
                slotUIs[i].Initialize((uint)i, inven[i], SlotType.Inventory);       // 각 슬롯UI들도 초기화
            }
        }
        else
        {
            // 크기가 같을 경우 슬롯UI들의 초기화만 진행
            slotUIs = slotParent.GetComponentsInChildren<ItemSlotUI>();
            for (int i = 0; i < inven.SlotCount; i++)
            {
                slotUIs[i].Initialize((uint)i, inven[i], SlotType.Inventory);
            }
        }

        RefreshAllSlots();  // 전체 슬롯UI 갱신
    }


    /// <summary>
    /// 모든 슬롯의 Icon이미지를 갱신
    /// </summary>
    protected virtual void RefreshAllSlots()
    {
        foreach (var slotUI in slotUIs)
        {
            slotUI.Refresh();
        }
    }

    /// <summary>
    /// 인벤토리 열고 닫기
    /// </summary>
    public void InventoryOnOffSwitch()
    {
        if (canvasGroup.blocksRaycasts)  // 캔버스 그룹의 blocksRaycasts를 기준으로 처리
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    /// <summary>
    /// 인벤토리 열기
    /// </summary>
    public void Open()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        OnInventoryOpen?.Invoke();
    }

    /// <summary>
    /// 인벤토리 닫기
    /// </summary>
    public void Close()
    {
        if (canvasGroup == null) return;

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        OnInventoryClose?.Invoke();
    }

    // 이벤트 시스템의 인터페이스 함수들 -------------------------------------------------------------

    public virtual void OnDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 드래그 시작시 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭일 때만 처리
        {
            if (TempItemSlotUI.TempSlotUI.IsEmpty())
            {
                startObj = eventData.pointerCurrentRaycast.gameObject;   // 드래그 시작한 위치에 있는 게임 오브젝트 가져오기
                if (startObj != null)
                {
                    ItemSlotUI slotUI = startObj.GetComponent<ItemSlotUI>();    // ItemSlotUI 컴포넌트 가져오기
                    if (slotUI != null && slotUI.ItemSlot.SlotItemData != null)
                    {
                        dragStartID = slotUI.ID;
                        inven.TempRemoveItem(dragStartID);   // 드래그 시작한 위치의 아이템을 TempSlot으로 옮김
                        TempItemSlotUI.TempSlotUI.Open();  // 드래그 시작할 때 TempSlot 열기
                        GameManager.instance.Detail.Close();         // 상세정보창 닫기
                        GameManager.instance.Detail.IsPause = true;  // 상세정보창 안열리게 하기
                    }
                }
            }
        }
    }

    /// <summary>
    /// 드래그가 끝났을 때 실행
    /// </summary>
    /// <param name="eventData"></param>
    public virtual void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left) // 좌클릭일 때만 처리
        {
            if (dragStartID != InvalideID)  // 드래그가 정상적으로 시작되었을 때만 처리
            {
                GameObject endObj = eventData.pointerCurrentRaycast.gameObject; // 드래그 끝난 위치에 있는 게임 오브젝트 가져오기
                ItemSlotUI fromslotUI = startObj.GetComponent<ItemSlotUI>();  // 아이템을 빼는곳
                if (endObj != null)
                {
                    // 드래그 끝난 위치에 게임 오브젝트가 있으면
                    ItemSlotUI toslotUI = endObj.GetComponent<ItemSlotUI>();  // 아이템을 보내는곳
                    if (toslotUI != null && fromslotUI != null && toslotUI != fromslotUI)
                    {
                        if (toslotUI.ItemSlot.SlotItemData == null)
                        {
                            if (toslotUI.slotType == SlotType.Box)
                            {
                                photonView.RPC("PunAddItem", RpcTarget.AllBuffered,
                                        ((BoxItemSlotUI)toslotUI).BoxID.ToString(),
                                        TempItemSlotUI.TempSlotUI.TempSlot.SlotItemData.id.ToString(),
                                        toslotUI.ID.ToString(),
                                        toslotUI.slotType.ToString());
                            }
                            else if (toslotUI.slotType == SlotType.NPC)
                            {
                                photonView.RPC("PunAddItem", RpcTarget.AllBuffered,
                                    ((NPCItemSlotUI)toslotUI).NPCID.ToString(),
                                    TempItemSlotUI.TempSlotUI.TempSlot.SlotItemData.id.ToString(),
                                    toslotUI.ID.ToString(),
                                    toslotUI.slotType.ToString());

                            }
                            else if (toslotUI.slotType == SlotType.WeaponBox)
                            {
                                photonView.RPC("PunAddItem", RpcTarget.AllBuffered,
                                    ((WeaponBoxItemSlotUI)toslotUI).BoxID.ToString(),
                                    TempItemSlotUI.TempSlotUI.TempSlot.SlotItemData.id.ToString(),
                                    toslotUI.ID.ToString(),
                                    toslotUI.slotType.ToString());
                            }
                        }
                        else
                        {
                            inven.MoveItem(TempItemSlotUI.TempSlotUI, fromslotUI);
                        }
                            
                        TempItemSlotUI.TempSlotUI.ItemSlot.ClearSlotItem();
                        toslotUI.Refresh();
                        fromslotUI.Refresh();
                    }
                    else if (toslotUI != null && fromslotUI != null && toslotUI == fromslotUI)
                    {
                        TempItemSlotUI.TempSlotUI.Drop();
                    }
                    GameManager.instance.Detail.Open(toslotUI.ItemSlot.SlotItemData);      // 상세정보창 열기                        
                }
                else
                {
                    TempItemSlotUI.TempSlotUI.Drop();
                }
                dragStartID = InvalideID;                       // 드래그 시작 id를 될 수 없는 값으로 설정(드래그가 끝났음을 표시)
                GameManager.instance.Detail.IsPause = false;                         // 상세정보창 다시 열릴 수 있게 하기
            }

            TempItemSlotUI.TempSlotUI.Close(); // 드래그를 끝내고 tempSlot이 비어지면 닫기
           
            startObj = null;
        }
    }

    /// <summary>
    /// RPC를 통해 박스에 아이템을 추가
    /// </summary>
    /// <param name="num">구분 번호</param>
    /// <param name="itemId">아이템 아이디</param>
    /// <param name="slotIndex">슬롯 위치</param>
    [PunRPC]
    protected virtual void PunAddItem(string num, string itemId, string slotIndex, string type)
    {
        if (SlotType.Box.ToString() == type)
        {
            BoxManager.instance.Boxs[int.Parse(num)].BoxInvenUI.inven.AddItem(uint.Parse(itemId), uint.Parse(slotIndex));
        }
        else if (SlotType.NPC.ToString() == type)
        {
            BoxManager.instance.NPCs[int.Parse(num)].NPCInvenUI.inven.AddItem(uint.Parse(itemId), uint.Parse(slotIndex));
        }
        else if(SlotType.WeaponBox.ToString() == type)
        {
            BoxManager.instance.Weaponboxs[int.Parse(num)].WeaponBoxInvenUI.inven.AddItem(uint.Parse(itemId), uint.Parse(slotIndex));
        }
    }

    /// <summary>
    /// RPC를 통해 박스에 있는 아이템을 제거
    /// </summary>
    /// <param name="num">구분 번호</param>
    /// <param name="slotIndex">슬롯 위치</param>
    [PunRPC]
    protected virtual void PunRemoveItem(string num, string slotIndex, string type)
    {
        if (SlotType.Box.ToString() == type)
        {
            BoxManager.instance.Boxs[int.Parse(num)].BoxInvenUI.inven.RemoveItem(uint.Parse(slotIndex));
        }
        else if (SlotType.NPC.ToString() == type)
        {
            BoxManager.instance.NPCs[int.Parse(num)].NPCInvenUI.inven.RemoveItem(uint.Parse(slotIndex));
        }
        else if (SlotType.WeaponBox.ToString() == type)
        {
            BoxManager.instance.Weaponboxs[int.Parse(num)].WeaponBoxInvenUI.inven.RemoveItem(uint.Parse(slotIndex));
        }
    }

    public ItemSlotUI GetEmptySlot()
    {
        foreach (var slotUI in slotUIs)
        {
            if (slotUI.ItemSlot.SlotItemData == null)
                return slotUI;
        }

        return null;
    }

    public ItemSlotUI GetSlot()
    {
        return slotUIs[0];
    }

    public void Drop()
    {
        ItemSlotUI slot = GetSlot();

        if (!slot.ItemSlot.IsEmpty())
        {
            Vector3 pos = NetworkManager.instance.MyPlayer.OnItemDropPosition(NetworkManager.instance.MyPlayer.transform.position);
            PhotonNetwork.Instantiate(slot.ItemSlot.SlotItemData.prefab.name, pos, Quaternion.identity);
            slot.ItemSlot.ClearSlotItem();
        }
    }
}
