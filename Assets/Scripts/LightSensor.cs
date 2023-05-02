using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSensor : MonoBehaviour
{
    public bool isPositiveLit = false; // A boolean to store whether this object is being illuminated by a Positive Light2D component
    public bool isNegativeLit = false; // A boolean to store whether this object is being illuminated by a Negative Light2D component
    
    public GameObject toSpawn;
    public Vector3 spawnPos;
    private GameObject obj;

    // Update is called once per frame
    void Update(){}

    private void OnTriggerEnter2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            if (other.CompareTag("Positive Light")) isPositiveLit = true;
            if (other.CompareTag("Negative Light")) isNegativeLit = true;
            
            if(toSpawn != null)
                obj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            if(toSpawn != null)
                Destroy(obj);
            
            if (other.CompareTag("Positive Light")) isPositiveLit = false;
            if (other.CompareTag("Negative Light")) isNegativeLit = false;
        }
    }
}
