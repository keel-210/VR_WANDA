using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPosMaintain : MonoBehaviour
{
    Vector3 localPos;
    void Start()
    {
        localPos = transform.localPosition;
    }
    void Update()
    {
        transform.localPosition = localPos;
        Debug.DrawLine(transform.position, transform.position + transform.up * 5, Color.green);
    }
}
