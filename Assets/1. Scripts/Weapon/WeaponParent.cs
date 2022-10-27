using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponParent : MonoBehaviourPun
{
    PhotonView PV;

    public static WeaponParent WP;

    public static WeaponParent instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<WeaponParent>();
                //m_instance.Initialize();
            }
            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static WeaponParent m_instance; // 싱글톤이 할당될 static 변수

    private Player player;
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    public bool attackBlocked;

    public bool IsAttacking { get; set; }

    public Transform circleOrigin;
    public float radius;



    private void Awake()
    {
    }
    private void Start()
    {
        PV = photonView;
    }

    public void ResetIsAttacking()
    {
        IsAttacking = false;
    }

    private void Update()
    {
        PV.RPC("WeaponFlip", RpcTarget.All);
        //if (IsAttacking)
        //    return;

        ////플레이어 Flip 이것을 조정해줘야함.     
        //Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        //transform.right = direction;


        ////무기의 Sclae Y 값 
        //Vector2 scale = transform.localScale;
        //if (direction.x < 0)
        //{
        //    scale.y = -1;
        //}
        //else if (direction.x > 0)
        //{
        //    scale.y = 1;
        //}

        //transform.localScale = scale;

        ////무기의 Rotaion Z 값의 실시간 SettingOrder
        //if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        //{
        //    weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        //}
    }

    [PunRPC]
    public void WeaponFlip()
    {
        if (IsAttacking)
            return;

        //플레이어 Flip 이것을 조정해줘야함.     
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;


        //무기의 Sclae Y 값 
        Vector2 scale = transform.localScale;
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        else if (direction.x > 0)
        {
            scale.y = 1;
        }

        transform.localScale = scale;

        //무기의 Rotaion Z 값의 실시간 SettingOrder
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
    }


    //어택에서 RPC로  ㅡ 그리고 player에서 RPC 실행. 
    [PunRPC]
    public void Attack()
    {
        //if (!photonView.IsMine) { return; }  

        if (attackBlocked)
            return;
        animator.SetTrigger("Attack");
        IsAttacking = true;
        attackBlocked = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(delay);
        attackBlocked = false;
    }

    //파란 기즈모를 그린다.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    //때림의 판정
    [PunRPC]
    public void DetectColliders()
    {
        foreach (Collider2D col in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Health health;
            if (health = col.GetComponent<Health>())
            {
                //health.GetComponent<PhotonView>().RPC("GetHit", RpcTarget.AllViaServer,1, transform.parent.gameObject);
                health.GetHit(1, transform.parent.gameObject);
                Player player = col.GetComponent<Player>();
                if(player != null)
                    player.HP -= 1;
            }
        }
    }
}
