﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] Transform HMDTransform;
    [SerializeField] HandController RightDevice, LeftDevice;
    [SerializeField] float WalkSpeed;
    Vector3 offset, RightPosCash, LeftPosCash, LocalGrippingPos;
    Collider collider;
    bool OldRightGrippingState, OldLeftGrippingState, IsGripping;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        RightPosCash = RightDevice.transform.position;
        LeftPosCash = LeftDevice.transform.position;
    }
    void Update()
    {
        if (RightDevice.IsHandGripping && LeftDevice.IsHandGripping)
        {
            DowbleGrip();
            Grip();
        }
        else if (RightDevice.IsHandGripping || LeftDevice.IsHandGripping)
        {
            if (RightDevice.IsHandGripping)
            {
                SingleGrip(RightDevice);
                Grip();
            }
            else
            {
                SingleGrip(LeftDevice);
                Grip();
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
        OldRightGrippingState = RightDevice.IsHandGripping;
        OldLeftGrippingState = LeftDevice.IsHandGripping;
    }
    void Grip()
    {
        if (!IsGripping)
        {
            IsGripping = true;
            LocalGrippingPos = transform.localPosition;
        }
        rigidbody.useGravity = false;
        collider.isTrigger = true;
        transform.localPosition = LocalGrippingPos;
        transform.localPosition += new Vector3(0.001f,0,0);
        LocalGrippingPos = transform.localPosition;
    }
    void DowbleGrip()
    {
        transform.parent = RightDevice.GrippingObject.transform;
        Vector3 RightOffset = RightDevice.transform.position - RightDevice.GripPosition;
        Vector3 LeftOffset = LeftDevice.transform.position - LeftDevice.GripPosition;
        offset = (RightOffset + LeftOffset) / 2;
    }
    void SingleGrip(HandController device)
    {
        transform.parent = device.GrippingObject.transform;
        offset = device.transform.position - device.GripPosition;
    }
    void Fall()
    {
        transform.parent = null;
        rigidbody.rotation = Quaternion.Euler(0, 0, 0);
        rigidbody.useGravity = true;
        collider.isTrigger = false;
        IsGripping = false;
    }
    void Walk()
    {
        rigidbody.useGravity = true;
        collider.isTrigger = false;
        float DifPosRight = (RightPosCash - RightDevice.transform.position).magnitude;
        float DifPosLeft = (LeftPosCash - LeftDevice.transform.position).magnitude;
        float AveVeloY = Mathf.Clamp((DifPosRight + DifPosLeft) / (2 * Time.deltaTime), 0, 3);
        Vector3 forward = new Vector3(HMDTransform.forward.x, 0, HMDTransform.forward.z);
        rigidbody.velocity = forward * WalkSpeed * AveVeloY;
        RightPosCash = RightDevice.transform.position;
        LeftPosCash = LeftDevice.transform.position;
    }
}