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
        if (Input.GetKeyDown(enterChatKey))
        {
            SendChatMessage();
        }
    }

    public void ShowEnterChatField()
    {
        //if (enterChatRoot != null)
        //    enterChatRoot.SetActive(true);
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
