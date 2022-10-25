using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;


public class KnockbackFeedback : MonoBehaviourPun, IPunObservable
{
    PhotonView PV;
    Vector3 curPos;
    [SerializeField]
    private Rigidbody2D rb2d;

    [SerializeField]
    private float strength = 16, delay = 0.15f;

    public UnityEvent OnBegin, OnDone;

    private void Start()
    {
        PV = photonView;
    }

    public void PlayFeedback(GameObject sender)
    {
        if (PV.IsMine)
        {
            StopAllCoroutines();
            OnBegin?.Invoke();
            Vector2 direction = (transform.position - sender.transform.position).normalized;
            rb2d.AddForce(direction * strength, ForceMode2D.Impulse);
            StartCoroutine(Reset());
        }
        ////그 외의 것들은 부드럽게 위치 동기화
        //else if ((transform.position - curPos).sqrMagnitude >= 100) transform.position = curPos;
        //else transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * 10);
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(delay);
        rb2d.velocity = Vector3.zero;
        OnDone?.Invoke();
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
        }
    }

}
