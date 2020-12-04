using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;
using QualisysRealTime.Unity;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    [Header("MAPPING")]
    private float scalingFactor = 0;

    [Header("Avatar")]
    public GameObject men;
    public GameObject women;
    public RTSkeleton menTargetAvatar;
    public RTSkeleton menAvatar;
    public RTSkeleton femaleTargetAvatar;
    public RTSkeleton femaleAvatar;
    public GameObject targetEyeFemaleAvatar;
    public GameObject targetEyeMaleAvatar;

    [Header("UI")]
    public Dropdown dropdown;
    public GameObject returnToInitialPosition;
    public TextMeshProUGUI infoBlock;
    public TextMeshProUGUI infoCommand;
    public GameObject loader;
    public GameObject scalingFactorInputPanel;
    public GameObject validatePanel;
    public TextMeshProUGUI nextTrialIndex;
    public TMP_InputField inputScalingFactor;
    public TextMeshProUGUI idParticipant;
    public Button quit;
    public Button startTrial;
    public Button nextTrial;
    bool inputStartTrial = false;
    bool inputNextTrial = false;

    [Header("SCREEN")]
    public VRStandardAssets.Utils.VRCameraFade fadeCameraVR;
    public GameObject panelInstruction;
    public GameObject screen;
    IEnumerator activeScreen;
    IEnumerator desactiveScreen;

    [Header("TASK")]  
    public GameObject targetPositionLine;
    public TextMeshProUGUI nextTrialTaskText;
    public Image imgLine;
    public GameObject targetMarker;
    private int currentTrialTask = 0;
    private bool isBellow = false;
    private bool startTask = false;
    private bool startTrialTask = false;
    private bool isWaiting = false;
    public int consecutiveTrial = 0;
    private bool taskIsValidated = false;
    private bool isStayBellowTheLine = false;
    IEnumerator waitingCoroutine;

    [Header("POSITIONS")]
    private Vector3 linePosition;
    private bool isGoBackToInitialPosition = false;

    bool startRecordingVideoThirdView = false;
    bool startRecordingVideoFirstView = false;

    [Header("Scene")]
    public GameObject room;
    public GameObject neutralRoom;
    int oldMask;


    public bool IsStartTask
    {
        get { return startTask; }
        set { startTask = value; }
    }

    public bool IsStartTrialTask
    {
        get { return startTrialTask; }
        set { startTrialTask = value; }
    }

    public float ScalingFactor
    {
        get { return scalingFactor; }
        set { scalingFactor = value; }
    }

    public bool IsBellow
    {
        get { return isBellow; }
        set { isBellow = value; }
    }

    public bool IsGoBack
    {
        get { return isGoBackToInitialPosition; }
        set { isGoBackToInitialPosition = value; }
    }

    public int TrialNumber
    {
        get { return currentTrialTask; }
        set { currentTrialTask = value; }
    }

    public bool IsWaiting
    {
        get { return isWaiting; }
        set { isWaiting = value; }
    }

    public bool IsTaskValidated
    {
        get { return taskIsValidated; }
        set { taskIsValidated = value; }
    }

    public bool IsStayBellowTheLine
    {
        get { return isStayBellowTheLine; }
        set { isStayBellowTheLine = value; }
    }

    public bool GetInputStartTrial
    {
        get { return inputStartTrial; }
    }

    public bool GetInputNextTrial
    {
        get { return inputNextTrial; }
    }

    public GameObject TargetMarker
    {
        get { return targetMarker; }
        set { targetMarker = value; }
    }

    public bool GetStatusRecordingFirstView
    {
        get { return startRecordingVideoFirstView; }
    }

    public bool GetStatusRecordingThirdView
    {
        get { return startRecordingVideoThirdView; }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Two instance of Manager is not possible!");
            Destroy(this);
        }
    }


    void Start()
    {
        oldMask = Camera.main.cullingMask;

        Random.InitState(42);

        //Coroutines
        waitingCoroutine = WaitingBellowTheLine();
        activeScreen = ActiveScreen();
        desactiveScreen = DesactiveScreen();

        //Callback
        quit.onClick.AddListener(delegate { QuitApplication(); });
        startTrial.onClick.AddListener(delegate { InputStartTrial(); });
        nextTrial.onClick.AddListener(delegate { InputNextTrial(); });
        nextTrial.gameObject.SetActive(false);
        Settings.instance.skeleton_id.onValueChanged.AddListener(delegate { ValueSkeletonIDChanged(); });

        //UI Command
        infoBlock.text = "Familiarisation with VR";
        //infoCommand.text = "Input 'N' key to start the task";


        if (Settings.instance != null)
        {
            if (Settings.instance.recordFirstView && WriteData.instance.path != null && WriteData.instance.path != "")
            {
                startRecordingVideoFirstView = true;
                OBSRecorder.Instance.StartRecordingFirstView();
            }
            if (Settings.instance.recordThirdView && WriteData.instance.path != null && WriteData.instance.path != "")
            {
                startRecordingVideoThirdView = true;
                OBSRecorder.Instance.StartRecordingThirdView();
            }
        }

        idParticipant.text = "ID : "+Settings.instance.inputParticipantId.text;
    }


    public void Update()
    {
       

        //Active men or women avatar
        if (Settings.instance.isMan)
        {
            if(Camera.main.GetComponent<CameraForcePosition>() != null)
                Camera.main.GetComponent<CameraForcePosition>().HMDHeadPosition = targetEyeMaleAvatar;

            men.SetActive(true);
            women.SetActive(false);

        }
        else if (Settings.instance)
        {
            if (Camera.main.GetComponent<CameraForcePosition>() != null)
                Camera.main.GetComponent<CameraForcePosition>().HMDHeadPosition = targetEyeFemaleAvatar;
           
            men.SetActive(false);
            women.SetActive(true);
        }


        //Skeleton ID
        menAvatar.SkeletonName = Settings.instance.skeleton_id.text;
        menTargetAvatar.SkeletonName = Settings.instance.skeleton_id.text;
        femaleAvatar.SkeletonName = Settings.instance.skeleton_id.text;
        femaleTargetAvatar.SkeletonName = Settings.instance.skeleton_id.text;

        //Marker position max
        linePosition = new Vector3(targetPositionLine.transform.position.x, float.Parse(Settings.instance.zMarkerPosition.text), targetPositionLine.transform.position.x);
        

        if(targetMarker != null)
        {
            //Start trial phase
            if ((GetInputStartTrial) && !IsStartTask)//START TASK BLOCK
            {
                IsStartTask = true;
                inputNextTrial = false;
                inputStartTrial = false;

                //Change culling mask
                oldMask = Camera.main.cullingMask;
                Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Avatar"));

                fadeCameraVR.m_FadeDuration = 0.5f;
                fadeCameraVR.FadeIn(false);

                //Change room
                room.SetActive(false);
                neutralRoom.SetActive(true);

                GoToNextTrial();
            }
            else if ((GetInputNextTrial) && IsStartTask && !IsStartTrialTask)//START NEXT TRIAL
            {
                StopAllCoroutines();
                inputNextTrial = false;
                inputStartTrial = false;
                nextTrial.gameObject.SetActive(true);
                startTrial.gameObject.SetActive(false);

                StartNextTrial();
            }
            else if ((GetInputNextTrial) && IsStartTask && IsStartTrialTask && consecutiveTrial == 2)//GO TO THE NEXT TRIAL | CONFIGURE IT BEFORE STARTING
            {
                inputNextTrial = false;
                inputStartTrial = false;

                nextTrial.gameObject.SetActive(false);
                startTrial.gameObject.SetActive(false);
                validatePanel.SetActive(true);

            }
            else if ((GetInputNextTrial) && IsStartTask && IsStartTrialTask && consecutiveTrial != 2)//GO TO THE SAME NEXT TRIAL
            {
                IsStartTrialTask = false;
                IsBellow = false;
                inputNextTrial = false;
                inputStartTrial = false;

                nextTrial.gameObject.SetActive(false);
                startTrial.gameObject.SetActive(false);
                validatePanel.SetActive(true);

            }

            if (IsStartTrialTask)
                Task();
        }
    }

   void InputStartTrial()
   {
        inputStartTrial = true;
   }

    void InputNextTrial()
    {
        inputNextTrial = true;
    }

    void Task()
    {
        if (!IsGoBack)
        {
            GoBellowTheLine();
        }
        else if (IsGoBack)//Going back to initial position
        {
            GoBack();
        }
    }

    void GoBack()
    {
        IsWaiting = false;

        if(targetMarker != null)
        {
            //Above the line = initial position
            if (targetMarker.transform.position.y >= imgLine.transform.position.y)
            {
                IsGoBack = false;

                imgLine.color = Color.green;
                imgLine.gameObject.SetActive(false);
                returnToInitialPosition.SetActive(false);

                IsStartTrialTask = false;
                isBellow = false;

                StopAllCoroutines();

                if(consecutiveTrial == 2)
                {

                    fadeCameraVR.m_FadeDuration = 1f;
                    fadeCameraVR.FadeOut(false);

                    consecutiveTrial = 0;
                    AddData();
                    GoToNextTrial();

                    StartCoroutine(StopRecording());

                }
                else
                {
                    AddData();
                    StartNextTrial();
                }
            }
        }
    }

    void GoBellowTheLine()
    {
        if(targetMarker != null)
        {
            //Below the line
            if (targetMarker.transform.position.y < imgLine.transform.position.y && IsBellow == false && IsWaiting == false)
            {
                IsBellow = true;

                //Change color line
                imgLine.color = new Color(255, 165, 0);//Yellow

                //Start wainting time
                waitingCoroutine = WaitingBellowTheLine();
                StopCoroutine(waitingCoroutine);
                StartCoroutine(waitingCoroutine);

            }
            //Above the line
            else if (targetMarker.transform.position.y > imgLine.transform.position.y && IsBellow == true && IsWaiting == false)//No stay during 4 seconds !!
            {
                IsBellow = false;
                IsWaiting = false;

                //Change color line
                imgLine.color = Color.red;

                StopCoroutine(waitingCoroutine);
            }

            if (IsWaiting && IsBellow)
            {
                //Change line color
                imgLine.color = Color.green;

                //Active UI feddback
                returnToInitialPosition.SetActive(true);

                //Set line position
                imgLine.transform.position = linePosition;

                IsBellow = false;
                IsGoBack = true;
            }
        }
    }

    void GoToNextTrial()
    {
        nextTrial.gameObject.SetActive(true);
        startTrial.gameObject.SetActive(false);

        inputNextTrial = false;
        inputStartTrial = false;

        IsStartTrialTask = false;
        IsStayBellowTheLine = false;
        IsBellow = false;
        IsTaskValidated = false;

        imgLine.gameObject.SetActive(false);
        returnToInitialPosition.SetActive(false);

        //UI SCALING FACTOR INPUT
        scalingFactorInputPanel.SetActive(true);

        //UI INFOS
        infoBlock.text = "Waiting  - Next trial";
        infoCommand.text = "Input 'N' key to start next trial";
        nextTrialTaskText.text = (TrialNumber + 1).ToString();
    }

    void StartNextTrial()
    {
        try
        {
            ScalingFactor = float.Parse(inputScalingFactor.text);
        }
        catch (System.Exception er)
        {
            infoCommand.text = "ERROR : You must to input scaling factor [-∞; +∞]";
            Debug.LogError(er.ToString());
            return;
        }

        StartRecording();

        //Start trial
        NextTrial();

        //UI INFOS
        infoBlock.text = "Trial " + (TrialNumber) + "\n" + "Scaling factor = " + ScalingFactor /*+"°"*/;
        //infoCommand.text = "Input 'N' key to pass to the next trial";
      
    }

    public void NextTrial()
    {
        if (IsStartTask)//task
        {
            fadeCameraVR.m_FadeDuration = 0.5f;
            fadeCameraVR.FadeIn(false);

            //Debug.Log("START TRIAL");
            inputNextTrial = false;
            inputStartTrial = false;
            IsStartTrialTask = true;
            IsStayBellowTheLine = false;
            IsTaskValidated = false;

            nextTrial.gameObject.SetActive(true);
            startTrial.gameObject.SetActive(false);

            returnToInitialPosition.SetActive(false);

            //Active line
            if (imgLine.gameObject.activeInHierarchy == false)
                imgLine.gameObject.SetActive(true);

            //Line
            imgLine.color = Color.red;
            imgLine.transform.position = linePosition;

            //Next trial
            TrialNumber++;
            consecutiveTrial++;

            //UI SCALING FACTOR INPUT
            scalingFactorInputPanel.SetActive(false);
        }
    }

    public void NotValidateTrial()
    {
        if(consecutiveTrial == 2)
        {
            fadeCameraVR.m_FadeDuration = 0.5f;
            fadeCameraVR.FadeOut(false);

            consecutiveTrial = 0;
            inputNextTrial = false;
            inputStartTrial = false;

            IsTaskValidated = false;
            validatePanel.SetActive(false);

            returnToInitialPosition.SetActive(false);

            StopAllCoroutines();
            AddData();
            GoToNextTrial();

            StartCoroutine(StopRecording());
        }
        else
        {
            validatePanel.SetActive(false);
            IsTaskValidated = false;

            StopAllCoroutines();
            AddData();
            StartNextTrial();
        }
        
    }

    public void ValidateTrial()
    {
        if (consecutiveTrial == 2)
        {
            fadeCameraVR.m_FadeDuration = 0.5f;
            fadeCameraVR.FadeOut(false);

            consecutiveTrial = 0;
            inputNextTrial = false;
            inputStartTrial = false;

            IsTaskValidated = true;
            validatePanel.SetActive(false);

            returnToInitialPosition.SetActive(false);

            StopAllCoroutines();
            AddData();
            GoToNextTrial();

            StartCoroutine(StopRecording());
        }
        else
        {
            validatePanel.SetActive(false);
            IsTaskValidated = true;

            StopAllCoroutines();
            AddData();
            StartNextTrial();
        }
    }

    private IEnumerator StopRecording()
    {
        if (Settings.instance.recordFirstView && startRecordingVideoFirstView == true)
        {
            startRecordingVideoFirstView = false;
            OBSRecorder.Instance.StopRecordingFirstView();
        }
        if (Settings.instance.recordThirdView && startRecordingVideoThirdView == true)
        {
            startRecordingVideoThirdView = false;
            OBSRecorder.Instance.StopRecordingThirdView();
        }


        while (fadeCameraVR.IsFading)
            yield return null;

        //Change culling mask
        oldMask = Camera.main.cullingMask;
        Camera.main.cullingMask &= ~(1 << LayerMask.NameToLayer("Avatar"));

        fadeCameraVR.m_FadeDuration = 0.5f;
        fadeCameraVR.FadeIn(false);
      
        //Change room
        room.SetActive(false);
        neutralRoom.SetActive(true);
    }

    private void StartRecording()
    {
        if (WriteData.instance.path != null && WriteData.instance.path != "")
        {
            if (Settings.instance.recordFirstView && startRecordingVideoFirstView == false)
            {
                startRecordingVideoFirstView = true;
                OBSRecorder.Instance.StartRecordingFirstView();
            }
            if (Settings.instance.recordThirdView && startRecordingVideoThirdView == false)
            {
                startRecordingVideoThirdView = true;
                OBSRecorder.Instance.StartRecordingThirdView();
            }
        }

       //Reset culling mask
        Camera.main.cullingMask = oldMask;

        neutralRoom.SetActive(false);
        room.SetActive(true);
    }

    IEnumerator WaitingBellowTheLine()
    {
        yield return new WaitForSeconds(4f);

        IsWaiting = true;

        IsStayBellowTheLine = true;
        IsTaskValidated = true;

        yield return true;
    }

    IEnumerator ActiveScreen()
    {
        Animation clip;
        clip = screen.GetComponent<Animation>();

        if (screen.GetComponent<Animation>().isPlaying)
        {
            screen.GetComponent<Animation>().Stop();

            clip["Screen"].speed = 1;
            clip["Screen"].time = clip["Screen"].time;
        }
        else
        {
            clip["Screen"].speed = 1;
            clip["Screen"].time = 0;
        }

        screen.GetComponent<Animation>().Play();

        while (screen.GetComponent<Animation>().isPlaying)
        {
            yield return null;
        }

        yield return true;
    }

    IEnumerator DesactiveScreen()
    {
        Animation clip;
        clip = screen.GetComponent<Animation>();

        panelInstruction.SetActive(false);

        if (screen.GetComponent<Animation>().isPlaying)
        {
            clip["Screen"].speed = -1;
            clip["Screen"].time = clip["Screen"].time;

        }
        else
        {
            clip["Screen"].speed = -1;
            clip["Screen"].time = clip["Screen"].length;
        }

        screen.GetComponent<Animation>().Play();

        while (screen.GetComponent<Animation>().isPlaying)
        {
            yield return null;
        }

        yield return true;
    }

    void AddData()
    {
        WriteData.instance.AddData(Settings.instance.participantId, TrialNumber.ToString(), ScalingFactor.ToString(), IsTaskValidated.ToString(), IsStayBellowTheLine.ToString());//Specify all arguments (here, name, age, adress)
    }
   
    public float MapValue(float x, float inMin, float inMax, float outMin, float outMax)
    {
        return (x - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    public void ValueSkeletonIDChanged()
    {
        menAvatar.SkeletonName = Settings.instance.skeleton_id.text;
        menTargetAvatar.SkeletonName = Settings.instance.skeleton_id.text;

        femaleAvatar.SkeletonName = Settings.instance.skeleton_id.text;
        femaleTargetAvatar.SkeletonName = Settings.instance.skeleton_id.text;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    //When application is quit, write all data to the file
    private void OnDestroy()
    {
        StopAllCoroutines();

        Thread myThread = new System.Threading.Thread(delegate ()
        {
            WriteData.instance.WriteAllData();
        });

        myThread.Start();
    }

}
