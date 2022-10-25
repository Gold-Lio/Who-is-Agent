using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAnimations : MonoBehaviourPun
{
    PhotonView PV;

    public static PlayerAnimations instance;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        PV = photonView;
    }

    //�÷��̾��� �ø� ����.   
    [PunRPC]
    public void RotateToPointer(Vector2 lookDirection)
    {
        Vector3 scale = transform.localScale;
        if (lookDirection.x > 0)
        {
            scale.x = 1;
        }
        else if (lookDirection.x < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
    }

    public void PlayRunning(Vector2 movementInput)
    {
        animator.SetBool("Running", movementInput.magnitude > 0);
    }

    [PunRPC]
    public void Die()
    {
        animator.SetTrigger("Die");
        Player.instance.isMove = false;
        Player.instance.maxSpeed = 0;
        PV.RPC("Die", RpcTarget.Others);
        //�̰� ���� �� ���� ���濡�Լ��� ���� ���� �� �̰��� ��ü�����ϰ�
        //
        //��� �ൿ �Ұ� �ϵ��� ����.
    }
}
