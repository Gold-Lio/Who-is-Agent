using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerAnimations : MonoBehaviourPun
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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

    public void Die()
    {
        animator.SetBool("Die", true);
    }
}
