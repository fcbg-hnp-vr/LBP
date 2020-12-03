using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class Settings : MonoBehaviour
{
    public static Settings instance;
    //public List<Block> myBlocks;
    //public string outputDataPath;
    public TMP_InputField skeleton_id;
    public Toggle man;
    public Toggle woman;
    public bool isMan;
    public string markerId;
    public string participantId;
    public bool recordFirstView;
    public bool recordThirdView;
    public Toggle videoThirdView;
    public Toggle videoFirstView;
    public TMP_InputField zMarkerPosition;
    public TMP_InputField inputParticipantId;

    public string ParticipantId
    {
        get { return participantId; }
        set { participantId = value; }
    }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        man.onValueChanged.AddListener(delegate { ChangeAvatar(); });
        woman.onValueChanged.AddListener(delegate { ChangeAvatar(); });

        inputParticipantId.onValueChanged.AddListener(delegate { SetIdParticipant(); });

        videoThirdView.onValueChanged.AddListener(delegate { SetRecordThirdView(); });
        videoFirstView.onValueChanged.AddListener(delegate { SetRecordFirstView(); });

        SetRecordThirdView();
        SetRecordFirstView();
        ChangeAvatar();
    }

    private void Start()
    {
        StartCoroutine(CreateProcessOBS());
    }

    private IEnumerator CreateProcessOBS()
    {
        OBSRecorder.Instance.StartCoroutine(OBSRecorder.Instance.CreateProcessFirstView());

        yield return new WaitForSeconds(3);

        OBSRecorder.Instance.StartCoroutine(OBSRecorder.Instance.CreateProcessThirdView());

        yield return new WaitForSeconds(2);

        GetComponent<SceneManagement>().mainLoader.SetActive(false);
    }

    private void ChangeAvatar()
    {
        if (man.isOn)
            isMan = true;
        else if (woman.isOn)
            isMan = false;
    }

    public void SetMarkerID(Toggle _toggle, string _name)
    {
        if (_toggle.isOn)
            markerId = _name;

    }

    private void SetIdParticipant()
    {
        participantId = inputParticipantId.text;
    }

    private void SetRecordFirstView()
    {
        recordFirstView = videoFirstView.isOn;
    }

    private void SetRecordThirdView()
    {
        recordThirdView = videoThirdView.isOn;
    }
}
