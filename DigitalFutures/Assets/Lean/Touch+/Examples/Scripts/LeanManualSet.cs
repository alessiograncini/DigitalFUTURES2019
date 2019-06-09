using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component calls the OnSet event when a finger monitored by this component is set.
	/// NOTE: This component doesn't do anything on its own, you first must call the AddFinger method.</summary>
	public class LeanSet : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class FingerEvent : UnityEvent<LeanFinger> {}

		[Tooltip("Only call OnSet with information from the first finger?")]
		public bool OnlyUseFirst;

		/// <summary>This event is invoked when a monitored finger is set.</summary>
		public FingerEvent OnSet;

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

		protected virtual void Update()
		{
			for (var i = 0; i < fingers.Count; i++)
			{
				var finger = fingers[i];

				// Invoke OnSet
				if (OnSet != null)
				{
					OnSet.Invoke(finger);
				}

				// Skip other fingers?
				if (OnlyUseFirst == true)
				{
					break;
				}
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			for (var i = fingers.Count - 1; i >= 0; i--)
			{
				if (fingers[i] == finger)
				{
					fingers.RemoveAt(i);
					
					return;
				}
			}
		}
	}
}