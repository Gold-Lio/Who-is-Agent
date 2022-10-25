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

    //플레이어의 플립 공유.   
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
        //이것 이하 ㅡ 현재 상대방에게서만 죽음 보임 ㅡ 이것을 전체공유하고
        //
        //모든 행동 불가 하도록 변경.
    }
}
