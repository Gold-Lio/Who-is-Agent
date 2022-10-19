using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static NetworkManager;

public class UIManager : MonoBehaviourPun
{
    PhotonView PV;

    public static UIManager UM
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<UIManager>();
            }
            return m_instance;
        }
    }

    private static UIManager m_instance; // �̱����� �Ҵ�� ����


    public Color[] colors;
    public Button startBtn;

    private void Awake()
    {
        if (UM != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PV = photonView;
    }

    void Update()
    {
        //if (!PhotonNetwork.InRoom) return;

        if (PhotonNetwork.IsMasterClient)
        {
            ShowStartBtn();
      
            if (NetworkManager.instance.isGameStart)
            {
                startBtn.gameObject.SetActive(false);
            }
        }
    }

    void ShowStartBtn()
    {
        startBtn.gameObject.SetActive(true);
        //startBtn.interactable = PhotonNetwork.CurrentRoom.PlayerCount >= 7; // �⺻��
        startBtn.interactable = PhotonNetwork.CurrentRoom.PlayerCount >= 1; // 2
    }

}
