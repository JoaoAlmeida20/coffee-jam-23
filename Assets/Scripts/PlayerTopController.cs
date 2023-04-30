using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerTopController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float cooldownTime;

    bool fire;
    float cooldownTimer;

    [Header("FlashLight")]
    public float intensity;
    public GameObject flashLight;
    bool lightState;

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
        if (Input.GetKeyDown(KeyCode.L)) {
            lightState = !lightState;
            if (lightState)
            {
                flashLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = intensity;
                flashLight.SetActive(true);
            }
            else
            {
                flashLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
                flashLight.SetActive(false);
            }
        }
    }

    void FixedUpdate() {
        if (cooldownTimer > 0.0f) {
            cooldownTimer -= Time.fixedDeltaTime;
        }
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - transform.position;

        if (fire && cooldownTimer <= 0.0f) {
            var projectile = Instantiate(projectilePrefab, transform.position, transform.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(direction.normalized * projectileSpeed);
            fire = false;
            cooldownTimer = cooldownTime;
        }
        if (lightState){
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            flashLight.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        }
    }
}
