using UnityEngine;
using UnityEngine.UI;

namespace Lean.Touch
{
	/// <summary>This component can be used with LeanSelect.Reselect = Select Again setting to count how many times you selected it.</summary>
	public class LeanSelectCount : MonoBehaviour
	{
		[Tooltip("The text element we will modify")]
		public Text NumberText;

		[Tooltip("The amount of times this GameObject has been reselected")]
		public int ReselectCount;

		public void OnSelect(LeanFinger finger)
		{
			ReselectCount += 1;

			NumberText.text = ReselectCount.ToString();
		}

		public void OnDeselect()
		{
			ReselectCount = 0;

			NumberText.text = "";
		}
	}
}