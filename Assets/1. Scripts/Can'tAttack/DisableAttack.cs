using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static NetworkManager;

public class DisableAttack : MonoBehaviourPun
{
    PhotonView PV;
    public GameObject infoText;

    private void Start()
    {
        PV = photonView;
        infoText.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.GetComponent<PhotonView>().IsMine)
        {
            NM.isAzit = true;
            if (NM.isAzit)
            {
                infoText.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && col.GetComponent<PhotonView>().IsMine)
        {
            NM.isAzit = false;
            if (!NM.isAzit)
            {
                infoText.SetActive(false);
            }
        }
    }
}
