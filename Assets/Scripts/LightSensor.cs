using Audio;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightSensor : MonoBehaviour
{
    [Header("General")]
    public bool isPermanent;
    public bool isPositive;

    [Header("Final Check")]
    public bool shouldOpendoor;
    public FinalDoor door;

    private AudioManager audioManager;
    private Light2D light2d;

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
     private bool hasMoved;

    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        light2d = GetComponent<Light2D>();
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
            if (toMove.transform.position == OriginalPos && !isPermanent)
                hasMoved = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Positive Light") && !isPositive) return;
        if (other.CompareTag("Negative Light") && isPositive) return;
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null && (other.CompareTag("Positive Light") || other.CompareTag("Negative Light")))
        {
            light2d.enabled = true;
            audioManager.Play("LitUP");
            if (shouldSpawnObj)
                spawnedObj = Instantiate(toSpawn, spawnPos, Quaternion.identity);
            if (shouldDespawnObj)
                toDespawn.SetActive(false);
            if (shouldMoveObj){
                hasMoved = true;
                 moving = true;
            }
            if (shouldOpendoor)
                door.conditionTrue();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Positive Light") && !isPositive) return;
        if (other.CompareTag("Negative Light") && isPositive) return;
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null && (other.CompareTag("Positive Light") || other.CompareTag("Negative Light")))
        {
            light2d.enabled = false;
            if (shouldSpawnObj && !isPermanent)
                Destroy(spawnedObj);
            if (shouldDespawnObj && !isPermanent)
                toDespawn.SetActive(true);
            if (shouldMoveObj && !isPermanent)
                moving = false;
            if (shouldOpendoor && !isPermanent)
                door.conditionFalse();
        }
    }
}
