using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSensor : MonoBehaviour
{
    [Header("General")]
    public bool isPermanent;

    [Header("Spawn/Despawn Object")]
    public bool shouldSpawnObj = false;
    public GameObject toSpawn;
    public Vector3 spawnPos;
    private GameObject spawnedObj;

    [Header("Despawn Object")]
    public bool shouldDespawnObj = false;
    public GameObject toDespawn;

    // [Header("Move Object")]
    // public bool shouldMoveObj = false;
    // public GameObject toMove;
    // public Vector3 movePos;
    // private Vector3 OriginalPos;
    // public float speed = .05f;
    // private bool moving;

    void Start(){
        //OriginalPos = toMove.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate(){
        // if (moving)
        //     toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, movePos, speed);
        // if (!moving)
        //     toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, OriginalPos, speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            if (shouldSpawnObj)
                spawnedObj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
            if (shouldDespawnObj)
                toDespawn.SetActive(false);
            // if (shouldMoveObj){
            //     OriginalPos = toMove.transform.position;
            //     moving = true;
            //}
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            if (shouldSpawnObj && !isPermanent)
                Destroy(spawnedObj);
            if (shouldDespawnObj && !isPermanent)
                toDespawn.SetActive(true);
            // if (shouldMoveObj)
            //     moving = false;
        }
    }
}
