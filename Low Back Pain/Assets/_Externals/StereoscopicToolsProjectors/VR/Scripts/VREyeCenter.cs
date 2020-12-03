using System;
using UnityEngine;

namespace UnityTools.VR
{
	/// <summary>
	/// Helper in case screens are too close to the user.
	/// </summary>
	[Serializable]
	public class FixAxis
	{
		public bool fixX = false;
		public bool fixY = false;
		public bool fixZ = false;
		public Vector3 fixedValue = Vector3.zero;

		public Vector3 FixValue(Vector3 pInput)
		{
			if (fixX)
			{
				pInput.x = fixedValue.x;
			}
			if (fixY)
			{
				pInput.y = fixedValue.y;
			}
			if (fixZ)
			{
				pInput.z = fixedValue.z;
			}
			return pInput;
		}
	}

    /// <summary>
    /// Interface to control the eye center for virtual reality applications.
    /// </summary>
    public abstract class VREyeCenter : MonoBehaviour
    {
		public FixAxis fixAxis;
		public bool debugOnScreenProjection = false;

		private VRCamera[] _vrCameras;
		private GameObject[] _debugOnScreensProjections;

        public StereoCamera StereoCamera { get; private set; }
        public bool IsStereo { get { return StereoCamera != null; } }

        public abstract void EnableTracking();
        public abstract void DisableTracking();

		/// <summary>
		/// Use this property to access Position instead of using transform.position.
		/// </summary>
		public Vector3 Position
		{
			get { return transform.position; }
			set { _SetEyeCenterPosition(value); }
		}

		public virtual void Awake()
		{
            StereoCamera = GetComponent<StereoCamera>();
            _vrCameras = FindObjectsOfType<VRCamera>();
			_debugOnScreensProjections = null;
        }

		void LateUpdate()
		{
			if (debugOnScreenProjection)
			{
				if(_debugOnScreensProjections == null)
				{
					Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
					int layer = LayerMask.NameToLayer("Debug");
					_debugOnScreensProjections = new GameObject[_vrCameras.Length];
					for (int i = 0; i < _debugOnScreensProjections.Length; ++i)
					{
						GameObject debug = GameObject.CreatePrimitive(PrimitiveType.Sphere);
						debug.name = "EyeCenter proj_" + _vrCameras[i].screen.name;
						debug.transform.localScale = scale;
						debug.layer = layer;
						_debugOnScreensProjections[i] = debug;
                    }
				}

				for (int i = 0; i < _debugOnScreensProjections.Length; ++i)
				{
					_debugOnScreensProjections[i].transform.position = _vrCameras[i].ProjectOnScreen(transform.position);
				}
			}
		}

		/// <summary>
		/// Constraints EyeCenter to not cross any Screen.
		/// </summary>
		/// <param name="pNewPos"></param>
		private void _SetEyeCenterPosition(Vector3 pNewPos)
		{
            transform.localPosition = pNewPos;
            pNewPos = transform.position;

            foreach (VRCamera vrCam in _vrCameras)
            {
                if (!vrCam.IsInFrontOfScreen(pNewPos))
                {
                    pNewPos = vrCam.ProjectOnScreen(pNewPos);
                }
            }

            transform.position = fixAxis.FixValue(pNewPos);
		}
	}
}