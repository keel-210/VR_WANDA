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
    }
}
