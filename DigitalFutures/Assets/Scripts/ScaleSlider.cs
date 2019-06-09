using UnityEngine;
using UnityEngine.UI;

public class ScaleSlider : MonoBehaviour
{
    // Assign in the inspector
    public GameObject objectToScale;
    public Slider slider;

    // Preserve the original and current orientation
    private float previousValue;

    void Awake()
    {
        slider = GameObject.FindGameObjectWithTag("ScaleSlider").GetComponent<Slider>();
        objectToScale = GameObject.FindGameObjectWithTag("City");

        // Assign a callback for when this slider changes
        this.slider.onValueChanged.AddListener(this.OnSliderChanged);

        // And current value
        this.previousValue = this.slider.value;
    }

    void OnSliderChanged(float value)
    {
        // How much we've changed
       float delta = value - this.previousValue;
        objectToScale.transform.localScale = new Vector3(delta, delta, delta);

        // Set our previous value for the next change
        this.previousValue = value;
    }
}