using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class Button : MonoBehaviour
{
    [Header("General")]
    public bool isPermanent;
    public float duration;
    private AudioManager audioManager;

    [Header("Spawn/Despawn Object")]
    public bool shouldSpawnObj = false;
    public GameObject toSpawn;
    public Vector3 spawnPos;
    private GameObject spawnedObj;

    [Header("Despawn Object")]
    public bool shouldDespawnObj = false;
    public GameObject toDespawn;

     [Header("Move Object")]
     public bool shouldMoveObj = false;
     public GameObject toMove;
     public Vector3 movePos;
     private Vector3 OriginalPos;
     public float speed = .05f;
     private bool moving;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        if(toMove != null)
            OriginalPos = toMove.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (toMove != null)
        {
            if (moving)
                toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, movePos, speed);
            if (!moving)
                toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, OriginalPos, speed);
        }
    }

    IEnumerator spawn(float duration) {
        yield return new WaitForSeconds(duration);
        Destroy(spawnedObj);
    }

    IEnumerator despawn(float duration) {
        yield return new WaitForSeconds(duration);
        toDespawn.SetActive(true);
    }

    IEnumerator move(float duration) {
        yield return new WaitForSeconds(duration);
        moving = false;
    }

    void OnCollisionEnter2D(Collision2D other){
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Bullet"))
        {
            audioManager.Play("ButtonClick");
            if (shouldSpawnObj){
                spawnedObj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
                if (!isPermanent)
                    StartCoroutine(spawn(duration));
            }
            if (shouldDespawnObj){
                toDespawn.SetActive(false);
                if (!isPermanent)
                    StartCoroutine(despawn(duration));
            }
            if (shouldMoveObj){
                OriginalPos = toMove.transform.position;
                moving = true;
                if (!isPermanent)
                    StartCoroutine(move(duration));
            }
        }
    }
}
