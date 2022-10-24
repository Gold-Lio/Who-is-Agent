using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections;
using UnityEditor;
using static NetworkManager;
using static UIManager;

public class Player : MonoBehaviourPun, IDamageable
{
    private PhotonView PV;

    private Inventory inven;
    private InventoryUI playerInventoryUI;
    public InventoryUI playerInvenUI => playerInventoryUI;

    // 아이템 줍는 범위
    private const float itemPickupRange = 1.0f;
    // 아이템 버리는 범위
    private const float dropRange = 1.0f;

    private bool isBox = false;
    public bool IsBox => isBox;

    private bool isNpc = false;
    public bool IsNPC => isNpc;

    private bool isWeapon = false;
    public bool IsWeapon => isWeapon;

    private GameObject NPCInteractGameObj;

    private bool isWeaponOpenStart = false;
    private bool isWeaponOpen = false;
    private float weaponLoadingTime = 0.0f;

    private GameObject boxLoadingGameObj;
    private BoxLoading boxLoading;

    private bool isBell = false;
    private Bell bell;

    //private PlayerMover playerMover;
    private PlayerAnimations playerAnim;
    private WeaponParent weaponParent;

    private Vector2 pointerInput, movementInput;

    private GameObject destroyObj = null;   // 바닥에 버려진 아이템을 삭제하기 위해 만든 변수

    //플레이어 이름
    [HideInInspector] public string nick;
    public Text nickNameText;

    public Transform character, canvas;

    //플레이어 스피드
    [SerializeField]
    private float maxSpeed = 5, acceleration = 50, deacceleration = 100;
    [SerializeField]
    private float currentSpeed = 0;
    private Vector2 oldMovementInput;

    public int actor, colorIndex;
    public SpriteRenderer[] SR;
    private Rigidbody2D RB;

    public bool isSpy, isDie;

    [Header("PlayerHP")]
    public const float MAX_HP = 100;
    public float curHP = MAX_HP;
    public Image hpBar;


    /////변수의 프로퍼티화 
    public Vector2 PointerInput { get => pointerInput; set => pointerInput = value; }
    public Vector2 MovementInput { get => movementInput; set => movementInput = value; }


    private PlayerInput playerInputs;
    public PlayerInput PlayerInputs => playerInputs;

    private Box box;
    public Box Box => box;

    private NPC npc;
    public NPC NPC => npc;

    private WeaponBox weaponBox;
    public WeaponBox WeaponBox => weaponBox;

    public bool isConnect = true;

    private void Awake()
    {
        if (!photonView.IsMine) return;

        playerInputs = GetComponent<PlayerInput>();
        playerAnim = GetComponentInChildren<PlayerAnimations>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        RB = GetComponent<Rigidbody2D>();

        //playerMover = GetComponent<PlayerMover>();
        playerInventoryUI = GameObject.Find("InventoryUI").GetComponent<InventoryUI>();
        inven = new Inventory(SlotType.Inventory);
    }

    private void Start()
    {
        //if (!photonView.IsMine) return;

        PV = photonView;

        actor = PV.Owner.ActorNumber;
        nick = PV.Owner.NickName;
        SetNick();
        NM.Players.Add(this);
        NM.SortPlayers();

        StartCoroutine(InvenSetup());
    }

    //자기것의 인벤을 따로 실행시키는 InvenSetup 코루틴
    public IEnumerator InvenSetup()
    {
        if (photonView.IsMine)
        {
            playerInvenUI.InitializeInventory(inven);
        }
        yield return null;
    }


    private void Update()
    {
        if (!photonView.IsMine) return;

        //playerMover.MovementInput = MovementInput;

        PlayerMove();
        weaponParent.PointerPosition = pointerInput;
        AnimateCharacter();

        if (NM.isGameStart)
        {
            NM.light2D.transform.position = transform.position + new Vector3(0, 0, 10); // 빛 적용
        }
    }


    private void FixedUpdate()
    {
        if (isWeapon)
        {
            if (isWeaponOpen == false && isWeaponOpenStart == true)
            {
                if (boxLoading != null)
                {
                    weaponLoadingTime += Time.deltaTime;
                    boxLoading.SetLoadingBar(weaponLoadingTime);
                    if (boxLoading.GetIsOpen())
                    {
                        boxLoading.SetClear();
                        boxLoadingGameObj.SetActive(false);
                        BoxManager.instance.Weaponboxs[weaponBox.boxNum].WeaponBoxInvenUI.InventoryOnOffSwitch();
                        isWeaponOpen = true;
                    }
                }
            }
        }
    }

    private void OnDisable()
    {
        if (!photonView.IsMine) return;

        Debug.Log($"{actor}, {photonView.ViewID}, {PhotonNetwork.IsConnected}");
        LogManager.Log("OnDisable");
    }

    //플레이어에게 닉네임을 부여
    void SetNick()
    {
        nickNameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
    }


    //플레이어의 기본적인 움직임
    public void PlayerMove()
    {
        if (!photonView.IsMine) { return; }

        if (MovementInput.magnitude > 0 && currentSpeed >= 0)
        {
            oldMovementInput = MovementInput;
            currentSpeed += acceleration * maxSpeed * Time.deltaTime;
        }
        
        else
        {
            currentSpeed -= deacceleration * maxSpeed * Time.deltaTime;
        }

        currentSpeed = Mathf.Clamp(currentSpeed, 0, maxSpeed);
        RB.velocity = oldMovementInput * currentSpeed;
    }


    private void AnimateCharacter()
    {
        if (!photonView.IsMine) return;

        Vector2 lookDirection = pointerInput - (Vector2)transform.position;
        
        playerAnim.RotateToPointer(lookDirection); //바라보는 플립. 

        playerAnim.PlayRunning(MovementInput); // 움직애님 
    }


    #region 플레이어 Color
    //플레이어의 다양한 색깔 적용 + 손색깔까지
    [PunRPC]
    public void SetColor(int _colorIndex)
    {
        SR[0].color = UM.colors[_colorIndex];
        SR[1].color = UM.colors[_colorIndex];
        colorIndex = _colorIndex;
    }

    //스파이 닉네임 빨간색으로 변경.
    public void SetNickColor()
    {
        if (!isSpy) return;

        for (int i = 0; i < NM.Players.Count; i++)
        {
            if (NM.Players[i].isSpy) NM.Players[i].nickNameText.color = Color.red;
        }
    }
    #endregion

    public void SetPos(Vector3 target)
    {
        transform.position = target;
    }

    [PunRPC]
    void SetSpy(bool _isSpy)
    {
        isSpy = _isSpy;
    }

    public void PerformAttack()
    {
        if (!photonView.IsMine) return;

        weaponParent.Attack();
    }


    public void TakeDamage(float damage)
    {
        PV.RPC(nameof(RPC_TakeDamage), PV.Owner, damage);
    }

    [PunRPC]
    void RPC_TakeDamage(float damage, PhotonMessageInfo info)
    {
        curHP -= damage;

        hpBar.fillAmount = curHP / MAX_HP;

        if (curHP <= 0)
        {
            playerAnim.Die();
            //PlayerManager.Find(info.Sender).GetKill(); //플레이어매니저에게 일르러 간다.
            StartCoroutine(DieAfter());
        }
    }


    public IEnumerator DieAfter()
    {
        yield return new WaitForSeconds(7f);
        
        NM.GetCrewCount(); //사람을 세서 WinCheck

    }


    #region 인벤토리 관련 함수

    public void InventoryOnOff()
    {
        if (!photonView.IsMine) return;

        playerInvenUI.InventoryOnOffSwitch();
    }

    public void ItemPickUp()
    {
        if (!photonView.IsMine) return;
        if (playerInventoryUI.GetSlot().ItemSlot.SlotItemData != null) return;
        if (TempItemSlotUI.TempSlotUI.ItemSlot.SlotItemData != null) return;

        Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position, itemPickupRange, LayerMask.GetMask("Item"));
        photonView.RPC("GetDropItem", RpcTarget.AllBuffered, col.gameObject.name);

        if (destroyObj == null) return;

        Item item = destroyObj.GetComponent<Item>();

        if (inven.AddItem(item.data))
        {
            GameManager.instance.Detail.IsPause = false;
            if (destroyObj.GetPhotonView())
            {
                photonView.RPC("ItemDestory", RpcTarget.AllBuffered);
            }

            ItemData_IDCard idcard = item.data as ItemData_IDCard;
            if (idcard != null)
            {
                IDCardManager.instance.npcIDCard[(int)idcard.NPCID].Open();
            }
        }
    }

    public void ItemUse()
    {
        if (!photonView.IsMine) return;
        if (!isSpy) return;

        Collider2D col = Physics2D.OverlapCircle((Vector2)transform.position, itemPickupRange, LayerMask.GetMask("NPC"));
        if (col == null) return;
        playerInvenUI.GetSlot().ItemSlot.UseSlotItem(col.gameObject);
    }

    public Vector3 OnItemDropPosition(Vector3 inputPos)
    {
        Vector3 result = Vector3.zero;
        if (!photonView.IsMine) return result;

        Vector3 toInputPos = inputPos - transform.position;
        if (toInputPos.sqrMagnitude > dropRange * dropRange)
        {
            result = transform.position + toInputPos.normalized * dropRange;
        }
        else
        {
            result = inputPos;
        }

        return result;
    }

    public void Interaction()
    {
        if (!photonView.IsMine) return;
        if (isBox)
        {
            BoxManager.instance.Boxs[box.boxNum].BoxInvenUI.InventoryOnOffSwitch();
        }
        else if (isNpc)
        {
            BoxManager.instance.NPCs[npc.npcNum].NPCInvenUI.InventoryOnOffSwitch();
        }
        else if (isWeapon)
        {
            boxLoadingGameObj.SetActive(true);
            isWeaponOpenStart = true;
        }
        else if(isBell)
        {
            bell.Muster(nick);
        }
    }

    #endregion

    #region OnCollision Enter/Exit 이벤트 함수

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!photonView.IsMine) return;
        if (col.gameObject.CompareTag("Box"))
        {
            if (box != null) return;
            isBox = true;
            box = col.gameObject.GetComponent<Box>();
        }
        else if (col.gameObject.CompareTag("NPC"))
        {
            if (npc != null) return;
            isNpc = true;
            NPCInteractGameObj = col.transform.GetChild(1).gameObject;
            npc = col.gameObject.GetComponent<NPC>();
            NPCInteractGameObj.SetActive(true);
        }
        else if (col.gameObject.CompareTag("WeaponBox"))
        {
            if (weaponBox != null) return;
            isWeapon = true;
            weaponLoadingTime = 0.0f;
            weaponBox = col.gameObject.GetComponent<WeaponBox>();
            boxLoadingGameObj = col.transform.GetChild(1).gameObject;
            boxLoading = boxLoadingGameObj.GetComponent<BoxLoading>();
        }
        else if (col.gameObject.CompareTag("Bell"))
        {
            if (bell != null) return;
            isBell = true;
            bell = col.gameObject.GetComponent<Bell>();
        }
        //플레이어끼리 서로 겹치지 않도록 하는 Col - 나중에 맵 수정하면서 지워도 괜찮음. 
        if (!col.gameObject.CompareTag("Player")) return;
        Physics2D.IgnoreCollision(GetComponent<CapsuleCollider2D>(), col.gameObject.GetComponent<CapsuleCollider2D>());
    }

    private void OnCollisionExit2D(Collision2D col)
    {
        if (!photonView.IsMine) return;

        if (box != null)
        {
            isBox = false;
            BoxManager.instance.Boxs[box.boxNum].BoxInvenUI.Close();
            box = null;
        }
        else if (npc != null)
        {
            isNpc = false;
            BoxManager.instance.NPCs[npc.npcNum].NPCInvenUI.Close();
            NPCInteractGameObj.SetActive(false);
            npc = null;
            NPCInteractGameObj = null;
        }
        else if (weaponBox != null)
        {
            isWeapon = false;
            isWeaponOpenStart = false;
            isWeaponOpen = false;
            BoxManager.instance.Weaponboxs[weaponBox.boxNum].WeaponBoxInvenUI.Close();
            boxLoading.SetClear();
            boxLoadingGameObj.SetActive(false);
            weaponBox = null;
            boxLoading = null;
            boxLoadingGameObj = null;
        }
        else if (bell != null)
        {
            isBell = false;
            bell = null;
        }
    }
    #endregion

    #region Test 소스
    // 테스트 소스----------------------------------------------------------------
    //public void Test_ItemCreate()
    //{
    //    GameObject obj = PhotonNetwork.Instantiate(GameManager.instance.ItemData[ItemIDCode.Bag].name, transform.position, Quaternion.identity);
    //}

    //public void Test_ItemDestroy()
    //{
    //    photonView.RPC("GetDropItem", RpcTarget.AllBuffered);

    //    if (destroyObj != null)
    //    {
    //        Item item = destroyObj.GetComponent<Item>();
    //        GameManager.instance.Detail.IsPause = false;
    //        if (destroyObj.GetPhotonView())
    //        {
    //            photonView.RPC("ItemDestory", RpcTarget.AllBuffered);
    //        }
    //    }
    //}
    // --------------------------------------------------------------------------
    #endregion

    #region RPC 소스
    // RPC 소스------------------------------------------------------------------
    [PunRPC]
    public void GetDropItem(string objName)
    {
        GameObject obj = GameObject.Find(objName);
        if (obj != null)
        {
            destroyObj = obj.gameObject;
        }
        else
        {
            destroyObj = null;
        }
    }

    [PunRPC]
    public void ItemDestory()
    {
        PhotonNetwork.Destroy(destroyObj);
        Destroy(destroyObj);
        destroyObj = null;
    }
    // --------------------------------------------------------------------------
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, itemPickupRange);
    }
}
