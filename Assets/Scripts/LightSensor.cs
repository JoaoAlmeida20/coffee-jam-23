using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSensor : MonoBehaviour
{
    //bool isLit = false; // A boolean to store whether this object is being illuminated by a Light2D component
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
            //isLit = true;
            obj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            Destroy(obj);
            //isLit = false;
        }
    }
}
