using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bus : Singleton<Bus>, IDamageable
{
    

    [Header("Base Settings")]
    [SerializeField] private GameSettings settings;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float maxHealth;
    
    [Header("Current Bus Stats")]
    [SerializeField] private bool heroUnderBus;
    [field: SerializeField] public float busTemperature { get; private set; }
    [field: SerializeField] public float busHealth { get; private set; }
    [field: SerializeField] public float currentUpForce { get; private set; }
    [field: SerializeField] public float currentSpeed { get; private set; }
    [field: SerializeField] public float currentHeight { get; private set; }

    [Header("Temperature Settings")]
    [SerializeField] private float hotSpeed = 10f;
    [SerializeField] private float defaultTemperature = 0f;
    [SerializeField] private float maxTemperature = 100f;
    [SerializeField] private float heatingRate = 1f;
    [SerializeField] private float coolingRate = 2f;
    [SerializeField] private AnimationCurve heatCurve;

    [Header("Force Settings")]
    [SerializeField] private float upForce = 100f;
    [field: SerializeField] public bool isGoingUp { get; private set; }
    private float forceAssist; // assists the force to get to slowed velocity

    [Header("Fall Settings")]
    [SerializeField] private float carryVelocity = 2.5f;
    [field:SerializeField] public float fallHeight { get; private set; }
    [field:SerializeField] public float terminalVelocity { get; private set; } = 35f;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        rb = BackgroundHolder.Instance.RB;
        
        SetValues();
        
        busHealth = maxHealth;
        busTemperature = defaultTemperature;
        
        fallHeight = BackgroundHolder.Instance.DISTANCE;

        currentHeight = fallHeight;
        
        EventManager.Instance.BusHealth?.Invoke(busHealth);
    }

    void Update()
    {
        currentSpeed = rb.velocity.y;
        currentHeight = Vector3.Distance(transform.position, BackgroundHolder.Instance.END.position);
        
        forceAssist = Mathf.Clamp(currentSpeed, 0f, 35f);
        isGoingUp = (heroUnderBus && HeroController.Instance.isPressingUp);
        
        float assistedUpForce = upForce * forceAssist;
        
        currentUpForce = isGoingUp ? assistedUpForce : 0f;

        if (currentSpeed >= hotSpeed)
        {
            float speedTempMultiplier = heatCurve.Evaluate((currentSpeed));
            float currentHeatRate = heatingRate * speedTempMultiplier;
            
            busTemperature += currentHeatRate * Time.deltaTime;
        }
        else
        {
            float speedTempMultiplier = 1 - (currentSpeed) / (hotSpeed);
            float currentCoolingRate = coolingRate * speedTempMultiplier;
            
            busTemperature -= currentCoolingRate * Time.deltaTime;
        }

        busTemperature = Mathf.Clamp(busTemperature, defaultTemperature, 1000f);

        if (busTemperature >= maxTemperature)
        {
            Explode();
        }

        EventManager.Instance.BusSpeed?.Invoke(currentSpeed);
        EventManager.Instance.BusTemperature?.Invoke(busTemperature);
        EventManager.Instance.BusHeight?.Invoke(currentHeight);
    }

    private void FixedUpdate() 
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, terminalVelocity);

        if (isGoingUp)
            PhysicsHelper.ApplyForceToReachVelocity2D(rb, Vector2.down * carryVelocity, currentUpForce);
    }

    private void Explode()
    {
        EventManager.OnGameOver?.Invoke();
    }

    public void TakeDamage(float damage)
    {
        busHealth -= damage;
        Debug.Log($"Bus Took {damage} Damage");

        EventManager.Instance.OnBusTakeDamage?.Invoke();
        EventManager.Instance.BusHealth?.Invoke(busHealth);

        if (busHealth <= 0)
        {
            Explode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.tag == "Hero")
            heroUnderBus = true;
            
        if (other.gameObject.tag == "Finish Line")
            EventManager.OnGameWon?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (!(other.gameObject.tag == "Hero"))
            return;
        
        heroUnderBus = false;
    }

    private void OnValidate() 
    {
        SetValues();
    }

    private void SetValues()
    {
        maxHealth = settings.maxHealth;
        hotSpeed = settings.hotSpeed;
        defaultTemperature = settings.defaultTemperature;
        maxTemperature = settings.maxTemperature;
        heatingRate = settings.heatingRate;
        coolingRate = settings.coolingRate;
        heatCurve = settings.heatCurve;
        terminalVelocity = settings.terminalVelocity;
    }
}
