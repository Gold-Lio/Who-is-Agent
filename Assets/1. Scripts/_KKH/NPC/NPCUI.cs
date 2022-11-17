using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public class NPCUI : InventoryUI, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    /// 이 인벤토리가 가지고 있는 슬롯UI들
    private NPCItemSlotUI[] npcSlotUIs;
    public NPCItemSlotUI[] NPCSlotUIs => npcSlotUIs;

    // 박스 구분을 위한 ID
    private int npcId = 0;

    // 박스 구분을 위한 ID 프로퍼티
    public int NPCID => npcId;

    protected override void Start()
    {
        Close();
    }

    public void InitializeInventory(Inventory newInven, int npcid)
    {
        npcId = npcid;
        inven = newInven;   //즉시 할당
        if (Inventory.Default_Inventory_Size != inven.SlotCount)    // 기본 사이즈와 다르면 기본 슬롯UI 삭제
        {
            // 기존 슬롯UI 전부 삭제
            NPCItemSlotUI[] slots = GetComponentsInChildren<NPCItemSlotUI>();
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }

            // 새로 만들기
            npcSlotUIs = new NPCItemSlotUI[inven.SlotCount];
            for (int i = 0; i < inven.SlotCount; i++)
            {
                GameObject obj = Instantiate(slotPrefab, slotParent);
                obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                npcSlotUIs[i] = obj.GetComponent<NPCItemSlotUI>();
                npcSlotUIs[i].Initialize((uint)i, inven[i], SlotType.NPC, npcId);       // 각 슬롯UI들도 초기화
            }
        }
        else
        {
            // 크기가 같을 경우 슬롯UI들의 초기화만 진행
            npcSlotUIs = slotParent.GetComponentsInChildren<NPCItemSlotUI>();

            if (npcSlotUIs.Length == 0)
            {
                npcSlotUIs = new NPCItemSlotUI[inven.SlotCount];
                for (int i = 0; i < inven.SlotCount; i++)
                {
                    GameObject obj = Instantiate(slotPrefab, slotParent);
                    obj.name = $"{slotPrefab.name}_{i}";            // 이름 지어주고
                    npcSlotUIs[i] = obj.GetComponent<NPCItemSlotUI>();
                    npcSlotUIs[i].Initialize((uint)i, inven[i], SlotType.NPC, npcId);       // 각 슬롯UI들도 초기화
                }
            }
            else
            {
                for (int i = 0; i < inven.SlotCount; i++)
                {
                    npcSlotUIs[i].Initialize((uint)i, inven[i], SlotType.NPC, npcId);
                }
            }
        }

        RefreshAllSlots();  // 전체 슬롯UI 갱신
    }

    protected override void RefreshAllSlots()
    {
        foreach (var slotUI in npcSlotUIs)
        {
            slotUI.Refresh();
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
    }

    /// <summary>
    /// 드래그 시작시 실행
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
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
                        photonView.RPC("PunRemoveItem", RpcTarget.AllBuffered,
                        ((NPCItemSlotUI)slotUI).NPCID.ToString(),
                        slotUI.ID.ToString(),
                        slotUI.slotType.ToString());
                        GameManager.instance.Detail.Close();         // 상세정보창 닫기
                        GameManager.instance.Detail.IsPause = true;  // 상세정보창 안열리게 하기
                    }
                }
            }
        }
    }

    /// <summary>
    /// 드래그 끝
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnEndDrag(PointerEventData eventData)
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
                        if (toslotUI.slotType == SlotType.NPC)
                        {
                            photonView.RPC("PunAddItem", RpcTarget.AllBuffered,
                                    ((NPCItemSlotUI)toslotUI).NPCID.ToString(),
                                    TempItemSlotUI.TempSlotUI.TempSlot.SlotItemData.id.ToString(),
                                    toslotUI.ID.ToString(),
                                    toslotUI.slotType.ToString());
                        }
                        else
                        {
                            if (toslotUI.slotType == SlotType.Inventory)
                            {
                                if (toslotUI.ItemSlot.SlotItemData == null)     // 인벤토리 슬롯이 비어있을때
                                {
                                    ItemData_IDCard idcard = TempItemSlotUI.TempSlotUI.ItemSlot.SlotItemData as ItemData_IDCard;
                                    if (idcard != null)
                                    {
                                        IDCardManager.instance.npcIDCard[(int)idcard.NPCID].Open();
                                    }
                                    inven.MoveItem(TempItemSlotUI.TempSlotUI, toslotUI);
                                }
                                else
                                {
                                    inven.MoveItem(TempItemSlotUI.TempSlotUI, fromslotUI);
                                }
                            }
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
                else if (fromslotUI.slotType == SlotType.NPC)
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
    protected override void PunAddItem(string num, string itemId, string slotIndex, string type)
    {
        BoxManager.instance.NPCs[int.Parse(num)].NPCInvenUI.inven.AddItem(uint.Parse(itemId), uint.Parse(slotIndex));
    }

    /// <summary>
    /// RPC를 통해 박스에 있는 아이템을 제거
    /// </summary>
    /// <param name="num">구분 번호</param>
    /// <param name="slotIndex">슬롯 위치</param>
    [PunRPC]
    protected override void PunRemoveItem(string num, string slotIndex, string type)
    {
        BoxManager.instance.NPCs[int.Parse(num)].NPCInvenUI.inven.RemoveItem(uint.Parse(slotIndex));
    }
}
