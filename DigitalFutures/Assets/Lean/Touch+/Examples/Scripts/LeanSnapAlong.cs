using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component alows you to place the current GameObject along the specified surface.</summary>
	public class LeanSnapAlong : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The conversion method used to find a world point from a screen point")]
		public LeanScreenDepth ScreenDepth;

		[System.NonSerialized]
		private Vector2 deltaDifference;

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
			// Get the fingers we want to use
			var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

			if (fingers.Count > 0)
			{
				var screenPoint   = LeanGesture.GetScreenCenter(fingers);
				var worldPosition = default(Vector3);

				if (ScreenDepth.TryConvert(ref worldPosition, screenPoint, gameObject) == true)
				{
					transform.position = worldPosition;
				}
			}
		}
	}
}