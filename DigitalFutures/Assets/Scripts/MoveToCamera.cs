    using UnityEngine;
    using System.Collections;


    public class MoveToCamera : MonoBehaviour
    {
        // Adjust the speed for the motion
        public float speed = 1.0f;

        // The target defining the vector 
        public Transform target;
        public Transform citymoves;

        // The condition toggle 
        public Transform decider;

        // The condition toggle for the rigidbody of the City 
        public Rigidbody rb;

    // The HitObejct referencing the object children of the model
    // The Position Target referencing the object children of the model

         //public Transform receiverHO;
         //public Transform receiverMTC;
         //public Transform postohit;


    void Awake()
        {
        //get the decider toggle 
        decider = GameObject.FindGameObjectWithTag("Decider").GetComponent<Transform>();

        //set it to false 
        decider.gameObject.SetActive(false);


        //get the other guys you referenced 
        target = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        citymoves = GameObject.FindGameObjectWithTag("City").GetComponent<Transform>();
        rb = GameObject.FindGameObjectWithTag("City").GetComponent<Rigidbody>();

        //referencing the city children receiver and outsider

        //receiverHO = GameObject.FindGameObjectWithTag("ReceiverHO").GetComponent<Transform>();
        //receiverMTC = GameObject.FindGameObjectWithTag("ReceiverMTC").GetComponent<Transform>();
        //postohit = GameObject.FindGameObjectWithTag("PosToHit").GetComponent<Transform>();



    }

        void FixedUpdate()
        {

        // getting the same location/position of PosToHit and Receiver 


        //postohit.transform.position = receiverHO.transform.position;
        //this.transform.position = receiverMTC.transform.position;

        //here goes the input // might be chaging based on device 

        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || (Input.GetMouseButtonDown(0)))

        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "PosToHit")
            {
                if (hit.collider != null)
                {
                    decider.gameObject.SetActive(true);
                  
                }
            }
        }

        // now the toggle decider is settle to true so we move 

        if (decider.gameObject.activeInHierarchy == true)
          
                {

                

                //city becomes parent of position where we are going 
                citymoves.transform.parent = this.transform;

                //Let's move our position a step closer to the target.

                float step = speed * Time.deltaTime; // calculate distance to move
                transform.position = Vector3.MoveTowards(transform.position, target.position, step); //calculate the vector 

                // Let's set the city Kinematic 
                 rb.isKinematic = true;
               

        }


        // Stop the attractor - Check if the position of the cube and sphere are approximately equal.


        if (Vector3.Distance(transform.position, target.position) < 0.001f)
        {
            decider.gameObject.SetActive(false);

        }


    }
}




