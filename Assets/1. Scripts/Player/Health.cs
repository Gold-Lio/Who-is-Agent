using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;
using static NetworkManager;


public class Health : MonoBehaviourPun
{
    PhotonView PV;

    [SerializeField]
    private float currentHealth, maxHealth;
    public float CurrentHealth => currentHealth;

    public UnityEvent<GameObject> OnHitWithReference, OnDeathWithReference;

    [SerializeField]
    private bool isDead = false;

    PlayerAnimations playerAnimations;

    private void Start()
    {
        PV = photonView;
        playerAnimations = GetComponent<PlayerAnimations>();
    }

    public void InitializeHealth(int healthValue)
    {
        currentHealth = healthValue;
        maxHealth = healthValue;    
        UIManager.UM.HP_Slider.value = maxHealth;
        isDead = false;
    }

    [PunRPC]
    public void GetHit(float amount, GameObject sender)
    {
        if (isDead)
            return;
       
        currentHealth -= amount;

        if (currentHealth > 0)
        {
            OnHitWithReference?.Invoke(sender);            
        }
        else
        {
            OnDeathWithReference?.Invoke(sender);
            isDead = true;
            //Destroy(gameObject); // 이것이 전체로 공유되도록 변경

            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);// 플레이어를 없애고. 
            }
            Destroy(gameObject);    // 혹시 몰라서 한번더 삭제 해준다.
        }
    }

}
