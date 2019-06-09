using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableCommand;
    public Rigidbody rb;

    void Awake()
    {
        variableCommand = GameObject.FindGameObjectWithTag("JoystickCity").GetComponent<VariableJoystick>();
        rb = GameObject.FindGameObjectWithTag("City").GetComponent<Rigidbody>();
    }


        public void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableCommand.Vertical + Vector3.right * variableCommand.Horizontal;
        rb.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }
}

