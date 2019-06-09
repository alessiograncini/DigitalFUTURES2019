using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codevenient.BasicSceneFade {

	// This component is used in the FadeInOnStart Prefab,
	// which needs to be added to your scene if you want an
	// automatic fade in animation at the start of the scene.
	public class FadeInOnStart : MonoBehaviour {

        // The color of the fade effect, which can be set by the user
        [Tooltip("The color used for the fade effect.")]
        public Color fadeColor = Color.black;

        // The  duration of the fade in. Can be set by the user
        [Tooltip("The duration of the fade in.")]
        public float duration = 1f;

        // The time delay before fading in
        [Tooltip("The time delay before fading in.")]
        public float delay = 0.2f;

        // Private variable to store the main FadeInOnStart component. The
        // first FadeInOnStart component for which Awake is called becomes the main component.
        static FadeInOnStart instance;

		void Awake()
		{
            if (instance != null) {
                // If the instance variable is not null, there is already a FadeInOnStart component in the scene.
                // In that case, show a warning message
                Debug.LogWarning("Multiple FadeInOnStart components found, it can be unpredictable which one is used. " +
                                 "It is recommended to have only one FadeInOnStart component in your scene.");
                return;
            }
            // If this is the first FadeInOnStart component for which Awake is called, register it as the main instance
            instance = this;
		}

		void OnDestroy()
		{
            // If this component is registered as the main FadeInOnStart component, the reference should now be removed
            if (instance == this)
                instance = null;
		}
		
		void Start() {
            // If this is the main FadeInOnStart component, start the fade in
            if (this == instance)
            {
                // Create a new TempFader GameObject, which will take care of the fade animation and then destroy itself
                TempFader tempFader = TempFader.CreateTempFader(fadeColor, duration, delay);
                if (tempFader != null)
                {
                    StartCoroutine(tempFader.FadeInCurrentSceneCR());
                }
            }
		}

	}

}