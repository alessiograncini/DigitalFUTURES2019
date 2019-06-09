using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Codevenient.BasicSceneFade {

	// This component is used in the FadeTransition Prefab, which
	// needs to be added to your scene if you want to fade to other scenes.
	// For example, you can fade to another scene using code by calling
	// Codevenient.BasicSceneFade.Instance.FadeToScene("MySceneName").
	// Alternatively, you can set up a transition using the Inspector,
	// for example by adding FadeToScene to the list of functions of the
	// OnClick () event of a Button.
	public class FadeTransition : MonoBehaviour {

		// The color of the fade effect, which can be set by the user
		[Tooltip("The color used for the fade effect.")]
		public Color fadeColor = Color.black;

		// The  duration of the fade out. The fade in has the same duration. Can be set by the user
		[Tooltip("The duration of the fade out. The fade in has the same duration.")]
		public float duration = 1f;

		// The time delay between fading in and out
		[Tooltip("The time delay after fading out, and before fading in.")]
		public float delay = 0.2f;

        // Keep a list of all FadeTransition components in a scene
        public static List<FadeTransition> Instances = new List<FadeTransition>();

        // Public variable for obtaining the FadeTransition component, to start a fade transition using code.
        // Note: when using this variable, it is recommended that there is only one FadeTransition
        // component per scene. If there are multiple FadeTransition components, the first one in the
        // Instances list will be returned. However, it can be unpredictable which one comes first, because the
        // Awake order is unpredictable. Therefore, this situation should be avoided, and the user will see a warning message.
        public static FadeTransition Instance
        {
            get
            {
                if (Instances.Count == 0)
                {
                    // If there is no FadeTransition component, produce a warning, and return null
                    Debug.LogWarning("No FadeTransition component was found. Please add the FadeTransition Prefab to your scene.");
                    return null;
                }
                else if (Instances.Count > 1)
                {
                    // If there are multiple FadeTransition components, produce a warning, and return only the first in the list
                    Debug.LogWarning("Multiple FadeTransition components found, it can be unpredictable which one is used. " +
                                     "When using FadeTransition.Instance, it is recommended to have only one FadeTransition component in your scene.");
                    return Instances[0];
                } else {
                    return Instances[0];
                }
            }
        }

        void Awake()
        {
            // Add the FadeTransition component to the list of instances
            Instances.Add(this);
        }

        void OnDestroy()
        {
            // Remove the FadeTransition component from the list of instances
            Instances.Remove(this);
        }

		// Function to be called to fade to another scene using the scene's build index
		// Note: this only works if the scene has been added to the project's Build Settings
		public void FadeToScene(int sceneBuildIndex) {
            // Create a new TempFader GameObject, which will take care of the fade animation and then destroy itself.
			TempFader tempFader = TempFader.CreateTempFader(fadeColor, duration, delay);
			if (tempFader != null) {
				StartCoroutine(tempFader.FadeToSceneCR(() => {SceneManager.LoadScene(sceneBuildIndex);}));
			}
		}

		// Function to be called to fade to another scene using the scene's name
        // Note: this only works if the scene has been added to the project's Build Settings
		public void FadeToScene(string sceneName ) {
            // Create a new TempFader GameObject, which will take care of the fade animation and then destroy itself.
			TempFader tempFader = TempFader.CreateTempFader(fadeColor, duration, delay);
			if (tempFader != null) {
				StartCoroutine(tempFader.FadeToSceneCR(() => {SceneManager.LoadScene(sceneName);}));
			}
		}

	}

}