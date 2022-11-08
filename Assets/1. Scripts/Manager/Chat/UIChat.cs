using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    public GameObject enterChatRoot;
    public InputField enterChatField;
    public KeyCode enterChatKey = KeyCode.Return;

    private bool enterChatFieldVisible;

    private void Update()
    {
        if (Input.GetKeyDown(enterChatKey)) //엔터ㅡ 근데 왜 엔터가 잘 안먹지? 흠.
        {
            SendChatMessage();
        }
    }

    public void ShowEnterChatField()
    {
        if (enterChatField != null)
        {
            enterChatField.Select();
            enterChatField.ActivateInputField();
        }
    }

    public void SendChatMessage()
    {
        if (ChatterEntity.Local == null || enterChatField == null)
            return;

        var trimText = enterChatField.text.Trim();
        if (trimText.Length == 0)
            return;

        enterChatField.text = "";
        ChatterEntity.Local.CmdSendChat(trimText);
    }
}
