using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRotator : MonoBehaviour
{
    public float roatorSpeed = 360.0f;
    public float moveDistance = 1.0f;

    private float timeElapsed = 0.0f;
    private Vector3 startPosition;
    private float moveHalf = 0.0f;

    private void Start()
    {
        startPosition = transform.position;
        moveHalf = moveDistance * 0.5f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        transform.position = startPosition + moveHalf * new Vector3(0, 1 - Mathf.Cos(timeElapsed), 0);
        transform.Rotate(roatorSpeed * Time.deltaTime * Vector3.up);
    }
}
