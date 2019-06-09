using UnityEngine;

namespace Lean.Touch
{
	/// <summary>This component copies path points to a line renderer.</summary>
	[ExecuteInEditMode]
	public class LeanPathLineRenderer : MonoBehaviour
	{
		[Tooltip("The path that will be used")]
		public LeanPath Path;

		[Tooltip("The line renderer the path will be output to")]
		public LineRenderer LineRenderer;

		[Tooltip("The amount of lines between each path point")]
		public int Smoothing = 1;

		protected virtual void Update()
		{
			if (Path != null && LineRenderer != null)
			{
				var pointCount = Path.PointCount;

				if (Smoothing > 1)
				{
					var smoothedPointCount = Path.GetPointCount(Smoothing);
					var smoothedStep       = 1.0f / Smoothing;

					SetPointCount(smoothedPointCount);

					for (var i = 0; i < smoothedPointCount; i++)
					{
						SetPoint(i, Path.GetSmoothedPoint(i * smoothedStep));
					}
				}
				else
				{
					SetPointCount(pointCount);

					for (var i = 0; i < pointCount; i++)
					{
						SetPoint(i, Path.GetPoint(i));
					}
				}
			}
		}

		private void SetPointCount(int pointCount)
		{
			LineRenderer.positionCount = pointCount;
		}

		private void SetPoint(int index, Vector3 point)
		{
			LineRenderer.SetPosition(index, point);
		}
	}
}