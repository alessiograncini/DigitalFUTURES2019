using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This script shows you how you can check tos ee which part of the screen a finger is on, and work accordingly.</summary>
	public class LeanFingerShoot : MonoBehaviour
	{
		// This class will store an association between a Finger and a LineRenderer instance
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public LineRenderer Line; // The line associated with this finger
		}

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreStartedOverGui = true;

		[Tooltip("Ignore fingers with IsOverGui?")]
		public bool IgnoreIsOverGui;

		[Tooltip("The line prefab")]
		public LineRenderer LinePrefab;

		[Tooltip("The distance from the camera the line points will be spawned in world space")]
		public float Distance = 1.0f;

		[Tooltip("The prefab you want to throw")]
		public GameObject ShootPrefab;

		[Tooltip("The strength of the throw")]
		public float Force = 1.0f;

		public float Thickness = 1.0f;

		public float Length = 1.0f;

		[Tooltip("The maximum amount of fingers used")]
		public int MaxLines;

		[Tooltip("The camera the translation will be calculated using (default = MainCamera)")]
		public Camera Camera;

		private List<FingerData> fingerDatas = new List<FingerData>();

		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerDown += FingerDown;
			LeanTouch.OnFingerSet  += FingerSet;
			LeanTouch.OnFingerUp   += FingerUp;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerDown -= FingerDown;
			LeanTouch.OnFingerSet  -= FingerSet;
			LeanTouch.OnFingerUp   -= FingerUp;
		}

		// Override the WritePositions method from LeanDragLine
		protected virtual void WritePositions(LineRenderer line, LeanFinger finger)
		{
			// Get start and current world position of finger
			var start = finger.GetStartWorldPosition(Distance);
			var end   = finger.GetWorldPosition(Distance);

			// Limit the length?
			if (start != end)
			{
				end = start + Vector3.Normalize(end - start) * Length;
			}

			// Write positions
			line.positionCount = 2;

			line.startWidth = Thickness;
			line.endWidth   = Thickness;

			line.SetPosition(0, start);
			line.SetPosition(1, end);
		}

		private void FingerDown(LeanFinger finger)
		{
			// Ignore?
			if (MaxLines > 0 && fingerDatas.Count >= MaxLines)
			{
				return;
			}

			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			// Make new link
			var leanFingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			// Create LineRenderer instance for this link
			leanFingerData.Line = Instantiate(LinePrefab);

			// Add new link to list
			fingerDatas.Add(leanFingerData);
		}

		private void FingerSet(LeanFinger finger)
		{
			// Try and find the link for this finger
			var leanFingerData = LeanFingerData.Find(ref fingerDatas, finger);

			// Link exists?
			if (leanFingerData != null && leanFingerData.Line != null)
			{
				WritePositions(leanFingerData.Line, leanFingerData.Finger);
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			// Try and find the link for this finger
			var link = LeanFingerData.Find(ref fingerDatas, finger);

			// Link exists?
			if (link != null)
			{
				// Remove link from list
				fingerDatas.Remove(link);

				// Destroy line GameObject
				if (link.Line != null)
				{
					Destroy(link.Line.gameObject);
				}

				Shoot(finger);
			}
		}

		private void Shoot(LeanFinger finger)
		{
			// Start and end points of the drag
			var start = finger.GetStartWorldPosition(Distance);
			var end   = finger.GetWorldPosition(Distance);

			if (start != end)
			{
				// Vector between points
				var direction = Vector3.Normalize(end - start);

				// Angle between points
				var angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

				// Instance the prefab, position it at the start point, and rotate it to the vector
				var instance = Instantiate(ShootPrefab);

				instance.transform.position = start;
				instance.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);

				// Apply 3D force?
				var rigidbody3D = instance.GetComponent<Rigidbody>();

				if (rigidbody3D != null)
				{
					rigidbody3D.velocity = direction * Force;
				}

				// Apply 2D force?
				var rigidbody2D = instance.GetComponent<Rigidbody2D>();

				if (rigidbody2D != null)
				{
					rigidbody2D.velocity = direction * Force;
				}
			}
		}
	}
}