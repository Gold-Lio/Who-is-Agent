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
            //Destroy(gameObject); // �̰��� ��ü�� �����ǵ��� ����

            PhotonNetwork.Destroy(gameObject);// �÷��̾ ���ְ�. 
        }
    }

}
