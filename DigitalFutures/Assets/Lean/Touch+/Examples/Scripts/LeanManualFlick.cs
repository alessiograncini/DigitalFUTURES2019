using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component calls the OnFlick event when a finger monitored by this component flicks.
	/// NOTE: This component doesn't do anything on its own, you first must call the AddFinger method.</summary>
	public class LeanManualFlick : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class FingerEvent : UnityEvent<LeanFinger> {}

		[Tooltip("Must the swipe be in a specific direction?")]
		public bool CheckAngle;

		[Tooltip("The required angle of the swipe in degrees, where 0 is up, and 90 is right")]
		public float Angle;

		[Tooltip("The left/right tolerance of the swipe angle in degrees")]
		public float AngleThreshold = 90.0f;

		/// <summary>This event is invoked when a monitored finger flicks.</summary>
		public FingerEvent OnFlick;

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

		protected virtual void Update()
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (UpdateFinger(i) == true)
				{
					fingers.RemoveAt(i);
				}
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			fingers.Remove(finger);
		}

		private bool UpdateFinger(int index)
		{
			var finger = fingers[index];

			// Remove fingers that are too old
			if (finger.Age >= LeanTouch.CurrentTapThreshold)
			{
				return true;
			}

			for (var i = finger.Snapshots.Count - 1; i >= 0; i--)
			{
				var shapshot = finger.Snapshots[i];
				var delta    = finger.ScreenPosition - shapshot.ScreenPosition;

				// Invalid angle?
				if (CheckAngle == true)
				{
					var angle      = Mathf.Atan2(delta.x, delta.y) * Mathf.Rad2Deg;
					var angleDelta = Mathf.DeltaAngle(angle, Angle);

					if (angleDelta < AngleThreshold * -0.5f || angleDelta >= AngleThreshold * 0.5f)
					{
						continue;
					}
				}

				if (delta.magnitude * LeanTouch.ScalingFactor > LeanTouch.CurrentSwipeThreshold)
				{
					if (OnFlick != null)
					{
						OnFlick.Invoke(finger);
					}

					return true;
				}
			}

			return false;
		}
	}
}