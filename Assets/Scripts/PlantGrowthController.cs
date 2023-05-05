using Audio;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlantGrowthController : MonoBehaviour
{
    [Header("General")]
    public bool isPositiveLit = false; // A boolean to store whether this object is being illuminated by a Positive Light2D component
    public bool isNegativeLit = false; // A boolean to store whether this object is being illuminated by a Negative Light2D component
    
    [Header("Growth Properties")]
    public float growthRate = 0.5f;
    public float maxGrowthNumber = 5f;
    public float minGrowthNumber = 0.2f;
    public bool goUp = false;
    public bool goDown = false;
    public bool goRight = false;
    public bool goLeft = false;
    
    private Vector3 maxGrowthVector;
    private Vector3 minGrowthVector;

    private AudioManager audioManager;
    
    private void Start()
    {
        audioManager = GetComponent<AudioManager>();
        if (goUp) //grow up
        {
            maxGrowthVector = new Vector3(transform.position.x, transform.position.y + maxGrowthNumber, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x, transform.position.y - minGrowthNumber, transform.position.z);
        }
        else if (goDown) //grow down
        {
            maxGrowthVector = new Vector3(transform.position.x, transform.position.y - maxGrowthNumber, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x, transform.position.y + minGrowthNumber, transform.position.z);
        }
        else if (goRight) //grow right
        {
            maxGrowthVector = new Vector3(transform.position.x + maxGrowthNumber, transform.position.y, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x - minGrowthNumber, transform.position.y, transform.position.z);
        }
        else if (goLeft) //grow left
        {
            maxGrowthVector = new Vector3(transform.position.x - maxGrowthNumber, transform.position.y, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x + minGrowthNumber, transform.position.y, transform.position.z);
        }
    }
    
    private void FixedUpdate()
    {
        if (isPositiveLit) transform.position = Vector3.MoveTowards(transform.position, maxGrowthVector, growthRate * Time.deltaTime);
        if (isNegativeLit) transform.position = Vector3.MoveTowards(transform.position, minGrowthVector, growthRate * Time.deltaTime);
        
        if(maxGrowthVector == transform.position) audioManager.Stop("Plant");
        if(minGrowthVector == transform.position) audioManager.Stop("Plant");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            audioManager.Play("Plant");
            if (other.CompareTag("Positive Light")) isPositiveLit = true;
            if (other.CompareTag("Negative Light")) isNegativeLit = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Light2D light2D = other.GetComponent<Light2D>();
        if (light2D != null)
        {
            audioManager.Stop("Plant");
            if (other.CompareTag("Positive Light")) isPositiveLit = false;
            if (other.CompareTag("Negative Light")) isNegativeLit = false;
        }
    }
}
