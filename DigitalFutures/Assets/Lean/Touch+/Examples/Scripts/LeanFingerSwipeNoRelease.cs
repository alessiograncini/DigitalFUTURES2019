using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This component detects swipes while the finger is touching the screen.</summary>
	public class LeanFingerSwipeNoRelease : LeanFingerSwipe
	{
		// This class will store an association between a Finger and cooldown values
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public bool Cooldown; // Currently waiting for cooldown to finish?
			public float CooldownTime; // Current cooldown time in seconds
		}

		[Tooltip("Allow multiple swipes for each finger press?")]
		public bool AllowMultiple = true;

		[Tooltip("If multiple swipes are allowed, this is the minimum amount of seconds between each OnFingerSwipe call")]
		public float MultipleSwipeDelay = 0.5f;

		// This stores all the links
		private List<FingerData> fingerDatas = new List<FingerData>();

		protected override void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerSet += FingerSet;
			LeanTouch.OnFingerUp  += FingerUp;
		}

		protected override void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerSet -= FingerSet;
			LeanTouch.OnFingerUp  -= FingerUp;
		}

		protected virtual void Update()
		{
			// Loop through all links
			if (fingerDatas != null)
			{
				for (var i = 0; i < fingerDatas.Count; i++)
				{
					var fingerData = fingerDatas[i];

					// Decrease cooldown?
					if (fingerData.Cooldown == true && AllowMultiple == true)
					{
						fingerData.CooldownTime -= Time.deltaTime;

						if (fingerData.CooldownTime <= 0.0f)
						{
							fingerData.Cooldown = false;
						}
					}
				}
			}
		}

		private void FingerSet(LeanFinger finger)
		{
			// Ignore this finger?
			if (IgnoreStartedOverGui == true && finger.StartedOverGui == true)
			{
				return;
			}

			if (IgnoreIsOverGui == true && finger.IsOverGui == true)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelectedBy(finger) == false)
			{
				return;
			}

			// Get link and skip if on cooldown
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			if (fingerData.Cooldown == true)
			{
				return;
			}

			// The scaled delta position magnitude required to register a swipe
			var swipeThreshold = LeanTouch.Instance.SwipeThreshold;

			// The amount of seconds we consider valid for a swipe
			var tapThreshold = LeanTouch.CurrentTapThreshold;

			// Get the scaled delta position between now, and 'swipeThreshold' seconds ago
			var recentDelta = finger.GetSnapshotScreenDelta(tapThreshold);

			// Has the finger recently swiped?
			if (recentDelta.magnitude > swipeThreshold)
			{
				if (CheckSwipe(finger, recentDelta) == true)
				{
					// Begin cooldown
					fingerData.CooldownTime = MultipleSwipeDelay;
					fingerData.Cooldown     = true;
				}
			}
		}

		private void FingerUp(LeanFinger finger)
		{
			// Get link and reset cooldown
			var link = LeanFingerData.Find(ref fingerDatas, finger);

			if (link != null)
			{
				link.Cooldown = false;
			}
		}
	}
}