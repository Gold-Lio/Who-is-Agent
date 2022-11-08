using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatterEntity : MonoBehaviourPunCallbacks
{
    PhotonView PV;
    public static ChatterEntity Local { get; private set; }
    [Header("Chat Bubble")]
    public float chatBubbleVisibleDuration = 2f;
    public GameObject chatBubbleRoot;
    public Text chatBubbleText;

    private float lastShowChatBubbleTime;

    private void Start()
    {
        if (photonView.IsMine)
            Local = this;

        PV = photonView;
    }

    private void Awake()
    {
        if (chatBubbleRoot != null)
            chatBubbleRoot.SetActive(false);
    }

    private void Update()
    {
        if (Time.realtimeSinceStartup - lastShowChatBubbleTime >= chatBubbleVisibleDuration)
        {
            if (chatBubbleRoot != null)
                chatBubbleRoot.SetActive(false);
        }
    }

    public void CmdSendChat(string message)
    {
        PV.RPC("RpcShowChat", RpcTarget.All, message);
    }

    [PunRPC]
    public void RpcShowChat(string message)
    {
        if (chatBubbleText != null)
            chatBubbleText.text = message;

        if (chatBubbleRoot != null)
            chatBubbleRoot.SetActive(true);

        lastShowChatBubbleTime = Time.realtimeSinceStartup;
    }
}
