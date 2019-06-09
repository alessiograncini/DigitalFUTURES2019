using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This modifies LeanMap3D to be smooth.</summary>
	public class LeanMap3DSmooth : LeanMap3D
	{
		[Tooltip("How smoothly this object moves to its target position")]
		public float Dampening = 10.0f;

		[System.NonSerialized]
		private Vector3 remainingTranslation;

		protected override void Update()
		{
			if (IsDragging == true)
			{
				var finalTransform = Target != null ? Target.transform : transform;

				// Store
				var oldPosition = finalTransform.localPosition;

				// Update
				base.Update();

				// Increment
				remainingTranslation = finalTransform.localPosition - oldPosition;

				// Revert
				finalTransform.localPosition = oldPosition;
			}
		}

		protected virtual void LateUpdate()
		{
			var finalTransform = Target != null ? Target.transform : transform;

			// Get t value
			var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

			// Dampen remainingDelta
			var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);

			// Shift this transform by the change in delta
			finalTransform.localPosition += remainingTranslation - newRemainingTranslation;

			// Update remainingDelta with the dampened value
			remainingTranslation = newRemainingTranslation;
		}
	}
}