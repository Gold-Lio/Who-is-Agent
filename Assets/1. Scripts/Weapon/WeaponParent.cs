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
            // ���� �̱��� ������ ���� ������Ʈ�� �Ҵ���� �ʾҴٸ�
            if (m_instance == null)
            {
                // ������ GameManager ������Ʈ�� ã�� �Ҵ�
                m_instance = FindObjectOfType<WeaponParent>();
                //m_instance.Initialize();
            }
            // �̱��� ������Ʈ�� ��ȯ
            return m_instance;
        }
    }

    private static WeaponParent m_instance; // �̱����� �Ҵ�� static ����

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

        ////�÷��̾� Flip �̰��� �����������.     
        //Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        //transform.right = direction;


        ////������ Sclae Y �� 
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

        ////������ Rotaion Z ���� �ǽð� SettingOrder
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

        //�÷��̾� Flip �̰��� �����������.     
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        transform.right = direction;


        //������ Sclae Y �� 
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

        //������ Rotaion Z ���� �ǽð� SettingOrder
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = characterRenderer.sortingOrder - 1;
        }
    }


    //���ÿ��� RPC��  �� �׸��� player���� RPC ����. 
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

    //�Ķ� ����� �׸���.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 position = circleOrigin == null ? Vector3.zero : circleOrigin.position;
        Gizmos.DrawWireSphere(position, radius);
    }

    //������ ����
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
