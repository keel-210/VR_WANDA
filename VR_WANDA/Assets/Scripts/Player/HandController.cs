using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public bool IsHandGripping, IsWalking;
    public Vector3 GripPosition, ControllerVelocity;
    public bool IsTriggered;
    public GameObject GrippingObject;
    [SerializeField] SteamVR_TrackedObject HandDevice;
    [SerializeField] Renderer modelrend;
    [SerializeField] Animator animator;
    Rigidbody rb;
    void Start ()
    {
        HandDevice = GetComponent<SteamVR_TrackedObject> ();
        rb = GetComponent<Rigidbody> ();
    }
    void Update ()
    {
        var device = SteamVR_Controller.Input ((int) HandDevice.index);
        IsTriggered = device.GetPressDown (SteamVR_Controller.ButtonMask.Trigger);
        ControllerVelocity = rb.velocity;
        if (device.GetTouchUp (SteamVR_Controller.ButtonMask.Trigger))
        {
            IsTriggered = false;
            IsHandGripping = false;
            animator.SetBool ("Trigger", false);
            modelrend.material.color = Color.white;
        }
        if (!IsWalking && device.GetPressDown (SteamVR_Controller.ButtonMask.Grip))
        {
            modelrend.material.color = Color.green;
            IsWalking = true;
        }
        if (IsWalking && device.GetPressUp (SteamVR_Controller.ButtonMask.Grip))
        {
            IsWalking = false;
            modelrend.material.color = Color.white;
        }
        if (IsTriggered)
        {
            modelrend.material.color = Color.blue;
            animator.SetBool ("Trigger", true);
        }
    }

    void OnTriggerStay (Collider collider)
    {
        if (!IsHandGripping)
        {
            if (IsTriggered && collider.tag == "GripPoint")
            {
                IsHandGripping = true;
                GrippingObject = collider.gameObject;
                GripPosition = transform.position;
                modelrend.material.color = Color.red;
                animator.SetBool ("Trigger", true);
            }
        }
    }
}