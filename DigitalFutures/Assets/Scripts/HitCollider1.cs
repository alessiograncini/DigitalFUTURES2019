using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;




public class HitCollider1 : MonoBehaviour
{

    //Elements too renference for instantiation



    public Transform PositionToGo;


    public float speed = 30.0f;


    private void Awake()


    {

        PositionToGo = GameObject.FindGameObjectWithTag("StudentCapture01").GetComponent<Transform>();

    }


    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit) && hit.transform.tag == "HitThis")
            {
                if (hit.collider != null)
                {
                    transform.position = Vector3.Lerp(transform.position, PositionToGo.position, Time.deltaTime * speed);


                }
            }

        }
    }
}
