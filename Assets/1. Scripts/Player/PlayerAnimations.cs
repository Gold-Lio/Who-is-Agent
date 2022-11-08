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
        instance = this;
        animator = GetComponent<Animator>();
        PV = photonView;
    }


    [PunRPC]
    public void RotateToPointer(float lookDirection)
    {
        if (!PV.IsMine) return;

        Vector3 scale = transform.localScale;
        if (lookDirection > 0)
        {
            scale.x = 1;
        }
        else if (lookDirection < 0)
        {
            scale.x = -1;
        }
        transform.localScale = scale;
       // photonView.RPC("PunPlayerXFlip", RpcTarget.AllBuffered);
    }

    //플레이어의 플립 공유.   
    [PunRPC]
    private void PunPlayerXFlip(/*float lookDirection*/)
    {
        Debug.Log("x????");

    }

    [PunRPC]
    private void PunPlayerYFlip(/*float lookDirection*/)
    {
        Debug.Log("y????");
        //Vector3 scale = transform.localScale;
        //if (lookDirection > 0)
        //{
        //    scale.x = 1;
        //}
        //else if (lookDirection < 0)
        //{
        //    scale.x = -1;
        //}
        //transform.localScale = scale;
    }

    public void PlayRunning(Vector2 movementInput)
    {
        animator.SetBool("Running", movementInput.magnitude > 0);
    }

    [PunRPC]
    public void Die()
    {
        animator.SetTrigger("Die");
    }
}
