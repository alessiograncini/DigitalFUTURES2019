using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component calls the OnDistance event based on how far a finger has moved. The fingers must be set using the AddFinger event.</summary>
	[HelpURL(LeanTouch.HelpUrlPrefix + "LeanManualDragDistance")]
	public class LeanManualDragDistance : MonoBehaviour
	{
		public enum ClampType
		{
			None,
			Normalize,
			Direction4,
			Horizontal,
			Vertical
		}

		// Event signature
		[System.Serializable] public class FloatEvent : UnityEvent<float> {}

		[Tooltip("Should the distance be scaled to DPI, or be in pixels?")]
		public bool Scaled;

		[Tooltip("Should the swipe delta be modified before use?")]
		public ClampType Clamp;

		[Tooltip("The final distance values will be multiplied by this.")]
		public float Multiplier = 1.0f;

		[Header("Constraints")]
		[Tooltip("The required angle of the swipe in degrees, where 0 is up, and 90 is right")]
		public float RequiredAngle;

		[Tooltip("The threshold of the swipe angle in degrees, where 90 is a quarter of a circle.")]
		public float AngleThreshold = 360.0f;

		public bool DualSided;

		[Tooltip("Must the scaled distance be over this value for the event to fire?")]
		public float DistanceThreshold;

		public FloatEvent OnDistance;

		private List<LeanFinger> fingers = new List<LeanFinger>();

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
		}

		protected virtual void Update()
		{
			for (var i = 0; i < fingers.Count; i++)
			{
				UpdateFinger(fingers[i]);
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			fingers.Remove(finger);
		}

		private void UpdateFinger(LeanFinger finger)
		{
			var vector = finger.SwipeScreenDelta;

			// Clamp delta?
			switch (Clamp)
			{
				case ClampType.Normalize:
				{
					vector = vector.normalized;
				}
				break;

				case ClampType.Direction4:
				{
					if (vector.x < -Mathf.Abs(vector.y)) vector = -Vector2.right;
					if (vector.x >  Mathf.Abs(vector.y)) vector =  Vector2.right;
					if (vector.y < -Mathf.Abs(vector.x)) vector = -Vector2.up;
					if (vector.y >  Mathf.Abs(vector.x)) vector =  Vector2.up;
				}
				break;

				case ClampType.Horizontal:
				{
					vector.y = 0.0f;
				}
				break;

				case ClampType.Vertical:
				{
					vector.x = 0.0f;
				}
				break;
			}

			var distance = vector.magnitude;

			// Invalid distance?
			if (DistanceThreshold > 0.0f)
			{
				if (distance < DistanceThreshold)
				{
					return;
				}
			}

			// Invalid angle?
			if (InvalidAngle(vector, ref distance) == true)
			{
				return;
			}

			if (Scaled == true)
			{
				distance *= LeanTouch.ScalingFactor;
			}

			if (OnDistance != null)
			{
				OnDistance.Invoke(distance * Multiplier);
			}
		}

		private bool InvalidAngle(Vector2 vector, ref float distance)
		{
			if (AngleThreshold < 360.0f)
			{
				var angle = Mathf.Atan2(vector.x, vector.y) * Mathf.Rad2Deg;
				var delta = Mathf.DeltaAngle(angle, RequiredAngle);
				var half  = RequiredAngle * 0.5f;

				if (delta >= -half && delta < half)
				{
					return false;
				}

				if (DualSided == true)
				{
					delta    = Mathf.DeltaAngle(angle + 180.0f, RequiredAngle);
					distance = -distance;

					if (delta >= -half && delta < half)
					{
						return false;
					}
				}

				return true;
			}

			return false;
		}
	}
}