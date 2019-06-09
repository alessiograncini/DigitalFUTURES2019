using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component stores a list of all fingers that began touching the current RectTransform.</summary>
	[RequireComponent(typeof(RectTransform))]
	public class LeanCanvasFingers : MonoBehaviour
	{
		// Event signature
		[System.Serializable] public class LeanFingerEvent : UnityEvent<LeanFinger> {}
		[System.Serializable] public class LeanFingerListEvent : UnityEvent<List<LeanFinger>> {}

		[Tooltip("The layers you want the raycast/overlap to hit")]
		public LayerMask LayerMask = Physics.DefaultRaycastLayers;

		public LeanFingerEvent OnDown;

		public LeanFingerListEvent OnSet;

		[System.NonSerialized]
		public List<LeanFinger> Fingers = new List<LeanFinger>();

		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerDown += FingerDown;
			LeanTouch.OnFingerUp   += FingerUp;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerDown -= FingerDown;
			LeanTouch.OnFingerUp   -= FingerUp;
		}

		protected virtual void Update()
		{
			if (Fingers.Count > 0)
			{
				if (OnSet != null)
				{
					OnSet.Invoke(Fingers);
				}
			}
		}

		private void FingerDown(LeanFinger finger)
		{
			var results = LeanTouch.RaycastGui(finger.ScreenPosition, LayerMask);

			if (results != null && results.Count > 0)
			{
				if (results[0].gameObject == gameObject)
				{
					Fingers.Add(finger);

					if (OnDown != null)
					{
						OnDown.Invoke(finger);
					}
				}
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			Fingers.Remove(finger);
		}
	}
}