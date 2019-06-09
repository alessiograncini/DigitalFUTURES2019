using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Waypoints : MonoBehaviour
{

    public Transform target;
    public float speed = 30.0f;
    Vector3 originalPos;
    public Slider mainSlider;



    void Awake()
    {
        originalPos = gameObject.transform.position;
        mainSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        mainSlider.value = -4.8f;
        //float step = mainSlider.value;
        { if (gameObject.tag == "TagSlider")
            {
             float step = mainSlider.value;
                transform.position = Vector3.MoveTowards(originalPos, target.position, -1 * step);

            }
        }
      

    }


    // public void ValueChangeCheck()

    //void Update()

         private void OnValueChanged()
    {
        if (gameObject.tag == "TagSlider")

        {
          
            float step = mainSlider.value;
            //* Time.deltaTime;
            transform.position = Vector3.MoveTowards(originalPos, target.position, -1 *step);

            //if (transform.position == target.position) 

            //{
            //    transform.position = Vector3.MoveTowards(target.position, originalPos, step);
            //}


        }
    }
        public void AdjustSpeed(float newSpeed)
        {

            speed = newSpeed;

        }
    }
   






