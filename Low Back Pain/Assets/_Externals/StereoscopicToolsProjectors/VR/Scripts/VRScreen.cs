using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityTools.VR
{
	/// <summary>
	/// Represents a Screen
	/// </summary>
	[ExecuteInEditMode]
	public class VRScreen : MonoBehaviour
	{
		public bool isStatic = true; // this one can be checked at runtime (http://docs.unity3d.com/ScriptReference/GameObject-isStatic.html)
		public float width = 16.0f; // in meters
		public float height = 9.0f; // in meters
		public Color debugColor = Color.blue;

#if UNITY_EDITOR
		private Mesh _plane = null;

		void OnEnable()
		{
			_plane = Geometry.Mesh.CreatePlane(width, height);
			// http://docs.unity3d.com/ScriptReference/HideFlags.html
			_plane.hideFlags = HideFlags.HideAndDontSave;
			//Debug.Log("Plane Created !");
		}

		void OnDisable()
		{
			UnityEngine.Object.DestroyImmediate(_plane);
			//Debug.Log("Plane Destroyed !");
		}

		void OnValidate()
		{
			gameObject.isStatic = isStatic;

			// Update the plane whenever the inspector's values changed.
			// http://unityready.com/implement-onvalidate-control-inspector-properties-values/
			if (_plane != null)
			{
				Geometry.Mesh.UpdatePlane(_plane, width, height);
				//Debug.Log("Plane Updated !");
			}
		}

		void OnDrawGizmos()
		{
			Gizmos.color = debugColor;
			Gizmos.DrawMesh(_plane, Center, transform.rotation);

			Handles.Label(TopLeftCorner, name + "_TL", GUI.skin.button);
			Handles.Label(BottomRightCorner, name + "_BR", GUI.skin.button);
		}
#endif

		#region Properties
		/// <summary>
		/// Middle of the Screen
		/// </summary>
		public Vector3 Center
		{
			get
			{
				return transform.position;
			}
		}

		/// <summary>
		/// The normal of the Screen's plane.
		/// </summary>
		public Vector3 Normal
		{
			get
			{
				return transform.forward;
			}
		}

		public Vector3 Up
		{
			get
			{
				return transform.up;
			}
		}

		public Vector3 TopLeftCorner
		{
			get
			{
				return Center + (transform.right * 0.5f * width) + (transform.up * 0.5f * height);
			}
		}

		public Vector3 TopRightCorner
		{
			get
			{
				return Center + (transform.right * -0.5f * width) + (transform.up * 0.5f * height);
			}
		}

		public Vector3 BottomLeftCorner
		{
			get
			{
				return Center + (transform.right * 0.5f * width) + (transform.up * -0.5f * height);
			}
		}

		public Vector3 BottomRightCorner
		{
			get
			{
				return Center + (transform.right * -0.5f * width) + (transform.up * -0.5f * height);
			}
		}
		#endregion

		public bool IsInFrontOfScreen(Vector3 pPoint, float pDistanceToScreenMin)
		{
			return Vector3.Dot(pPoint - (Center + (Normal * pDistanceToScreenMin)), Normal) > 0.0f;
			//return Vector3.Dot(pPoint - Center, Normal) > 0.0f;
		}

		public Vector3 ProjectOnScreen(Vector3 pPoint, float pDistanceToScreenMin)
		{
			return ProjectPointOnPlane(Normal, (Center + (Normal * pDistanceToScreenMin)), pPoint);
			//return Maths.Geometry.ProjectPointOnPlane(Normal, Center, pPoint);
		}

		public static Vector3 ProjectPointOnPlane(Vector3 pPlaneNormal, Vector3 pPlanePoint, Vector3 pPoint)
		{
			return pPoint + (pPlaneNormal * -Vector3.Dot(pPlaneNormal, (pPoint - pPlanePoint)));
		}
	}

	//[CustomEditor(typeof(Screen))]
	//public class ScreenEditor : Editor
	//{
	//	public override void OnInspectorGUI()
	//	{
	//		// Draw Inspector
	//		DrawDefaultInspector();

	//		// Detect changes in the Inspector to update the plane.
	//		//http://answers.unity3d.com/questions/39515/how-do-i-detect-that-monobehaviour-is-modified-in.html
	//		if (GUI.changed)
	//		{
	//			Screen screen = target as Screen;
	//			screen.UpdatePlane();
	//			EditorUtility.SetDirty(target);
	//		}
	//	}
	//}

	//public class ScreenGizmoDrawer
	//{
	//	[DrawGizmo(GizmoType.NonSelected | GizmoType.Selected)]
	//	public static void DrawScreenGizmos(Screen screen, GizmoType gizmoType)
	//	{
	//		screen.DrawGizmos();
	//	}
	//}
}