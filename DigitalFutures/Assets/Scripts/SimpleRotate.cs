using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotate : MonoBehaviour
{

    public float speed; 
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        transform.Rotate(Vector3.right * speed * Time.deltaTime);
        transform.Rotate(Vector3.up * speed * Time.deltaTime);

    }


}