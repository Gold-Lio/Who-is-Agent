using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class IDScanner : MonoBehaviour
{
    public int scannerNum = 0;

    private Inventory inven;

    private IDScannerUI idScannerUI;
    public IDScannerUI IdScannerUI => idScannerUI;

    private PhotonView pv = null;

    public int ItemID = 999999;

    public const int scannerSize = 1;

    private GameObject ScannerInteractGameObj;

    private void Awake()
    {
        idScannerUI = transform.GetComponentInChildren<IDScannerUI>();
        pv = GetComponent<PhotonView>();
        ScannerInteractGameObj = transform.Find("ScannerInteract").gameObject;
    }

    void Start()
    {
        inven = new Inventory(SlotType.IDScanner, scannerSize);
        idScannerUI.InitializeInventory(inven, scannerNum);
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
        interactionUI = ScannerInteractGameObj.transform.GetChild(0).gameObject;
        interactionUI.SetActive(onoff);
    }
}
