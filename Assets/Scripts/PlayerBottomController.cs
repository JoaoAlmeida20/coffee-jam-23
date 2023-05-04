using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBottomController : MonoBehaviour
{

    [Header("Run")]
    public float runMaxSpeed;
    public float speedPower;
    public float accel;
    public float deccel;
    [Range(0, 1)] public float accelAirMultiplier;
    [Range(0, 1)] public float deccelAirMultiplier;

    [Header("Jump")]
    public float jumpStrength;
    [Range(0, 1)] public float jumpShorthopMultiplier;
    [Range(0, 0.5f)] public float jumpCoyoteTime;
    [Range(0, 0.5f)] public float jumpBufferTime;

    [Header("Pulling")]
    public float pullStrength;
    public float pullDistance = 5f;
    bool isPulling;
    string movableTag = "Movable";
    GameObject[] movableObjects;
    
    [Header("Physics")]
    public float friction;
    public float fallGravityMultiplier;
    public float maxFallSpeed;
    public float rotationSpeed;

    [Header("Sprite")]
    public Sprite movingSprite;

    Rigidbody2D rigidbody2d;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D capsuleCollider2D;
    FixedJoint2D fixedJoint2D;
    Sprite defaultSprite;

    Quaternion defaultRotation;
    Vector2 playerSize;
    Vector2 groundCheckSize;
    Vector3 defaultTopPosition;
    float gravityScale;

    private AudioManager audioManager;
    
    [Header("FlashLight")]
    public float intensity;
    public GameObject flashLight;
    bool lightState;

    [HideInInspector] public float horizontal;
    [HideInInspector] public float vertical;
    bool isJumping;
    bool jumpReleased;
    float lastJumpedTime;
    float lastGroundedTime;
    bool fire1;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        fixedJoint2D = GetComponent<FixedJoint2D>();
        defaultTopPosition = transform.GetChild(0).GetComponent<Transform>().localPosition;
        defaultSprite = spriteRenderer.sprite;
        defaultRotation = transform.rotation;
        playerSize = capsuleCollider2D.size;
        gravityScale = rigidbody2d.gravityScale;
        groundCheckSize = new Vector2(playerSize.x * transform.localScale.x - 0.3f, .5f); //mudei o Y por causa da scale
        isJumping = false;
        lastJumpedTime = jumpBufferTime + 1.0f;
        lastGroundedTime = jumpCoyoteTime + 1.0f;
        movableObjects = GameObject.FindGameObjectsWithTag(movableTag);
        audioManager = GetComponent<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseSystem.isPaused) return;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Jump")) {
            audioManager.Play("Jump");
            lastJumpedTime = 0.0f;
            jumpReleased = false;
        }
        else {
            lastJumpedTime += Time.deltaTime;
        }
        if (Input.GetButtonUp("Jump")) {
            jumpReleased = true;
        }
        
        //Negative Flashlight
        if (Input.GetKeyDown(KeyCode.E)) {
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

        if (Input.GetKeyDown(KeyCode.R)) {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            audioManager.Play("Pulling");
            isPulling = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) {
            audioManager.Play("NotPulling");
            isPulling = false;
        }
    }

    void FixedUpdate() {
        // Grounded Check
        lastGroundedTime += Time.fixedDeltaTime;
        Vector2 groundCheckCenter = (Vector2) transform.position + (Vector2.down * (transform.localScale.y + playerSize.y) / 3.0f);
        if (Physics2D.OverlapBox(groundCheckCenter, groundCheckSize, 0.0f, LayerMask.GetMask("Default", "Button")) != null) {
            lastGroundedTime = 0.0f;
        }

        // Run
        float targetSpeed = horizontal * runMaxSpeed;
        float speedDiff = targetSpeed - rigidbody2d.velocity.x;
        float accelRate;
        if (lastGroundedTime == 0.0f) {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel : deccel;
        }
        else {
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? accel * accelAirMultiplier : deccel * deccelAirMultiplier;
        }

        // Applies acceleration to speed difference, then raises to set power so acceleration increases with higher speeds
        // Multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, speedPower) * Mathf.Sign(speedDiff);
        if (!(Mathf.Abs(targetSpeed) != 0 && Mathf.Sign(targetSpeed) == Mathf.Sign(rigidbody2d.velocity.x) && Mathf.Abs(targetSpeed) < Mathf.Abs(rigidbody2d.velocity.x))) {
            rigidbody2d.AddForce(movement * Vector2.right);
        }

        // Jump
        if (isJumping && rigidbody2d.velocity.y <= 0.0f) {
            isJumping = false;
        }
        if (!isJumping && lastJumpedTime <= jumpBufferTime) {
            if (lastGroundedTime <= jumpCoyoteTime) {
                rigidbody2d.AddForce(Vector2.up * jumpStrength * (0.9f + Mathf.Min(0.1f, Mathf.Abs(rigidbody2d.velocity.x) / (runMaxSpeed * 10))), ForceMode2D.Impulse);
                isJumping = true;
            }
        }

        if (jumpReleased && isJumping) {
            rigidbody2d.AddForce(Vector2.down * rigidbody2d.velocity * (1 - jumpShorthopMultiplier), ForceMode2D.Impulse);
            jumpReleased = false;
        }

        // Friction
        if (lastGroundedTime == 0.0f && (Mathf.Abs(horizontal) < 0.01f || Mathf.Abs(rigidbody2d.velocity.x) > runMaxSpeed)) {
            float amount = Mathf.Min(Mathf.Abs(rigidbody2d.velocity.x), friction);
            amount *= Mathf.Sign(rigidbody2d.velocity.x);
            rigidbody2d.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        // Increased falling gravity
        if (rigidbody2d.velocity.y < 0.0f) {
            rigidbody2d.gravityScale = gravityScale * fallGravityMultiplier;
        }
        else {
            rigidbody2d.gravityScale = gravityScale;
        }

        // Max fall speed
        if (rigidbody2d.velocity.y < -maxFallSpeed) {
            rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, -maxFallSpeed);
        }

        //Pull
        if (isPulling)
        {
            foreach (GameObject obj in movableObjects)
            {
                float distanceToPlayer = Vector3.Distance(obj.transform.position, transform.position);
                if (distanceToPlayer <= pullDistance)
                {
                    Vector3 directionToPlayer = (transform.position - obj.transform.position).normalized;
                    float pullForce = (pullDistance - distanceToPlayer) / pullDistance * pullStrength;
                    obj.GetComponent<Rigidbody2D>().AddForce(directionToPlayer * pullForce);
                }
            }
        }
        
        Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, -Camera.main.transform.position.z)) - transform.position;

        //Negative Light
        if (lightState){
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            flashLight.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

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
