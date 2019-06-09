using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetComponentsInChildren : MonoBehaviour
{
    public Component[] Waypoints;

    void Start()
    {
        Waypoints = GetComponentsInChildren<Waypoints>();

       // foreach (HingeJoint joint in Waypoints)
         //   joint.useSpring = false;
    }
}