using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour {

    [Tooltip("The name of the scene to fade to.")]
	public string nextSceneName = "FadeTransitionUsingCode";

	void Start () {
		Button btn = this.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick()
	{
        // Get the FadeTransition component and tell it to fade to the next scene
		Codevenient.BasicSceneFade.FadeTransition.Instance.FadeToScene (nextSceneName);
	}

}
