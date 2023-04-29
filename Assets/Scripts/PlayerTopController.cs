using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTopController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float cooldownTime;

    bool fire;
    float cooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")) {
            fire = true;
        }
    }

    void FixedUpdate() {
        if (cooldownTimer > 0.0f) {
            cooldownTimer -= Time.fixedDeltaTime;
        }
        if (fire && cooldownTimer <= 0.0f) {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            Vector2 direction = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            projectile.GetComponent<Rigidbody2D>().AddForce(direction.normalized * projectileSpeed);
            fire = false;
            cooldownTimer = cooldownTime;
        }
    }
}
