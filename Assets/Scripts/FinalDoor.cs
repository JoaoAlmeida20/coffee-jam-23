using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoor : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite openSprite;
    public int ConditionCount;
    private int conditionsTrue;
    public Scene nextLvl;
    private bool isOpen;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void conditionTrue(){
        ConditionCount++;
    }
    public void conditionFalse(){
        ConditionCount--;
    }
    
    void Update()
    {
        if (conditionsTrue == ConditionCount)
        {
            Open();
        }
    }

    public void Open(){
        isOpen = true;
        spriteRenderer.sprite = openSprite;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") && isOpen)
        {
            if (other.gameObject.GetComponent<FixedJoint2D>().enabled)
            {
                SceneManager.LoadScene(nextLvl.buildIndex);
            }
        }
    }
}
