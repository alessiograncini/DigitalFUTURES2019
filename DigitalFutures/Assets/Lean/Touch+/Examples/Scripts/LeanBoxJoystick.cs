using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Lean.Touch
{
	/// <summary>This component turns the current UI element into a joystick bound to a box.</summary>
	[RequireComponent(typeof(RectTransform))]
	public class LeanBoxJoystick : LeanCanvasDraggable
	{
		// Event signature
		[System.Serializable] public class Vector2Event : UnityEvent<Vector2> {}

		/// <summary>The size limits of the joystick.</summary>
		[Tooltip("The size limits of the joystick.")]
		public Vector2 Size = new Vector2(25.0f, 25.0f);

		/// <summary>How quickly the joystick returns to the center when not being dragged.</summary>
		[Tooltip("How quickly the joystick returns to the center when not being dragged.")]
		public float Dampening = 5.0f;

		/// <summary>The -1..1 x/y position of the joystick relative to the Size.</summary>
		[Tooltip("The -1..1 x/y position of the joystick relative to the Size.")]
		public Vector2 ScaledValue;

		/// <summary>This event is invoked each frame with the ScaledValue.</summary>
		public Vector2Event OnSet;

		public override void OnDrag(PointerEventData eventData)
		{
			base.OnDrag(eventData);

			if (dragging == true)
			{
				var anchoredPosition = TargetTransform.anchoredPosition;

				anchoredPosition.x = Mathf.Clamp(anchoredPosition.x, -Size.x, Size.x);
				anchoredPosition.y = Mathf.Clamp(anchoredPosition.y, -Size.y, Size.y);

				TargetTransform.anchoredPosition = anchoredPosition;
			}

			UpdateScaledValue();
		}

		protected virtual void Update()
		{
			if (dragging == false)
			{
				// Get the current anchored position
				var anchoredPosition = TargetTransform.anchoredPosition;

				// Get t value
				var factor = LeanTouch.GetDampenFactor(Dampening, Time.deltaTime);

				// Dampen the current position toward the target
				anchoredPosition = Vector2.Lerp(anchoredPosition, Vector2.zero, factor);

				// Write updated anchored position
				TargetTransform.anchoredPosition = anchoredPosition;

				UpdateScaledValue();
			}

			if (OnSet != null)
			{
				OnSet.Invoke(ScaledValue);
			}
		}

		private void UpdateScaledValue()
		{
			// Get the current anchored position
			var anchoredPosition = TargetTransform.anchoredPosition;

			// Scale X
			if (Size.x > 0.0f)
			{
				ScaledValue.x = anchoredPosition.x / Size.x;
			}
			else
			{
				ScaledValue.x = 0.0f;
			}

			// Scale Y
			if (Size.y > 0.0f)
			{
				ScaledValue.y = anchoredPosition.y / Size.y;
			}
			else
			{
				ScaledValue.y = 0.0f;
			}
		}
	}
}