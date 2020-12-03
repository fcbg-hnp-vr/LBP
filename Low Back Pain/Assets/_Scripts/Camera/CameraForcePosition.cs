using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class CameraForcePosition : MonoBehaviour
{
    public GameObject HMDHeadPosition;

    void Awake()
    {
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        this.transform.position = HMDHeadPosition.transform.position;
    }

    void Update()
    {
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        this.transform.position = HMDHeadPosition.transform.position;
    }

    void LateUpdate()
    {
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        this.transform.position = HMDHeadPosition.transform.position;
    }

}
