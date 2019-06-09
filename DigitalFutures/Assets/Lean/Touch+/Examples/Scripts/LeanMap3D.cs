using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component allows you to drag the camera along the X/Z axis, where the drag point always remains under the first finger. To begin the drag you must call BeginDrag from somewhere.</summary>
	public class LeanMap3D : MonoBehaviour
	{
		[Tooltip("The conversion method used to find a world point from a screen point")]
		public LeanScreenDepth ScreenDepth;

		[Tooltip("If you want to translate a different GameObject, then specify it here")]
		public GameObject Target;
		
		[System.NonSerialized]
		private LeanFinger dragFinger;
		
		[System.NonSerialized]
		private float dragHeight;

		[System.NonSerialized]
		private Vector3 dragPoint;

		[System.NonSerialized]
		private List<LeanFinger> fingers = new List<LeanFinger>();

		public bool IsDragging
		{
			get
			{
				return dragFinger != null && fingers.Count == 1;
			}
		}

		public void BeginDrag(LeanFinger finger)
		{
			var point = default(Vector3);

			if (ScreenDepth.TryConvert(ref point, finger.ScreenPosition, gameObject) == true)
			{
				dragFinger = finger;
				dragPoint  = point;
				dragHeight = point.y;

				fingers.Add(finger);
			}
		}

		public void CancelDrag()
		{
			dragFinger = null;

			fingers.Clear();
		}

		protected virtual void OnEnable()
		{
			LeanTouch.OnFingerUp += FingerUp;
		}

		protected virtual void OnDisable()
		{
			LeanTouch.OnFingerUp -= FingerUp;
		}

		protected virtual void Update()
		{
			if (IsDragging == true)
			{
				var camera = LeanTouch.GetCamera(ScreenDepth.Camera, gameObject);

				if (camera != null)
				{
					var ray   = camera.ScreenPointToRay(dragFinger.ScreenPosition);
					var slope = -ray.direction.y;

					if (slope != 0.0f)
					{
						var scale          = (ray.origin.y - dragHeight) / slope;
						var point          = ray.GetPoint(scale);
						var delta          = dragPoint - point;
						var finalTransform = Target != null ? Target.transform : transform;

						finalTransform.position += delta;
					}
				}
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			fingers.Remove(finger);

			if (finger == dragFinger)
			{
				CancelDrag();
			}
		}
	}
}