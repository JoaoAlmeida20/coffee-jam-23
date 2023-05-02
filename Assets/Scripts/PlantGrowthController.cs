using UnityEngine;

public class PlantGrowthController : LightSensor
{
    [Header("Growth Properties")]
    public float growthRate = 0.5f;
    public float maxGrowthNumber = 5f;
    public float minGrowthNumber = 0f;
    
    private Vector3 maxGrowthVector;
    private Vector3 minGrowthVector;
    
    private void Start()
    {
        //Check the direction to grow
        if (transform.rotation.eulerAngles.z == 0f) //grow up
        {
            maxGrowthVector = new Vector3(transform.position.x, transform.position.y + maxGrowthNumber, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x, transform.position.y - minGrowthNumber, transform.position.z);
        }
        else if (transform.rotation.eulerAngles.z == 90f) //grow to the left
        {
            maxGrowthVector = new Vector3(transform.position.x - maxGrowthNumber, transform.position.y, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x + minGrowthNumber, transform.position.y, transform.position.z);
        }
        else if (transform.rotation.eulerAngles.z == 270f) //grow to the right
        {
            maxGrowthVector = new Vector3(transform.position.x + maxGrowthNumber, transform.position.y, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x - minGrowthNumber, transform.position.y, transform.position.z);
        }
        else if (transform.rotation.eulerAngles.z == 180f) //grow down
        {
            maxGrowthVector = new Vector3(transform.position.x, transform.position.y - maxGrowthNumber, transform.position.z);
            minGrowthVector = new Vector3(transform.position.x, transform.position.y + minGrowthNumber, transform.position.z);
        }
    }

    private void FixedUpdate()
    {
        if (isPositiveLit) transform.position = Vector3.MoveTowards(transform.position, maxGrowthVector, growthRate * Time.deltaTime);
        if (isNegativeLit) transform.position = Vector3.MoveTowards(transform.position, minGrowthVector, growthRate * Time.deltaTime);
    }
}
