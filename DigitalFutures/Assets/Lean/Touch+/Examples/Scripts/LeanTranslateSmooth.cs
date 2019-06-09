using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component modifies LeanTranslate to be smooth.</summary>
	public class LeanTranslateSmooth : LeanTranslate
	{
		[Tooltip("How smoothly this object moves to its target position")]
		public float Dampening = 10.0f;

		[System.NonSerialized]
		private Vector3 remainingTranslation;

		protected override void Update()
		{
			// Store
			var oldPosition = transform.localPosition;

			// Update
			base.Update();

			// Increment
			remainingTranslation += transform.localPosition - oldPosition;

			// Revert
			transform.localPosition = oldPosition;
		}

		protected virtual void LateUpdate()
		{
			// Get t value
			var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

			// Shift this transform by the change in delta
			transform.localPosition += remainingTranslation - newRemainingTranslation;

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;
		}
	}
}