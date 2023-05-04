using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerTopController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float cooldownTime;

    bool fire;
    float cooldownTimer;
    [HideInInspector] public bool tryRelinking;
    float heightLink;
    float relinkCooldownTimer;

    Rigidbody2D rigidbody2d;
    GameObject playerBottom;
    FixedJoint2D fixedJoint2D;
    Rigidbody2D bottomRigidbody;
    SpriteRenderer spriteRenderer;
    Sprite defaultSprite;
    Vector3 defaultPosition;

    private AudioManager audioManager;
    
    [Header("FlashLight")]
    public float intensity;
    public GameObject flashLight;
    bool lightState;

    [Header("Linking")]
    public float topSearchRadius;
    public float maxHeightLink;
    public float relinkCooldownTime;

    [Header("Sprite")]
    public Sprite movingSprite;

    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = 0.0f;
        relinkCooldownTimer = 0.0f;
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerBottom = transform.parent.gameObject;
        fixedJoint2D = playerBottom.GetComponent<FixedJoint2D>();
        bottomRigidbody = playerBottom.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
        defaultPosition = transform.localPosition;
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseSystem.isPaused) return;

        if (Input.GetButtonDown("Fire1")) {
            fire = true;
            audioManager.Play("Shoot");
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse2)) {
            audioManager.Play("Flashlight");
            lightState = !lightState;
            if (lightState)
            {
                audioManager.Play("LightHum");
                flashLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = intensity;
                flashLight.SetActive(true);
            }
            else
            {
                audioManager.Stop("LightHum");
                flashLight.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = 0;
                flashLight.SetActive(false);
            }
        }
        if (Input.GetButtonDown("Fire2")) {
            if (fixedJoint2D.enabled) {
                audioManager.Play("Joint");
                fixedJoint2D.enabled = false;
            }
            else if (relinkCooldownTimer <= 0.0f) {
                print("hello");
                var colliders = new List<Collider2D>();
                Physics2D.OverlapCircle(transform.position, topSearchRadius, new ContactFilter2D().NoFilter(), colliders);
                foreach (var c in colliders) {
                    if (c.TryGetComponent(out PlayerBottomController playerBottomController)) {
                        tryRelinking = true;
                        heightLink = rigidbody2d.position.y;
                        if (bottomRigidbody.velocity.y > 0.0f) {
                            bottomRigidbody.velocity *= new Vector2(1.0f, 0.5f);
                        }
                    }
                }
            }
        }
    }

    void FixedUpdate() {
        if (cooldownTimer > 0.0f) {
            cooldownTimer -= Time.fixedDeltaTime;
        }
        if (relinkCooldownTimer > 0.0f) {
            relinkCooldownTimer -= Time.fixedDeltaTime;
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

        //if (!fixedJoint2D.enabled) rigidbody2d.gravityScale = 0;      //Code to leave it floating

        if (!fixedJoint2D.enabled && tryRelinking) {
            var colliders = new List<Collider2D>();
            Physics2D.OverlapCircle(transform.position, topSearchRadius, new ContactFilter2D().NoFilter(), colliders);
            bool inRange = false;
            foreach (var c in colliders) {
                if (c.TryGetComponent(out PlayerBottomController playerBottomController)) {
                    inRange = true;
                    break;
                }
            }
            if (!inRange || maxHeightLink < rigidbody2d.position.y - heightLink) {
                tryRelinking = false;
                rigidbody2d.velocity *= new Vector2(0.2f, 0.1f);
                relinkCooldownTimer = relinkCooldownTime;
            }
            if (tryRelinking) {
                rigidbody2d.velocity = 12.0f * ((playerBottom.transform.position + defaultPosition) - transform.position);
                if (Vector3.Distance(transform.localPosition, defaultPosition) < 0.3f)
                {
                    audioManager.Play("ReJoint");
                    transform.localPosition = defaultPosition;
                    fixedJoint2D.enabled = true;
                    tryRelinking = false;
                    //rigidbody2d.gravityScale = 1;                         //Code to leave it floating
                }
            }
        }

        // Change Sprite if moving
        if (Mathf.Abs(rigidbody2d.velocity.x) > 0.4f) {
            spriteRenderer.sprite = movingSprite;
            spriteRenderer.flipX = Mathf.Sign(rigidbody2d.velocity.x) == -1;
        }
        else {
            spriteRenderer.sprite = defaultSprite;
            spriteRenderer.flipX = false;
        }
    }
}
