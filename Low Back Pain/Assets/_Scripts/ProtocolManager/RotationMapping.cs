using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMapping : MonoBehaviour
{
    public Transform spineAvatar;
    public Transform spineTargetAvatar;

    public Transform rightShoulderAvatar;
    public Transform rightShoulderTargetAvatar;
    public Transform leftShoulderAvatar;
    public Transform leftShoulderTargetAvatar;

    public float speed;

    public Transform from, to, result;
    public float angle;
    public Material materialMarker;

    float distFromTo;

    void OnEnable()
    {
        if(Manager.instance.TargetMarker != null)
        {
            Destroy(Manager.instance.TargetMarker);
        }

        result = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
        result.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
        result.transform.gameObject.layer = 20;
        result.GetComponent<Renderer>().material = materialMarker;

        Manager.instance.TargetMarker = result.gameObject;
    }


    void LateUpdate()
    {
        
        if (Settings.instance != null)
        {
            if (from == null)
                GetReferences();
            if (to == null)
                GetReferences();
            if (Settings.instance.markerId != null && Settings.instance.markerId != "")
            {
                if (result != null)
                {
                    if(result.transform.parent!=null && result.transform.parent.name != (Settings.instance.markerId))
                    {
                        GetReferences();
                        ChangeMarkerTargetParent();
                    }
                }
            }

            RotationMappingAvatar();

            if (from != null && to != null)
                RotationMappingMarker();
        }
    }

    void RotationMappingMarker()
    {
        
        angle = spineTargetAvatar.localEulerAngles.x - spineAvatar.localEulerAngles.x;

        //Distance point from to point to
        distFromTo = Vector3.Distance(from.transform.position, to.transform.position);
        //distFromResult = distFromTo;

        //point from
        from.LookAt(to);
        Vector3 eurlerAngle = new Vector3(angle, 0, 0);
        from.Rotate(eurlerAngle);

        //point result
        result.position = from.position + from.forward * distFromTo;

        //Point to
        to.LookAt(result);

        Debug.DrawLine(to.position, to.forward * 1000f, Color.blue);
        Debug.DrawLine(from.position, from.forward * 1000f, Color.white);
        Debug.DrawLine(from.position, to.position, Color.red);
    }

    void RotationMappingAvatar()
    {
        if (Manager.instance.IsStartTask && Manager.instance.IsStartTrialTask)
        {
            //Increas rotation (only on flexion - rotation sup >0)
            if ((spineAvatar.localEulerAngles.x >= 0 && (spineAvatar.localEulerAngles.x * Manager.instance.ScalingFactor < 100)))
            {
                //modifying the Vector3, based on input multiplied by speed and time
                Vector3 currentEulerAngles = new Vector3(spineTargetAvatar.localEulerAngles.x * Manager.instance.ScalingFactor, spineTargetAvatar.localEulerAngles.y, spineTargetAvatar.localEulerAngles.z);

                spineTargetAvatar.transform.localEulerAngles = currentEulerAngles;

                if (spineAvatar.localEulerAngles.x >= 20)
                {
                    Vector3 currentEulerAnglesrightShoulder = new Vector3(rightShoulderTargetAvatar.eulerAngles.x , rightShoulderTargetAvatar.eulerAngles.y, rightShoulderTargetAvatar.eulerAngles.z);
                    currentEulerAnglesrightShoulder += new Vector3(angle, 0, 0);
                    rightShoulderTargetAvatar.transform.eulerAngles = currentEulerAnglesrightShoulder;

                    Vector3 currentEulerAnglesleftShoulder = new Vector3(leftShoulderTargetAvatar.eulerAngles.x , leftShoulderTargetAvatar.eulerAngles.y, leftShoulderTargetAvatar.eulerAngles.z);
                    currentEulerAnglesleftShoulder += new Vector3(angle, 0, 0);
                    leftShoulderTargetAvatar.transform.eulerAngles = currentEulerAnglesleftShoulder;
                }
            }
        }
    }

    void GetReferences()
    {
        if (GameObject.Find("H_T10") != null)
            from = GameObject.Find("H_T10").transform;

        if (Settings.instance.markerId != null && GameObject.Find(Settings.instance.markerId) != null)
            to = GameObject.Find(Settings.instance.markerId).transform;

        if (result != null)
        {
            result.transform.SetParent(to);
            result.transform.localPosition = Vector3.zero;
            result.transform.localRotation = Quaternion.identity;
        }
    }

    void ChangeMarkerTargetParent()
    {
        result.transform.SetParent(to);
        result.transform.localPosition = Vector3.zero;
        result.transform.localRotation = Quaternion.identity;
    }

    public static float GetVectorInternalAngle(Vector3 a, Vector3 b, Vector3 c)
    {
        return Vector3.Angle(b - a, c - a);
    }

    public float MapValue(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
