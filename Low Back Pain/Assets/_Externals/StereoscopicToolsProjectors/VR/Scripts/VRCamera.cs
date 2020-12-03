    using UnityEngine;

namespace UnityTools.VR
{
	/// <summary>
	/// Use this to give a VR effect to your Camera projection matrix.
	/// When Enabled, the Camera motion is constrained with the eyeCenter transform.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class VRCamera : MonoBehaviour
	{
		public VRScreen screen; // The screen the camera frustum is attached to.
		//public float distanceLimit = 0.0f; // prevent Camera from being to close to the screen.

		private Camera _camera;
        private VREyeCenter _eyeCenter;
        private Transform _originalParent;
		private Quaternion _originalOri;

		private Matrix4x4 _proj = Matrix4x4.identity; // projection matrix
		private Matrix4x4 _rot = Matrix4x4.identity; // rotation matrix
		private Matrix4x4 _trans = Matrix4x4.identity; // translation matrix
		private Quaternion _ori = Quaternion.identity; // Camera rotation

		/// <summary>
		/// Screen corners.
		/// </summary>
		private Vector3 _pa; // BottomLeftCorner
		private Vector3 _pb; // BottomRightCorner
		private Vector3 _pc; // TopLeftCorner
		private Vector3 _pm; // Middle

		/// <summary>
		/// Screen Coordinate system.
		/// </summary>
		private Vector3 _vr; // right axis of screen
		private Vector3 _vu; // up axis of screen
		private Vector3 _vn; // normal vector of screen


		// Use this for initialization
		public void EnableVR(VREyeCenter pEyeCenter)
        {
            _eyeCenter = pEyeCenter;

			_originalParent = transform.parent;
			_originalOri = transform.rotation;
			_camera.transform.parent = _eyeCenter.transform;
            if(_eyeCenter.IsStereo)
            {
                _eyeCenter.StereoCamera.UpdateIPD();
            }
            else
            {
                _camera.transform.localPosition = Vector3.zero;
            }

            //Debug.Log("VR enabled on Camera : " + name);
		}

        public void DisableVR()
        {
			_eyeCenter = null;

			transform.parent = _originalParent;
			transform.rotation = _originalOri;

			_camera.ResetWorldToCameraMatrix();
			_camera.ResetProjectionMatrix();

            //Debug.Log("VR disabled on Camera : " + name);
        }

		public void UpdateCameraProjection()
		{
            _UpdateCameraProjection(transform.position, false);
		}

		/// <summary>
		/// Check if pPoint is in front of Camera's attached screen
		/// </summary>
		/// <param name="pPoint"></param>
		/// <returns></returns>
		public bool IsInFrontOfScreen(Vector3 pPoint)
		{
			return screen.IsInFrontOfScreen(pPoint, _camera.nearClipPlane); //+ distanceLimit);
		}

		/// <summary>
		/// Check if pPoint is in front of Camera's attached screen
		/// </summary>
		/// <param name="pPoint"></param>
		/// <returns></returns>
		public Vector3 ProjectOnScreen(Vector3 pPoint)
		{
			return screen.ProjectOnScreen(pPoint, _camera.nearClipPlane); //+ distanceLimit);
		}

		//public void SetDisplay()
		//{
		//	// Change Target Display
		//	if (TargetDisplay < Display.displays.Length)
		//	{
		//		Display.displays[TargetDisplay].Activate();
		//		_camera.targetDisplay = TargetDisplay;
		//		Debug.Log(_camera.name + " renders to Display : " + TargetDisplay);
		//	}
		//	Debug.Log("SetDisplay called !");
		//}

		#region MonoBehaviour Methods
		void Awake()
		{
			_camera = GetComponent<Camera>();
            //Display.onDisplaysUpdated += SetDisplay;
            _eyeCenter = null;

			_UpdateScreenCoordinates();
        }
		#endregion

		#region Private Methods
		private void _UpdateScreenCoordinates()
		{
			_pa = screen.BottomLeftCorner;
			_pb = screen.BottomRightCorner;
			_pc = screen.TopLeftCorner;
			_pm = screen.Center; 

			_vr = _pb - _pa;
			_vu = _pc - _pa;
			_vr.Normalize();
			_vu.Normalize();
			_vn = -Vector3.Cross(_vr, _vu);
			// we need the minus sign because Unity 
			// uses a left-handed coordinate system
			_vn.Normalize();
		}

		/// <summary>
		/// Thanks to https://en.wikibooks.org/wiki/Cg_Programming/Unity/Projection_for_Virtual_Reality
		/// </summary>
		/// <param name="pEyePos"></param>
		/// <param name="pEstimateViewFrustrum"></param>
		private void _UpdateCameraProjection(Vector3 pEyePos, bool pEstimateViewFrustrum)
		{
			 if(!screen.isStatic)
			{
				_UpdateScreenCoordinates();
			}
             
			float n = _camera.nearClipPlane;
			float f = _camera.farClipPlane;

			Vector3 va; // from pEyePos to pa
			Vector3 vb; // from pEyePos to pb
			Vector3 vc; // from pEyePos to pc
			
			float l; // distance to left screen edge
			float r; // distance to right screen edge
			float b; // distance to bottom screen edge
			float t; // distance to top screen edge
			float d; // distance from eye to screen

			va = _pa - pEyePos;
			vb = _pb - pEyePos;
			vc = _pc - pEyePos;

			d = -Vector3.Dot(va, _vn);
			l = Vector3.Dot(_vr, va) * n / d;
			r = Vector3.Dot(_vr, vb) * n / d;
			b = Vector3.Dot(_vu, va) * n / d;
			t = Vector3.Dot(_vu, vc) * n / d;

			// projection matrix
			_proj[0, 0] = 2.0f * n / (r - l);
			_proj[0, 1] = 0.0f;
			_proj[0, 2] = (r + l) / (r - l);
			_proj[0, 3] = 0.0f;

			_proj[1, 0] = 0.0f;
			_proj[1, 1] = 2.0f * n / (t - b);
			_proj[1, 2] = (t + b) / (t - b);
			_proj[1, 3] = 0.0f;

			_proj[2, 0] = 0.0f;
			_proj[2, 1] = 0.0f;
			_proj[2, 2] = (f + n) / (n - f);
			_proj[2, 3] = 2.0f * f * n / (n - f);

			_proj[3, 0] = 0.0f;
			_proj[3, 1] = 0.0f;
			_proj[3, 2] = -1.0f;
			_proj[3, 3] = 0.0f;

			// rotation matrix
			_rot[0, 0] = _vr.x;
			_rot[0, 1] = _vr.y;
			_rot[0, 2] = _vr.z;
			_rot[0, 3] = 0.0f;

			_rot[1, 0] = _vu.x;
			_rot[1, 1] = _vu.y;
			_rot[1, 2] = _vu.z;
			_rot[1, 3] = 0.0f;

			_rot[2, 0] = _vn.x;
			_rot[2, 1] = _vn.y;
			_rot[2, 2] = _vn.z;
			_rot[2, 3] = 0.0f;

			_rot[3, 0] = 0.0f;
			_rot[3, 1] = 0.0f;
			_rot[3, 2] = 0.0f;
			_rot[3, 3] = 1.0f;

			// translation matrix
			_trans[0, 0] = 1.0f;
			_trans[0, 1] = 0.0f;
			_trans[0, 2] = 0.0f;
			_trans[0, 3] = -pEyePos.x;

			_trans[1, 0] = 0.0f;
			_trans[1, 1] = 1.0f;
			_trans[1, 2] = 0.0f;
			_trans[1, 3] = -pEyePos.y;

			_trans[2, 0] = 0.0f;
			_trans[2, 1] = 0.0f;
			_trans[2, 2] = 1.0f;
			_trans[2, 3] = -pEyePos.z;

			_trans[3, 0] = 0.0f;
			_trans[3, 1] = 0.0f;
			_trans[3, 2] = 0.0f;
			_trans[3, 3] = 1.0f;

            // set matrices

            _camera.projectionMatrix = _proj;
            _camera.worldToCameraMatrix = _rot * _trans;


			// The original paper puts everything into the projection 
			// matrix (i.e. sets it to p * rm * tm and the other 
			// matrix to the identity), but this doesn't appear to 
			// work with Unity's shadow maps.

			if (pEstimateViewFrustrum)
			{
				// rotate camera to screen for culling to work
				_ori.SetLookRotation(_pm - pEyePos, _vu);
				// look at center of screen
				_camera.transform.rotation = _ori;

				// set fieldOfView to a conservative estimate 
				// to make frustum tall enough
				if (_camera.aspect >= 1.0)
				{
					_camera.fieldOfView = Mathf.Rad2Deg *
					   Mathf.Atan(((_pb - _pa).magnitude + (_pc - _pa).magnitude)
					   / va.magnitude);
				}
				else
				{
					// take the camera aspect into account to 
					// make the frustum wide enough 
					_camera.fieldOfView =
					   Mathf.Rad2Deg / _camera.aspect *
					   Mathf.Atan(((_pb - _pa).magnitude + (_pc - _pa).magnitude)
					   / va.magnitude);
				}
			}
		}

		#endregion
	}
}