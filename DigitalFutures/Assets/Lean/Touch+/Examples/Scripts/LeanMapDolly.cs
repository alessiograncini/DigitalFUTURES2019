using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary></summary>
	public class LeanMapDolly : MonoBehaviour
	{
		[Tooltip("The camera that will be zoomed (None = MainCamera)")]
		public Camera Camera;

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("If RequiredSelectable.IsSelected is false, ignore?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
		[Range(-1.0f, 1.0f)]
		public float WheelSensitivity;

		public float SpeedMultiplier = 1.0f;

		public Transform Target;

		protected virtual void Update()
		{
			var camera = LeanTouch.GetCamera(Camera, gameObject);

			if (camera != null)
			{
				var finalTransform = Target != null ? Target.transform : transform;

				// Get the fingers we want to use
				var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

				// Get the pinch ratio of these fingers
				var pinchRatio = LeanGesture.GetPinchRatio(fingers, WheelSensitivity);

				var pinchShift = pinchRatio - 1.0f;
				var center     = LeanGesture.GetScreenCenter(fingers);

				if (fingers.Count == 0)
				{
					center = Input.mousePosition;
				}

				var ray = camera.ScreenPointToRay(center);

				finalTransform.position -= ray.direction * pinchShift * SpeedMultiplier;
			}
		}
	}
}