using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; // �ó׸ӽ� ���� �ڵ�
using Photon.Pun;

public class CameraSetup : MonoBehaviourPun
{
    public CinemachineVirtualCamera followCam;
    // �ó׸ӽ� ī�޶� ���� �÷��̾ �����ϵ��� ����
    void Start()
    {
        // ���� �ڽ��� ���� �÷��̾���
        if (photonView.IsMine)
        {
            // ���� �ִ� �ó׸ӽ� ���� ī�޶� ã��
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            // ���� ī�޶��� ���� ����� �ڽ��� Ʈ���������� ����
            followCam.Follow = transform;
            followCam.LookAt = transform;
        }
    }
}
