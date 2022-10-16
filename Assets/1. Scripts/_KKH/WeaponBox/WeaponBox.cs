using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBox : MonoBehaviour
{
    public int boxNum = 0;

    private Inventory inven;

    private WeaponBoxUI weaponBoxInvenUI;
    public WeaponBoxUI WeaponBoxInvenUI => weaponBoxInvenUI;

    private SpriteRenderer weaponBoxRenderer;

    private PhotonView pv = null;
    
    public ItemIDCode ItemID;

    public Sprite[] sprites;

    private bool isOpen = false;
    public bool IsOpen => isOpen;

    private void Awake()
    {
        weaponBoxInvenUI = transform.GetComponentInChildren<WeaponBoxUI>();
        weaponBoxRenderer = GetComponent<SpriteRenderer>();
        pv = GetComponent<PhotonView>();
    }

    private void Start()
    {
        inven = new Inventory(SlotType.Box, 3);
        weaponBoxInvenUI.InitializeInventory(inven, boxNum);

        if (PhotonNetwork.IsMasterClient)
        {
            pv.RPC("PunAddItem", RpcTarget.AllBuffered, ItemID);
        }
    }

    /// <summary>
    /// RPC�� ���� �ڽ��� �������� �߰�
    /// </summary>
    /// <param name="itemIDCode">�߰��� ������ ���̵�</param>
    [PunRPC]
    private void PunAddItem(ItemIDCode itemIDCode)
    {
        inven.AddItem(itemIDCode);
    }

    public void OnOpen()
    {
        isOpen = true;
        weaponBoxRenderer.sprite = sprites[0];
    }
}
