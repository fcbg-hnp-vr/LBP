using UnityEngine;

namespace UnityTools.VR
{
    /// <summary>
    /// Use it to manage VR & Stereo with Unity3D v5.3.4f1.
    /// You must set your Player Settings like this:
    ///		- DirectX11(Fullscreen Window)/DirectX9(Fullscreen Window)/OpenGL.
    ///		- Stereoscopic Rendering
    ///		- Virtual Reality Supported
    ///		- Gamma Color Space (Linear breaks Stereo).
    /// Only works since Windows 8.1 (DX 11.1) with Nvidia Quadro & AMD FirePro GPUs.
    /// Run your app with commandline argument: MyApp.exe -vrmode stereo (-force-glcore)
    /// </summary>
    public class VRStereoManager : MonoBehaviour
    {
        public GameObject stereoManagerUI;
        public UnityEngine.UI.Slider vrModeSlider;
        public UnityEngine.UI.Slider stereoSeparationSlider;
        public UnityEngine.UI.Text stereoModeText;

        private UnityEngine.UI.Text _vrModeText;
        private UnityEngine.UI.Text _stereoSeparationText;

        public bool activeStereo = false;
        private bool ipdNeedUpdate = false;

        // VR
        public VREyeCenter eyeCenter;
        private VRCamera[] _vrCameras;
        private StereoCamera[] _StereoCameras;

        public bool VRModeEnabled
        {
            get { return vrModeSlider.value != 0.0f; }
            set { vrModeSlider.value = value ? 1.0f : 0.0f; }
        }

		/// <summary>
		/// inter-pupillary distance (default value = 0.065f).
		/// </summary>
		public float IPD
        {
            get { return stereoSeparationSlider.value; }
            set { stereoSeparationSlider.value = value; }
        }

		public bool StereoEnabled
		{
			get
			{
				return IPD > 0.0f;
			}
		}

		void Awake()
        {
			_vrCameras = FindObjectsOfType<VRCamera>();
            _StereoCameras = FindObjectsOfType<StereoCamera>();
        }

		void Start()
		{
			stereoManagerUI.SetActive(false);

            vrModeSlider.minValue = 0.0f;
            vrModeSlider.maxValue = 1.0f;
            vrModeSlider.wholeNumbers = true;
            _vrModeText = vrModeSlider.GetComponentInChildren<UnityEngine.UI.Text>();

            stereoSeparationSlider.minValue = 0.0f;
            stereoSeparationSlider.maxValue = 0.5f;
            stereoSeparationSlider.wholeNumbers = false;
            _stereoSeparationText = stereoSeparationSlider.GetComponentInChildren<UnityEngine.UI.Text>();

            IPD = 0.065f;
            VRModeEnabled = true;

			// StereoSeparation adjusment works only in OpenGL mode (needed in Unity v5.2.2f1).
			//if(SystemInfo.graphicsDeviceType != UnityEngine.Rendering.GraphicsDeviceType.OpenGLCore)
			//{
			//	stereoSeparationSlider.gameObject.SetActive(false);
			//	stereoModeText.gameObject.SetActive(false);
			//}
		}

		void Update()
		{
			if(Input.GetKey(KeyCode.LeftControl) && Input.GetKeyUp(KeyCode.F1))
			{
				// Show / Hide UI
				stereoManagerUI.SetActive(!stereoManagerUI.activeInHierarchy);
                if(stereoManagerUI.activeInHierarchy)
                {
                    // Put in front of all other UI.
                    stereoManagerUI.transform.SetAsLastSibling();
                }
			}
		}

		void LateUpdate()
		{
			// Always Update VR First
			if (VRModeEnabled)
			{
				_UpdateVR();
			}

            // then Stereo :)
            if(ipdNeedUpdate)
            {
                if (activeStereo)
                {
                    _UpdateActiveStereo();
                }
                else
                {
                    _UpdatePassiveStereo();
                }
            }
        }

        #region Public Methods
        public void OnVRModeSliderValueChanged()
        {
            bool ok = VRModeEnabled;
            _SetVRModeEnabled(ok);
            if(ok)
            {
                _vrModeText.text = "VR ON";
            }
            else
            {
                _vrModeText.text = "VR OFF";
            }
        }

        public void OnStereoSeparationSliderValueChanged()
        {
			if(IPD < 0.002f)
			{
				IPD = 0.0f;
			}

			_stereoSeparationText.text = string.Format("IPD: {0:0.000}", IPD);

			if(StereoEnabled)
			{
				stereoModeText.text = "Stereo ON";
			}
			else
			{
				stereoModeText.text = "Stereo OFF";
			}

            ipdNeedUpdate = true;
        }
        #endregion

        #region Private Methods
  //      private void _SetStereoModeEnabled(bool pEnabled)
  //      {
  //          if(pEnabled)
  //          {
  //              VRSettings.loadedDevice = VRDeviceType.Stereo;
  //              VRSettings.enabled = true;
  //          }
  //          else
  //          {
		//		// Doesn't work correctly
		//		VRSettings.enabled = false;
		//		VRSettings.loadedDevice = VRDeviceType.None;
		//		foreach (Camera cam in Camera.allCameras)
		//		{
		//			if (cam.stereoEnabled)
		//			{
		//				cam.ResetStereoProjectionMatrices();
		//				cam.ResetStereoViewMatrices();
		//			}
		//		}
		//	}
		//}

        private void _SetVRModeEnabled(bool pEnabled)
        {
            if (pEnabled)
            {
                foreach (VRCamera vrCam in _vrCameras)
                {
                    vrCam.EnableVR(eyeCenter);
                }
                eyeCenter.EnableTracking();
            }
            else
            {
                foreach (VRCamera vrCam in _vrCameras)
                {
                    vrCam.DisableVR();
                }
                eyeCenter.DisableTracking();
            }
        }

		private void _UpdateVR()
		{
			foreach (VRCamera vrCam in _vrCameras)
			{
				vrCam.UpdateCameraProjection();
			}
		}

        private void _UpdatePassiveStereo()
        {
            foreach (StereoCamera stCam in _StereoCameras)
            {
                stCam.SetIPD(IPD);
            }
        }

        private void _UpdateActiveStereo()
		{
			float halfIPD = IPD * 0.5f;
			foreach (Camera cam in Camera.allCameras)
			{
				if (cam.stereoEnabled)
				{
					_UpdateStereoViewMatrices(cam, halfIPD);
				}
			}
		}

		private void _UpdateStereoViewMatrices(Camera pCamera, float pHalfIPD)
		{
			//pCamera.stereoSeparation = IPD; // doesn't work in DX11/OpenGL modes

			// Works with OpenGL and DX11 since Unity v5.3.4f1
			Matrix4x4 viewL = pCamera.worldToCameraMatrix;
			Matrix4x4 viewR = pCamera.worldToCameraMatrix;
			viewL[12] += pHalfIPD;
			viewR[12] -= pHalfIPD;
			//pCamera.SetStereoViewMatrices(viewL, viewR);
			pCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Left, viewL);
			pCamera.SetStereoViewMatrix(Camera.StereoscopicEye.Right, viewR);
		}
		#endregion
	}
}