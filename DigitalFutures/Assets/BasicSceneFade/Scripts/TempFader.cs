using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Codevenient.BasicSceneFade {

	// This component is responsible for the logic and drawing of the fade effect.
	// It is attached to the TempFader GameObject that is created programmatically
    // by the FadeInOnStart or FadeTransition class when a fade animation is started.
    // This component destroys TempFader GameObject when the fade animation is over.
	public class TempFader : MonoBehaviour {

		// Private variable that contains the TempFader instance.
        // This is used to check that there is only one TempFader component in a scene at the same time.
		static TempFader instance;

		// The texture used for the fade effect
		Texture2D texture;

		// The current alpha value of the rectangle that is drawn to the screen
		float alpha = 0f;

		// The time it takes to complete one fade out, or fade in
		float duration = 1.0f;

		// The time delay before fading in
		// Makes fading into the current scene or fading to a new scene look better
		float delay = 0.2f;

		// Helper function to create a TempFader GameObject
		public static TempFader CreateTempFader(Color color, float duration, float delay) {
			var go = new GameObject("TempFader");
			TempFader tempFader = go.AddComponent<TempFader> ();
			if (tempFader == null)
				return null;
			else {
				tempFader.SetProperties (color, duration, delay);
				return tempFader;
			}
		}
		
        // Function to set the TempFader component's properties after it is created
		public void SetProperties(Color color, float duration, float delay) {
			this.duration = duration;
			this.delay = delay;
			
			// Prepare the texture for drawing to the screen
			texture = new Texture2D (1, 1);
			texture.SetPixel( 0, 0, color );
			texture.Apply();
		}
		
		// Coroutine for fade transition to a new scene. The action parameter contains an
		// action that should take place after fading out, normally, loading the new scene
		public IEnumerator FadeToSceneCR(System.Action action) {
			
			// Make sure that the GameObject is taken to the next scene,
			// to do the fade in animation
			DontDestroyOnLoad (this.gameObject);
			
			// Start fading out
			yield return StartCoroutine(FadeOutCR());
			
			// Perform the action that was passed as a parameter, normally,
			// loading the new scene
			action();
			
			// Start fading in
			yield return StartCoroutine(FadeInCR());
		}
		
        // Coroutine for fading into the current scene
		public IEnumerator FadeInCurrentSceneCR() {
			// Start fading in
			yield return StartCoroutine (FadeInCR ());
		}

		void Awake()
		{
			// If there is already another TempFader components, this one is not needed.
            // Destroy it. This way, there are never multiple TempFader components present in the scene.
			if (instance != null) {
				Destroy(gameObject);
				return;
			}

			instance = this;
		}

		// Code for drawing a rectangle that covers the entire screen with a certain alpha value
		void OnGUI()
		{
			if (texture != null) {
				// Set the alpha value
				GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);

				// Make sure that the rectangle is rendered above all other objects
				GUI.depth = -1000;

				// Draw the rectangle
				GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), texture);
			}
		}

		// Helper coroutine for fading out
		IEnumerator FadeOutCR() {
			// Make sure the alpha has the correct starting value
			alpha = 0f;

			// Gradually increase the alpha value
			while(alpha < 1f)
			{
				alpha = Mathf.MoveTowards(alpha, 1f, 1f / duration * Time.deltaTime);
				yield return null;
			}
		}

        // Helper coroutine for fading in
        IEnumerator FadeInCR()
        {
            // Make sure that the alpha has the correct starting value
            alpha = 1f;

            // Extra delay before fading in, to make the fade animation look better
            yield return new WaitForSeconds(delay);

            // Gradually decrease the alpha value
            while (alpha > 0f)
            {
                alpha = Mathf.MoveTowards(alpha, 0f, 1f / duration * Time.deltaTime);
                yield return null;
            }

            // After fading in, the GameObject should always be destroyed
            Destroy(this.gameObject);
        }
	}

}