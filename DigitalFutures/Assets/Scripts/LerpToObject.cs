using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpToObject : MonoBehaviour
{
 public Transform endMarker = null; // create an empty gameobject and assign in inspector
                                    //public Transform ObjecttoHit = null; // create an empty gameobject and assign in inspector

    public float speed = 1.0F;
    private float startTime;
    private float journeyLength;


    void Awake ()
    {
        //ObjecttoHit= GameObject.FindGameObjectWithTag("StudentCapture01").GetComponent<Transform>();
        endMarker = GameObject.FindGameObjectWithTag("FinalPos").GetComponent<Transform>();

        startTime = Time.time;

        journeyLength = Vector3.Distance(transform.position, endMarker.position);
    }

    void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "StudentCapture01")
            {
                if (hit.collider != null)
                {

                   // transform.position = endMarker.position;



                    // Distance moved = time * speed.
                    float distCovered = (Time.time - startTime) * speed;

                    // Fraction of journey completed = current distance divided by total distance.
                    float fracJourney = distCovered / journeyLength;

                    // Set our position as a fraction of the distance between the markers.
                    transform.position = Vector3.Lerp(transform.position, endMarker.position, fracJourney);


                    //Vector3.Lerp(transform.position, endMarker.position, Time.deltaTime);
                }
            }
        }
 }
 }