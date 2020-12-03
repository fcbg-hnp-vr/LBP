using UnityEngine;

namespace UnityTools.VR
{
    public class VREyeCenterTracker : VREyeCenter
    {
		public MonoBehaviour Tracker;
        public Transform Origin;

        void Update()
		{
			if(Tracker.enabled)
			{
				Position = Origin.InverseTransformPoint(Tracker.transform.position);
				//Position = new Vector3(-Position.x, Position.y, Position.z);
			}
        }

        public override void EnableTracking()
        {
            Tracker.enabled = true;
        }

        public override void DisableTracking()
        {
            Tracker.enabled = false;
        }
    }
}