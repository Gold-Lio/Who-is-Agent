using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private TextMeshProUGUI messageText;

    private void Awake()
    {
        messageText = GameObject.Find("All_Message").GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator Message(string message)
    {
        messageText.text = message;
        yield return new WaitForSeconds(3.0f);
        messageText.text = "";
    }
}
