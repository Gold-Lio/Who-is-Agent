using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCIDCard : MonoBehaviour
{
    private Image idCardImage;
    public int npcID = 0;

    private void Awake()
    {
        idCardImage = GetComponent<Image>();
    }

    private void Start()
    {
        idCardImage.sprite = IDCardManager.instance.npcImage[npcID];
        Close();
    }

    public void Close()
    {
        if (idCardImage == null) return;

        Color color = idCardImage.color;
        color.a = 0.0f;
        idCardImage.color = color;
    }

    public void Open()
    {
        if (idCardImage == null) return;

        Color color = idCardImage.color;
        color.a = 1.0f;
        color = Color.white;
        idCardImage.color = color;
    }

    public void Initialize(int id)
    {
        npcID = id;
    }
}
