using UnityEngine;
using System.Collections.Generic;

namespace Lean.Touch
{
	/// <summary>This script will draw the path each finger has taken since it started being pressed.</summary>
	public class LeanFingerTrailFade : MonoBehaviour
	{
		// This class will store an association between a finger and a LineRenderer instance
		[System.Serializable]
		public class FingerData : LeanFingerData
		{
			public LineRenderer Line; // The LineRenderer instance associated with this link
			public float        Life; // The amount of seconds until this link disappears
		}

		[Tooltip("Ignore fingers with StartedOverGui?")]
		public bool IgnoreIfStartedOverGui = true;

		[Tooltip("Must RequiredSelectable.IsSelected be true?")]
		public LeanSelectable RequiredSelectable;

		[Tooltip("The line prefab")]
		public LineRenderer LinePrefab;

		[Tooltip("The conversion method used to find a world point from a screen point")]
		public LeanScreenDepth ScreenDepth;

		[Tooltip("How many seconds it takes for each line to disappear after a finger is released")]
		public float FadeTime = 1.0f;

		[Tooltip("The maximum amount of fingers used")]
		public int MaxLines;

		public Color StartColor = Color.white;

		public Color EndColor = Color.white;

		// This stores all the links between fingers and LineRenderer instances
		private List<FingerData> fingerDatas = new List<FingerData>();

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
	
		protected virtual void OnEnable()
		{
			// Hook events
			LeanTouch.OnFingerSet += FingerSet;
			LeanTouch.OnFingerUp  += FingerUp;
		}

		protected virtual void OnDisable()
		{
			// Unhook events
			LeanTouch.OnFingerSet += FingerSet;
			LeanTouch.OnFingerUp  += FingerUp;
		}

		protected virtual void Update()
		{
			// Loop through all links
			for (var i = 0; i < fingerDatas.Count; i++)
			{
				var fingerData = fingerDatas[i];

				// Has this link's finger been unlinked? (via OnFingerUp)
				if (fingerData.Finger == null)
				{
					// Remove life from the link
					fingerData.Life -= Time.deltaTime;

					// Is the link still alive?
					if (fingerData.Life > 0.0f)
					{
						// Make sure FadeTime is set to prevent divide by 0
						if (FadeTime > 0.0f)
						{
							// Find the life to FadeTime 0..1 ratio
							var ratio = fingerData.Life / FadeTime;

							// Copy the start & end colors and fade them by the ratio
							var color0 = StartColor;
							var color1 =   EndColor;

							color0.a *= ratio;
							color1.a *= ratio;

							// Write the new colors
							fingerData.Line.startColor = color0;
							fingerData.Line.endColor   = color1;
						}
					}
					// Kill the link?
					else
					{
						// Remove link from list
						fingerDatas.Remove(fingerData);

						// Destroy line GameObject
						Destroy(fingerData.Line.gameObject);
					}
				}
			}
		}

		// Override the WritePositions method from LeanDragLine
		protected virtual void WritePositions(LineRenderer line, LeanFinger finger)
		{
			// Reserve one vertex for each snapshot
			line.positionCount = finger.Snapshots.Count;

			// Loop through all snapshots
			for (var i = 0; i < finger.Snapshots.Count; i++)
			{
				var snapshot = finger.Snapshots[i];

				// Get the world postion of this snapshot
				var worldPoint = ScreenDepth.Convert(snapshot.ScreenPosition, gameObject);

				// Write position
				line.SetPosition(i, worldPoint);
			}
		}

		private void FingerSet(LeanFinger finger)
		{
			if (MaxLines > 0 && fingerDatas.Count >= MaxLines)
			{
				return;
			}

			if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
			{
				return;
			}

			// Get link for this finger and write positions
			var fingerData = LeanFingerData.FindOrCreate(ref fingerDatas, finger);

			if (fingerData.Line == null)
			{
				fingerData.Line = Instantiate(LinePrefab);
			}

			WritePositions(fingerData.Line, fingerData.Finger);
		}

		private void FingerUp(LeanFinger finger)
		{
			// If link with this finger exists, null finger and assign life, so it can be destroyed later
			var fingerData = LeanFingerData.Find(ref fingerDatas, finger);

			if (fingerData != null)
			{
				// Call up method
				LinkFingerUp(fingerData);

				fingerData.Finger = null;
				fingerData.Life   = FadeTime;
			}
		}

		protected virtual void LinkFingerUp(FingerData link)
		{
		}
	}
}