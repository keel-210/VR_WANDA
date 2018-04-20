using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField]
    Transform HMDTransform;
    [SerializeField]
    HandController RightDevice, LeftDevice;
    [SerializeField]
    float WalkSpeed;
    Vector3 RightPosCash, LeftPosCash;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        RightPosCash = RightDevice.transform.position;
        LeftPosCash = LeftDevice.transform.position;
    }
    void Update()
    {
        if (RightDevice.IsHandGripping && LeftDevice.IsHandGripping)
        {
            DowbleGrip();
        }
        else if (RightDevice.IsHandGripping || LeftDevice.IsHandGripping)
        {
            if (RightDevice.IsHandGripping)
            {
                SingleGrip(RightDevice);
            }
            else
            {
                SingleGrip(LeftDevice);
            }
        }
        else
        {
            Fall();
            if (RightDevice.IsWalking && LeftDevice.IsWalking)
            {
                Walk();
            }
        }
    }
    void DowbleGrip()
    {
        rigidbody.useGravity = false;
        transform.parent = RightDevice.GrippingObject.transform;
        Vector3 RightOffset = RightDevice.transform.position - RightDevice.GripPosition;
        Vector3 LeftOffset = LeftDevice.transform.position - LeftDevice.GripPosition;
        Vector3 AveOffset = (RightOffset + LeftOffset) / 2;
        rigidbody.position = Vector3.Lerp(rigidbody.position, rigidbody.position - AveOffset, 0.1f);
    }
    void SingleGrip(HandController device)
    {
        rigidbody.useGravity = false;
        transform.parent = device.GrippingObject.transform;
        Vector3 offset = device.transform.position - device.GripPosition;
        rigidbody.position = Vector3.Lerp(rigidbody.position, rigidbody.position - offset, 0.1f);
    }
    void Fall()
    {
        transform.parent = null;
        rigidbody.rotation = Quaternion.Euler(0,0,0);
        rigidbody.useGravity = true;
    }
    void Walk()
    {
        rigidbody.useGravity = true;
        float DifPosRight = (RightPosCash - RightDevice.transform.position).magnitude;
        float DifPosLeft = (LeftPosCash - LeftDevice.transform.position).magnitude;
        float AveVeloY = Mathf.Clamp((DifPosRight + DifPosLeft) / (2 * Time.deltaTime), 0, 3);
        Vector3 forward = new Vector3(HMDTransform.forward.x, 0, HMDTransform.forward.z);
        Debug.Log("Walking : " + forward);
        rigidbody.velocity = forward * WalkSpeed * AveVeloY;
        RightPosCash = RightDevice.transform.position;
        LeftPosCash = LeftDevice.transform.position;
    }
}