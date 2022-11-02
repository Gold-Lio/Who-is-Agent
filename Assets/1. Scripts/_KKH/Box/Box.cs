using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Box : MonoBehaviour
{
    public int boxNum = 0;

    private Inventory inven;

    private BoxUI boxInvenUI;
    public BoxUI BoxInvenUI => boxInvenUI;

    private PhotonView pv = null;

    public int ItemID = 999999;

    private const int boxSize = 3;

    private GameObject BoxInteractGameObj;

    private void Awake()
    {
        boxInvenUI = transform.GetComponentInChildren<BoxUI>();
        pv = GetComponent<PhotonView>();
        BoxInteractGameObj = transform.Find("BoxInteract").gameObject;
    }

    void Start()
    {
        inven = new Inventory(SlotType.Box, boxSize);
        boxInvenUI.InitializeInventory(inven, boxNum);
    }

    public void CreateItem()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (ItemID == 999999) return;

        pv.RPC("PunAddItem", RpcTarget.AllBuffered, (ItemIDCode)ItemID);
    }

    /// <summary>
    /// RPC를 통해 박스에 아이템을 추가
    /// </summary>
    /// <param name="itemIDCode">추가할 아이템 아이디</param>
    [PunRPC]
    private void PunAddItem(ItemIDCode itemIDCode)
    {
        if (inven.AddItem(itemIDCode))
        {

        }
    }

    public void OnInteractionUI(bool onoff)
    {
        GameObject interactionUI = null;
        interactionUI = BoxInteractGameObj.transform.GetChild(0).gameObject;
        interactionUI.SetActive(onoff);
    }
}
