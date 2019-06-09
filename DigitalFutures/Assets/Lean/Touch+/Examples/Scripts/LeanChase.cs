using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This script allows you to drag this GameObject in a way that causes it to chase the current finger.</summary>
	public class LeanChase : MonoBehaviour
	{
		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("Ignore fingers if the finger count doesn't match? (0 = any)")]
		public int RequiredFingerCount;

		[Tooltip("Does translation require an object to be selected?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("How sharp the position value changes update (-1 = instant)")]
		public float Dampening = -1.0f;

		[Tooltip("The conversion method used to find a world point from a screen point")]
		public LeanScreenDepth ScreenDepth;

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

		protected virtual void FixedUpdate()
		{
			// Get the fingers we want to use
			var fingers = LeanSelectable.GetFingers(IgnoreStartedOverGui, IgnoreIsOverGui, RequiredFingerCount, RequiredSelectable);

			if (fingers.Count > 0)
			{
				var targetPoint = LeanGesture.GetScreenCenter(fingers);
				var newPosition = ScreenDepth.Convert(targetPoint, gameObject);
				var factor      = LeanTouch.GetDampenFactor(Dampening, Time.fixedDeltaTime);

				transform.position = Vector3.Lerp(transform.position, newPosition, factor);
			}
		}
	}
}