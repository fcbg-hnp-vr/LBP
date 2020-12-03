using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowQTMMarkers : MonoBehaviour
{
    public GameObject neck;
    public GameObject head;
    public GameObject rightArm;
    public GameObject rightForeArm;
    public GameObject rightHand;
    public GameObject spine;
    public GameObject spine1;
    public GameObject spine2;
    public GameObject leftArm;
    public GameObject leftHand;
    public GameObject leftForeArm;
    public GameObject leftUpLeg;
    public GameObject leftLeg;
    public GameObject leftFoot;
    public GameObject leftRoeBase;
    public GameObject leftToeEnd;
    public GameObject hips;
    public GameObject rightUpLeg;
    public GameObject rightLeg;
    public GameObject rightFoot;
    public GameObject rightToeBase;
    public GameObject rightToeEnd;

    public GameObject RFHD;
    public GameObject RSHO;
    public GameObject RELB;
    public GameObject RHAN;
    public GameObject L3;
    public GameObject T12;
    public GameObject T6;
    public GameObject LSHO;
    public GameObject LELB;
    public GameObject LHAN;
    public GameObject LGTR;
    public GameObject LKNE;
    public GameObject LANK;
    public GameObject LSMH;
    public GameObject LHAL;
    public GameObject S1;
    public GameObject RGTR;
    public GameObject RKNE;
    public GameObject RANK;
    public GameObject RSMH;
    public GameObject RHAL;

    private void OnEnable()
    {
        RFHD = GameObject.Find("RFHD");
        RSHO = GameObject.Find("RSHO");
        RELB = GameObject.Find("RELB");
        RHAN = GameObject.Find("RHAN");
        L3 = GameObject.Find("L3");
        T12 = GameObject.Find("T12");
        T6 = GameObject.Find("T6");
        LSHO = GameObject.Find("LSHO");
        LELB = GameObject.Find("LELB");
        LHAN = GameObject.Find("LHAN");
        LGTR = GameObject.Find("LGTR");
        LKNE = GameObject.Find("LKNE");
        LANK = GameObject.Find("LANK");
        LSMH = GameObject.Find("LSMH");
        LHAL = GameObject.Find("LHAL");
        S1 = GameObject.Find("S1");
        RGTR = GameObject.Find("RGTR");
        RKNE = GameObject.Find("RKNE");
        RANK = GameObject.Find("RANK");
        RSMH = GameObject.Find("RSMH");
        RHAL = GameObject.Find("RHAL");
    }
}
