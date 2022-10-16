using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailInfoUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI itemName;
    private TextMeshProUGUI itemPrice;
    private Image itemIcon;

    private ItemData itemData;

    public bool IsPause = false;    // 상태 정보창 열고 닫기 일시 정지(true면 열리지 않고, false면 열린다.)

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        itemName = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        itemIcon = transform.Find("Icon").GetComponent<Image>();
    }

    public void Open(ItemData data)
    {
        if (!IsPause)
        {
            itemData = data;
            Refresh();
            canvasGroup.alpha = 1;
        }
    }

    public void Close()
    {
        if (!IsPause)
        {
            itemData = null;
            canvasGroup.alpha = 0;
        }
    }

    private void Refresh()
    {
        if (itemData != null)
        {
            itemName.text = itemData.itemName;
            itemIcon.sprite = itemData.itemIcon;
        }
    }
}
