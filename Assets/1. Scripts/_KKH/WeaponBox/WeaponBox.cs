using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponBox : MonoBehaviour
{
    public int boxNum = 0;

    private Inventory inven;

    private WeaponBoxUI weaponBoxInvenUI;
    public WeaponBoxUI WeaponBoxInvenUI => weaponBoxInvenUI;

    private SpriteRenderer weaponBoxRenderer;

    public GameObject useImageGameObj;

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
    /// RPC를 통해 박스에 아이템을 추가
    /// </summary>
    /// <param name="itemIDCode">추가할 아이템 아이디</param>
    [PunRPC]
    private void PunAddItem(ItemIDCode itemIDCode)
    {
        inven.AddItem(itemIDCode);
    }

    public void OnSteal()
    {
        pv.RPC("PunOnSteal", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PunOnSteal()
    {
        isOpen = true;
        weaponBoxRenderer.sprite = sprites[0];

        SpriteRenderer[] images = useImageGameObj.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer image in images)
        {
            Color color = image.color;
            color.a = 1;
            image.color = color;
        }
    }
}
