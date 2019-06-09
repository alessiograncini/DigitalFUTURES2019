using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	/// <summary>This script displays the result of LeanMultiTap in a Text element.</summary>
	public class LeanMultiTapInfo : MonoBehaviour
	{
		[Tooltip("The Text element you want to display the info in")]
		public Text Text;

		// This method is called from the LeanMultiTap event
		public void MultiTap(int count, int highest)
		{
			if (Text != null)
			{
				Text.text = "You just multi-tapped " + count + " time(s) with " + highest + " finger(s)";
			}
		}
	}
}