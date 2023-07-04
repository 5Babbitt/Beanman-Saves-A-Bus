using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroController : Singleton<HeroController>
{
    private HeroInput input;

    public bool CanMove;
    public bool CanDash;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform visuals;
    [SerializeField] private AudioClip[] laserSounds;
    [SerializeField] private AudioClip[] dashSounds;
    
    [Header("Input Settings")]
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 mouseInput;
    [SerializeField] private Vector2 mouseWorldPos;
    [SerializeField] private bool wantsToDash;
    [SerializeField] private bool isShooting;
    [field: SerializeField] public bool isPressingUp { get; private set; }

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;
    [Range(0f, 10f), SerializeField] private float acceleration;
    [Range(0f, 10f), SerializeField] private float deceleration;
    [SerializeField] private AnimationCurve accelerationFactorFromDot; 

    [Header("Dash Settings")]
    [SerializeField] private float dashSpeed;
    [field: SerializeField] public float dashCooldown { get; private set; }
    [SerializeField] private Transform dashTransform;
    [SerializeField] private ParticleSystem dashEffect;

    [Header("Shooting Settings")]
    [SerializeField] private float shootCooldown;
    [SerializeField] private float fireOffset;
    [SerializeField] private float fireForce;
    [SerializeField] private float aimAngle;
    [SerializeField] private Vector2 aimDir;
    [SerializeField] private Transform fireOrigin;
    [SerializeField] private GameObject laserPrefab;

    [Header("Timer Settings")]
    [SerializeField] private float shootTimer;
    [field: SerializeField] public float dashTimer { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        
        rb = GetComponent<Rigidbody2D>();
        
        input = new HeroInput();

        input.Player.Move.started += OnMovementInput;
        input.Player.Move.performed += OnMovementInput;
        input.Player.Move.canceled += OnMovementInput;

        input.Player.Dash.started += OnDashInput;
        input.Player.Dash.canceled += OnDashInput;

        input.Player.Look.started += OnMouseInput;
        input.Player.Look.performed += OnMouseInput;
        input.Player.Look.canceled += OnMouseInput;

        input.Player.Fire.started += OnFireInput;
        input.Player.Fire.performed += OnFireInput;
        input.Player.Fire.canceled += OnFireInput;
    }

    void OnEnable()
    {
        input.Player.Enable();
    }

    void OnDisable()
    {
        input.Player.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.isPaused)
            return;
        
        Timers();

        Aim();
        Shoot();

        FlipFaceDirection();
    }

    void FixedUpdate()
    {
        if (GameManager.isPaused)
            return;
        
        if (CanMove) ApplyMovoement();
        if (CanDash) ApplyDash();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Debris")
            other.gameObject.GetComponent<Debris>().TakeDamage(1f);
    }

    void Timers()
    {
        dashTimer += Time.deltaTime;
        shootTimer += Time.deltaTime;
    }

    void ApplyMovoement()
    {
        Vector2 _targetSpeed = moveInput * moveSpeed;
        Vector2 _speedDif = _targetSpeed - rb.velocity;

        float _accelRate = (Mathf.Abs(_targetSpeed.magnitude) > 0.01f) ? acceleration : deceleration; // acceleration rate if pressing movement input

        float _velDot = Vector3.Dot(moveInput, _targetSpeed); // Dot product between current and target velocities
        _accelRate *= accelerationFactorFromDot.Evaluate(_velDot); // Evaluate take off speed based on dot product. same direction = 1, opposite direction = 2

        Vector2 _moveForce = rb.mass *_speedDif * _accelRate; // Force = mass x acceleration

        rb.AddForce(_moveForce);
    }

    void ApplyDash()
    {
        if (wantsToDash && dashTimer >= dashCooldown)
        {
            dashTimer = 0f;
            rb.velocity = Vector2.zero;

            Vector2 _dashDir = moveInput.normalized;
            Vector2 _dashForce = rb.mass * (_dashDir * dashSpeed);

            rb.AddForce(_dashForce, ForceMode2D.Impulse);

            EventManager.Instance.OnHeroDash?.Invoke();
            AudioManager.PlayRandomSoundEffect(dashSounds);
            var dashFX = Instantiate(dashEffect, dashTransform.position, Quaternion.LookRotation(Vector3.forward, moveInput.normalized));

            wantsToDash = false;
        }
        else if (wantsToDash && dashTimer < dashCooldown)
        {
            wantsToDash = false;
        }
    }

    void Aim()
    {
        Vector2 firePos = new Vector2(fireOrigin.position.x, fireOrigin.position.y);
        
        mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseInput);
        aimDir = (mouseWorldPos - firePos).normalized;
        aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

        fireOrigin.rotation = Quaternion.Euler(fireOrigin.rotation.x, fireOrigin.rotation.y, aimAngle);

        Debug.DrawLine(firePos, mouseWorldPos, Color.green);
    }

    void Shoot()
    {
        if (isShooting && shootTimer >= shootCooldown)
        {
            shootTimer = 0f;

            Vector3 offset = (aimDir * fireOffset);

            GameObject laser = Instantiate(laserPrefab, fireOrigin.position, fireOrigin.rotation);
            laser.GetComponent<Rigidbody2D>().AddForce(fireOrigin.up * fireForce, ForceMode2D.Impulse);

            EventManager.Instance.OnHeroShoot?.Invoke();
            AudioManager.PlayRandomSoundEffect(laserSounds);
            
            Debug.Log("shot fired");
        }
    }

    void FlipFaceDirection()
    {
        bool isFacingRight = (mouseWorldPos.x > fireOrigin.position.x);

        Vector3 rightDirection = new Vector3(-1, 1, 1);
        Vector3 leftDirection = new Vector3(1, 1, 1);

        visuals.localScale = (isFacingRight) ? rightDirection : leftDirection;
    }

    void OnMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        
        isPressingUp = moveInput.y >= 0.7f;
    }
    
    private void OnMouseInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started) wantsToDash = true;
    }

    private void OnFireInput(InputAction.CallbackContext context)
    {
        isShooting = context.ReadValueAsButton();
    }
}
