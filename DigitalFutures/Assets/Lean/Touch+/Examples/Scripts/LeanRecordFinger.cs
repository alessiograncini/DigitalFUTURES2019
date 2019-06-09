using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This shows you how to record a finger's movement data that can later be replayed.</summary>
	public class LeanRecordFinger : MonoBehaviour
	{
		[Tooltip("The cursor used to show the recording")]
		public Transform Cursor;

		[Tooltip("The conversion method used to find a world point from a screen point")]
		public LeanScreenDepth ScreenDepth;

		[Tooltip("Is the recording playing?")]
		public bool Playing;

		[Tooltip("The position of the playback in seconds")]
		public float PlayTime;

		// Currently recorded snapshots
		private List<LeanSnapshot> snapshots = new List<LeanSnapshot>();

		public void ClickPlay()
		{
			Playing  = true;
			PlayTime = 0.0f;
		}

		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerSet += OnFingerSet;
			LeanTouch.OnFingerUp  += OnFingerUp;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerSet -= OnFingerSet;
			LeanTouch.OnFingerUp  -= OnFingerUp;
		}

		protected virtual void Update()
		{
			// Is the recording being played back?
			if (Playing == true)
			{
				PlayTime += Time.deltaTime;

				var screenPosition = default(Vector2);

				if (LeanSnapshot.TryGetScreenPosition(snapshots, PlayTime, ref screenPosition) == true)
				{
					Cursor.position = ScreenDepth.Convert(screenPosition, gameObject);
				}
			}
		}

		private void OnFingerSet(LeanFinger finger)
		{
			if (finger.StartedOverGui == false)
			{
				Playing = false;

				if (Cursor != null)
				{
					Cursor.position = ScreenDepth.Convert(finger.ScreenPosition, gameObject);
				}
			}
		}

		private void OnFingerUp(LeanFinger finger)
		{
			if (finger.StartedOverGui == false)
			{
				CopySnapshots(finger);
			}
		}

		private void CopySnapshots(LeanFinger finger)
		{
			// Clear old snapshots
			snapshots.Clear();

			// Go through all new snapshots
			for (var i = 0; i < finger.Snapshots.Count; i++)
			{
				// Copy data into new snapshot
				var snapshotSrc = finger.Snapshots[i];
				var snapshotCpy = new LeanSnapshot();

				snapshotCpy.Age            = snapshotSrc.Age;
				snapshotCpy.ScreenPosition = snapshotSrc.ScreenPosition;

				// Add new snapshot to list
				snapshots.Add(snapshotCpy);
			}
		}
	}
}