using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleSliderRight : MonoBehaviour
{
    // Assign in the inspector
    public GameObject objectToRotate;
    public Slider slider;

    // Preserve the original and current orientation
    private float previousValue;


    void Awake()
    {
        slider = GameObject.FindGameObjectWithTag("ScaleSlider").GetComponent<Slider>();
        objectToRotate = GameObject.FindGameObjectWithTag("City");

        // Assign a callback for when this slider changes
        this.slider.onValueChanged.AddListener(this.OnSliderChanged);

        // And current value
        this.previousValue = this.slider.value;
    }

    void OnSliderChanged(float value)
    {
        // How much we've changed
        float delta = value - this.previousValue;
        this.objectToRotate.transform.localScale = new Vector3(delta*3.8f, delta*1, delta*5.2f);

        // Set our previous value for the next change
       // this.previousValue = value;




    }
}



//objectToRotate.gameObject.transform.localScale = new Vector3(updatescale, updatescale, updatescale);