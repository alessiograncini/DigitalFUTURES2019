// Smooth towards the target

using UnityEngine;
using System.Collections;

public class DriftToPosHit : MonoBehaviour
{
    public Transform target;
    public float smoothTime = 0.3F;
    private Vector3 velocity = Vector3.zero;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("FinalPos").GetComponent<Transform>();
    }

    void Update()

    {


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "StudentCapture01")
            {
                if (hit.collider != null)


                { 

                // Define a target position above and behind the target transform
                Vector3 targetPosition = target.TransformPoint(new Vector3(0, 5, -10));

                // Smoothly move the camera towards that target position
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);



            }
        }
    }

}


}




