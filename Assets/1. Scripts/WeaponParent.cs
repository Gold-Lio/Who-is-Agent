using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class WeaponParent : MonoBehaviourPun
{
    PhotonView PV;

    private Player player;
    public SpriteRenderer characterRenderer, weaponRenderer;
    public Vector2 PointerPosition { get; set; }

    public Animator animator;
    public float delay = 0.3f;
    private bool attackBlocked;

    public bool IsAttacking { get; private set; }

    public Transform circleOrigin;
    public float radius;

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
    public void DetectColliders()
    {
        if (!photonView.IsMine) { return; }

        foreach (Collider2D col in Physics2D.OverlapCircleAll(circleOrigin.position, radius))
        {
            Debug.Log(col.name);

            Health health;
            if (health = col.GetComponent<Health>())
            {
                health.GetHit(1, transform.parent.gameObject);
            }
        }
    }
}
