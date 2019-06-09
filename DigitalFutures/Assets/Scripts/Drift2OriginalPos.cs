using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Drift2OriginalPos : MonoBehaviour
{
    
    Vector3 startPosition, driftPosition;
    Quaternion startRotation, driftRotation;

          
    public float driftSeconds = 3;
    private float driftTimer = 0;
    private bool isDrifting = false;

       void Start()

    {
         
            startPosition = transform.position;
            startRotation = transform.rotation;

    }

    private void StartDrift ()
    {
        isDrifting = true;
        driftTimer = 0;

        driftPosition = transform.position;
        driftRotation = transform.rotation;


        //This is conflicting with the rigidbody you already added.if commented in, let you go your objects in the 0,0,0 axis that is what unity consider as Vector zero

        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //if (rb !=null)
        //{

        //    rb.velocity = Vector3.zero;
        //    rb.constraints = RigidbodyConstraints.FreezeAll;
      
        //}
    }
    private void StopDrift()
    {
        isDrifting = false;
        transform.position = startPosition;
        transform.rotation = startRotation;

        //Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        //if (rb != null)
        //{

        //    rb.velocity = Vector3.zero;
        //    rb.constraints = RigidbodyConstraints.None;
        //}
    }

    void Update () 
    {
        if (Input.GetMouseButton(1))

            StartDrift();

        if (isDrifting)

        {

            driftTimer += Time.deltaTime;
            if (driftTimer > driftSeconds)
            {
                StopDrift();
            }

            else

            {
                float ratio = driftTimer / driftSeconds;
                transform.position = Vector3.Lerp(driftPosition, startPosition, ratio);
                transform.rotation = Quaternion.Slerp(driftRotation, startRotation, ratio);

            }
        }
    }
}
