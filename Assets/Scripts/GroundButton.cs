using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class GroundButton : MonoBehaviour
{
    [Header("General")]
    public bool isPermanent;
    private AudioManager audioManager;
    private Transform platePress;

    [Header("Final Check")]
    public bool shouldOpendoor;
    public FinalDoor door;

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
     private int triggerCount;
     private bool hasMoved;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        platePress = transform.Find("pressure_plate_press");
        if(toMove != null)
            OriginalPos = toMove.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (toMove != null)
        {
            if (moving)
                toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, movePos, speed);
            if (!moving && hasMoved)
                toMove.transform.position = Vector3.MoveTowards(toMove.transform.position, OriginalPos, speed);
            if (toMove.transform.position == OriginalPos)
                hasMoved = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player") && triggerCount == 0)
        {
            platePress.localPosition = platePress.localPosition + Vector3.down * 0.05f;
            audioManager.Play("ButtonClick");
            if (shouldSpawnObj){
                spawnedObj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
            }
            if (shouldDespawnObj){
                toDespawn.SetActive(false);
            }
            if (shouldMoveObj){
                hasMoved = true;
                moving = true;
            }
            if (shouldOpendoor){
                door.conditionTrue();
            }
        }
        triggerCount++;
    }

    void OnTriggerExit2D(Collider2D other) {
        triggerCount--;
        if (triggerCount == 0 && !isPermanent) {
            int layer = other.gameObject.layer;
            if (layer == LayerMask.NameToLayer("Player"))
            {
                platePress.localPosition = platePress.localPosition - Vector3.down * 0.05f;
                if (shouldSpawnObj){
                    Destroy(spawnedObj);
                }
                if (shouldDespawnObj){
                    toDespawn.SetActive(true);
                }
                if (shouldMoveObj){        
                    moving = false;
                }
                if (shouldOpendoor){
                    door.conditionFalse();
                }
            }
        }
    }
}
