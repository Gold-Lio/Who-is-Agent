using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxLoading : MonoBehaviour
{
    public float maxtime = 10.0f;
    private Transform fillTrans;
    private bool isOpen = false;
    private Vector3 scale = Vector3.one;

    private void Awake()
    {
        fillTrans = transform.Find("FillPivot").transform;
    }

    public void SetLoadingBar(float time)
    {
        if (fillTrans == null) return;
        float ratio = 1 - (time / maxtime);
        if (ratio < 0) ratio = 0;
        scale.x = ratio;
        fillTrans.localScale = scale;

        if(ratio <= 0 && isOpen == false)
        {
            isOpen = true;
        }
    }

    public void SetClear()
    {
        if (fillTrans == null) return;
        scale.x = 1;
        fillTrans.localScale = scale;

        if (isOpen == true)
        {
            isOpen = false;
        }
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }
}
