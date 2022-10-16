using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Bell : MonoBehaviour
{
    private TextMeshProUGUI messageText;
    private bool isMuster = false;
    private PhotonView pv;

    private void Awake()
    {
        messageText = GameObject.Find("All_Message").GetComponent<TextMeshProUGUI>();
        pv = GetComponent<PhotonView>();
    }

    public void Muster(string playerName)
    {
        if(!isMuster)
            pv.RPC("PunMuster", RpcTarget.AllBuffered, playerName);
    }

    [PunRPC]
    private void PunMuster(string playerName)
    {
        isMuster = true;
        if (messageText != null)
        {
            StartCoroutine(MusterCoolTime());
            StartCoroutine(MusterMessage(playerName));
        }
    }

    private IEnumerator MusterMessage(string playerName)
    {
        messageText.text = $"{playerName}(이)가 플레이어들을 아지트로 호출하였습니다.";
        yield return new WaitForSeconds(3.0f);

        messageText.text = "";
    }

    private IEnumerator MusterCoolTime()
    {
        yield return new WaitForSeconds(30.0f);
        isMuster = false;
    }
}
