using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component calls the OnSwipe event when a finger monitored by this component swipes.
	/// NOTE: This component doesn't do anything on its own, you first must call the AddFinger method.</summary>
	public class LeanSwipe : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class FingerEvent : UnityEvent<LeanFinger> {}

		[Tooltip("Must the swipe be in a specific direction?")]
		public bool CheckAngle;

		[Tooltip("The required angle of the swipe in degrees, where 0 is up, and 90 is right")]
		public float Angle;

		[Tooltip("The left/right tolerance of the swipe angle in degrees")]
		public float AngleThreshold = 90.0f;

		/// <summary>This event is invoked when a monitored finger swipes.</summary>
		public FingerEvent OnSwipe;

		[System.NonSerialized]
		private List<LeanFinger> fingers = new List<LeanFinger>();

		/// <summary>This removes all monitored fingers from this component.</summary>
		public void ClearFingers()
		{
			fingers.Clear();
		}

		/// <summary>This adds a finger to the component, allowing it to be monitored for events.</summary>
		public void AddFinger(LeanFinger finger)
		{
			if (fingers.Contains(finger) == false)
			{
				finger.StartScreenPosition = finger.ScreenPosition;

				fingers.Add(finger);
			}
		}

		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerUp += FingerUp;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerUp -= FingerUp;

			ClearFingers();
		}

		private void FingerUp(LeanFinger finger)
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (fingers[i] == finger)
				{
					UpdateFinger(finger);

					fingers.RemoveAt(i);
					
					return;
				}
			}
		}

		private void UpdateFinger(LeanFinger finger)
		{
			// Too old?
			if (finger.Age >= LeanTouch.CurrentTapThreshold)
			{
				return;
			}

			var delta = finger.ScreenPosition - finger.StartScreenPosition;

			// Invalid angle?
			if (CheckAngle == true)
			{
				var angle      = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
				var angleDelta = Mathf.DeltaAngle(angle, Angle);

				if (angleDelta < AngleThreshold * -0.5f || angleDelta >= AngleThreshold * 0.5f)
				{
					return;
				}
			}

			if (delta.magnitude * LeanTouch.ScalingFactor > LeanTouch.CurrentSwipeThreshold)
			{
				if (OnSwipe != null)
				{
					OnSwipe.Invoke(finger);
				}
			}
		}
	}
}