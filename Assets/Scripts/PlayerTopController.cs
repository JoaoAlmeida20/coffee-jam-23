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
    [SerializeField] public bool tryRelinking;

    Rigidbody2D rigidbody2d;
    GameObject playerBottom;
    FixedJoint2D fixedJoint2D;
    Vector3 defaultPosition;

    [Header("FlashLight")]
    public float intensity;
    public GameObject flashLight;
    bool lightState;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = 0.0f;
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerBottom = transform.parent.gameObject;
        fixedJoint2D = playerBottom.GetComponent<FixedJoint2D>();
        defaultPosition = transform.localPosition;
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

        Debug.Log(Vector3.Distance(transform.localPosition, defaultPosition));
        Debug.Log(tryRelinking);
        if (!fixedJoint2D.enabled && tryRelinking) {
            rigidbody2d.velocity = 12.0f * ((playerBottom.transform.position + defaultPosition) - transform.position);
            if (Vector3.Distance(transform.localPosition, defaultPosition) < 0.3f)
            {
                transform.localPosition = defaultPosition;
                fixedJoint2D.enabled = true;
                tryRelinking = false;
            }
        }
    }
}
