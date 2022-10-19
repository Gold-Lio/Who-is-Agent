using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
//using POpusCodec.Enums;

public class NPC : MonoBehaviourPun
{
    public int npcNum = 0;

    private Inventory inven;

    private NPCUI npcInvenUI;
    public NPCUI NPCInvenUI => npcInvenUI;

    private bool isDead = false;
    public bool IsDead => isDead;

    private bool isAgent = false;

    private ItemIDCode[] invenItemCode;

    private TextMeshProUGUI messageText;

    private void Awake()
    {
        messageText = GameObject.Find("All_Message").GetComponent<TextMeshProUGUI>();
        npcInvenUI = transform.GetComponentInChildren<NPCUI>();
    }

    void Start()
    {
        inven = new Inventory(SlotType.NPC, 3);
        npcInvenUI.InitializeInventory(inven, npcNum);
        invenItemCode = new ItemIDCode[inven.SlotCount];

        if (PhotonNetwork.IsMasterClient)
        {
           // 아이템 생성 코드 넣자..
        }
    }

    public void SetAgent()
    {
        photonView.RPC("PunSetAgent", RpcTarget.AllBuffered);
    }

    [PunRPC]
    private void PunSetAgent()
    {
        isAgent = true;
    }

    public void NPCDead()
    {
        photonView.RPC("PunNPCDead", RpcTarget.AllBuffered, true);
    }

    [PunRPC]
    private void PunNPCDead(bool isdead)
    {
        isDead = isdead;
        if(!isAgent)
        {
            StartCoroutine(NPCDeadMessage());
        }
        Destroy(this.gameObject, 0.1f);
    }

    private IEnumerator NPCDeadMessage()
    {
        messageText.text = $"민간인{npcNum}가 죽었습니다.....";
        yield return new WaitForSeconds(3.0f);

        messageText.text = "";
    }
}
