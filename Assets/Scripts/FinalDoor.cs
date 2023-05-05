using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public int ConditionCount;
    private int conditionsTrue = 0;
    public string nextLvl;
    private bool isOpen;
    public GameObject extraDoor;
    private bool moving;
    private bool hasMoved;
    private Vector3 OriginalPos;
    public Vector3 movePos;
    public float speed = .05f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(extraDoor != null)
            OriginalPos = extraDoor.transform.position;
    }

    public void conditionTrue(){
        conditionsTrue++;
        print(conditionsTrue);
    }
    public void conditionFalse(){
        conditionsTrue--;
        print(conditionsTrue);
    }
    
    void Update()
    {
        if (conditionsTrue == ConditionCount)
        {
            Open();
        }
    }

    void FixedUpdate(){
        if (extraDoor != null)
        {
            if (moving)
                extraDoor.transform.position = Vector3.MoveTowards(extraDoor.transform.position, movePos, speed);
            if (!moving && hasMoved)
                extraDoor.transform.position = Vector3.MoveTowards(extraDoor.transform.position, OriginalPos, speed);
            if (extraDoor.transform.position == OriginalPos)
                hasMoved = false;
        }
    }

    public void Open(){
        isOpen = true;
        spriteRenderer.sprite = openSprite;
        if (extraDoor != null)
        {
            hasMoved = true;
            moving = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && isOpen)
        {
            if (other.gameObject.GetComponent<FixedJoint2D>() != null && other.gameObject.GetComponent<FixedJoint2D>().enabled)
            {
                StartCoroutine(FinishLevel());
            }
        }
    }

    IEnumerator FinishLevel() {
        GameObject.Find("LoadingScreen").GetComponent<LoadingScreen>().close = true;
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);

        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene(nextLvl);
    }
}
