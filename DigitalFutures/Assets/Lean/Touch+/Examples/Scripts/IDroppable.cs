using UnityEngine;

namespace Lean.Touch
{
	/// <summary>Any component implementing this interface will make it compatible with LeanSelectableDrop, allowing you to perform a specific action when the current object is dropped on something.</summary>
	public interface IDroppable
	{
		void OnDrop(GameObject droppedGameObject, LeanFinger finger);
	}
}