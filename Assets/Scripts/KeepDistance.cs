using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepDistance : MonoBehaviour
{
    public Transform target; // The object to keep the distance from
    private Vector3 distance; // The desired distance to maintain
    
    void Start()
    {
        // Calculate the initial distance between the two objects
        distance = target.position - transform.position;
    }
    
    void Update()
    {
        // Move the object to the desired position
        transform.position = target.position - distance;
    }
}
