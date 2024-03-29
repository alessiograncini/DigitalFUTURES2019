using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This script calculates the multi-tap event.
	/// A multi-tap is where you press and release at least one finger at the same time.</summary>
	public class LeanMultiTap : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class IntEvent : UnityEvent<int, int> {}

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("If RequiredSelectable.IsSelected is false, ignore?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("This is set to true the frame a multi-tap occurs")]
		public bool MultiTap;

		[Tooltip("This is set to the current multi-tap count")]
		public int MultiTapCount;

		[Tooltip("Highest number of fingers held down during this multi-tap")]
		public int HighestFingerCount;

		// Called when a multi-tap occurs (Int = multi-tap count, Int = highest finger count)
		public IntEvent OnTap;

		// Seconds at least one finger has been held down
		private float age;

		// Previous fingerCount
		private int lastFingerCount;

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Start();
		}
#endif

		protected virtual void Start()
		{
			if (RequiredSelectable == null)
			{
				RequiredSelectable = GetComponent<LeanSelectable>();
			}
		}

		protected virtual void Update()
		{
			// Get fingers and calculate how many are still touching the screen
			var fingers     = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);
			var fingerCount = GetFingerCount(fingers);

			// At least one finger set?
			if (fingerCount > 0)
			{
				// Did this just begin?
				if (lastFingerCount == 0)
				{
					age                = 0.0f;
					HighestFingerCount = fingerCount;
				}
				else if (fingerCount > HighestFingerCount)
				{
					HighestFingerCount = fingerCount;
				}
			}

			age += Time.unscaledDeltaTime;

			// Reset
			MultiTap = false;

			// Is a multi-tap still eligible?
			if (age <= LeanTouch.CurrentTapThreshold)
			{
				// All fingers released?
				if (fingerCount == 0 && lastFingerCount > 0)
				{
					MultiTapCount += 1;

					if (OnTap != null)
					{
						OnTap.Invoke(MultiTapCount, HighestFingerCount);
					}
				}
			}
			// Reset
			else
			{
				MultiTapCount      = 0;
				HighestFingerCount = 0;
			}

			lastFingerCount = fingerCount;
		}

		private int GetFingerCount(List<LeanFinger> fingers)
		{
			var count = 0;

			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (fingers[i].Up == false)
				{
					count += 1;
				}
			}

			return count;
		}
	}
}