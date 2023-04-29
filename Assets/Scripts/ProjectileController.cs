using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    public float maxLifetime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        maxLifetime -= Time.fixedDeltaTime;
        if (maxLifetime <= 0) {
            GameObject.Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        int layer = other.gameObject.layer;
        if (layer == LayerMask.NameToLayer("Player"))
            return;
        if (layer == LayerMask.NameToLayer("Button")) {
            var otherChild = other.transform.GetChild(0).gameObject;
            otherChild.SetActive(!otherChild.activeSelf);
        }
        GameObject.Destroy(this.gameObject);
    }
}
