using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SliderOnValue : MonoBehaviour
{
    public GameObject cube;
    public Transform target;
    public float speed = 5.0f;

    public Slider sliderInstance;
    public float minimumSlider;
    public float maximumSlider;


    // Start is called before the first frame update

    void Start()
    {
        sliderInstance.minValue = minimumSlider;
        sliderInstance.maxValue = maximumSlider;
        sliderInstance.value = minimumSlider;
    }

    // Update is called once per frame
    void Update ()

    {
        if (sliderInstance.value == sliderInstance.minValue)
        {

            float step = speed * Time.deltaTime;
            cube.transform.position = cube.transform.position;
        }

        if (sliderInstance.value == sliderInstance.maxValue)
        {

            float step = speed * Time.deltaTime;
            cube.transform.position = Vector3.MoveTowards (target.position,cube.transform.position, step);
        }

        if (cube.transform.position == target.position)
        {
            cube.transform.position = cube.transform.position;

        }
        //if ((sliderInstance.value > minimumSlider)
        //{
        //    float step = speed * Time.deltaTime;
        //    cube.transform.position = Vector3.MoveTowards(target.position, cube.transform.position, step);
        //}

    }

    public void AdjustSpeed(float newSpeed)
    {

        speed = newSpeed;

    }
}
