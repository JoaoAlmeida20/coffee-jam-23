using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private AudioManager audioManager;
    public float maxLifetime;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseSystem.isPaused) return;

        
    }

    void FixedUpdate() {
        maxLifetime -= Time.fixedDeltaTime;
        if (maxLifetime <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
            return;
        if (layer == LayerMask.NameToLayer("Button")) {
            //var otherChild = other.transform.GetChild(0).gameObject;
            //otherChild.SetActive(!otherChild.activeSelf);
        }
        if (layer == LayerMask.NameToLayer("Mirror")){
            audioManager.Play("Mirror");
            //GetComponent<Rigidbody2D>().velocity = Vector3.Reflect(GetComponent<Rigidbody2D>().velocity, other.contacts[0].normal);
            return;
        }
        GameObject.Destroy(this.gameObject);
    }
    void OnTriggerEnter2D(Collider2D other) {
        
    }
}
