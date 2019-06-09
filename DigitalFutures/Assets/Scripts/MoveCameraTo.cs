using UnityEngine;
using System.Collections;

// Vector3.MoveTowards example.

// A cube can be moved around the world. It is kept inside a 1 unit by 1 unit
// xz space. A small, long cylinder is created and positioned away from the center of
// the 1x1 unit. The cylinder is moved between two locations. Each time the cylinder is
// positioned the cube moves towards it. When the cube reaches the cylinder the cylinder
// is re-positioned to the other location. The cube then changes direction and moves
// towards the cylinder again.
//
// A floor object is created for you.
//
// To view this example, create a new 3d Project and create a Cube placed at
// the origin. Create Example.cs and change the script code to that shown below.
// Save the script and add to the Cube.
//
// Now run the example.

public class MoveCameraTo: MonoBehaviour
{
    // Adjust the speed for the application.
    public float speed = 1.0f;

    // The target (cylinder) position.
    public Transform target;

    void Awake()

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
                    // Move our position a step closer to the target.
                    float step = speed * Time.deltaTime; // calculate distance to move
                    transform.position = Vector3.MoveTowards(transform.position, target.position, step);

                    // Check if the position of the cube and sphere are approximately equal.

                }


            }
        }
    }
}
