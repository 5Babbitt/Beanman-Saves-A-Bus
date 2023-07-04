using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisSpawner : Singleton<DebrisSpawner>
{
    [SerializeField] private GameSettings settings;
    [SerializeField] private Transform bus;
    [SerializeField] private GameObject[] debrisArray; 

    [Header("Spawn Settings")]
    [SerializeField] private float spawnRadius = 12f;
    [SerializeField] private float maxSpawnForce = 1f;
    [SerializeField] private float minSpawnForce = 5f;

    [Header("Spawn Time Settings")]
    [SerializeField] private float defaultSpawnRate;
    [SerializeField] private float startSpawnRate;
    [SerializeField] private float minSpawnRate;
    [SerializeField] private float decreaseRate;

    private float spawnRate;

    [Header("Debris Settings")]
    [SerializeField] private ParticleSystem explosionEffect;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void OnEnable() 
    {
        EventManager.Instance.OnDebrisDestroyed += OnDebrisDestroyed;
    }

    private void OnDisable() 
    {
        EventManager.Instance.OnDebrisDestroyed -= OnDebrisDestroyed;
    }

    void Start()
    {
        SetValues();
        
        bus = Bus.Instance.transform;

        startSpawnRate = defaultSpawnRate;
        spawnRate = defaultSpawnRate;
    }

    void Update()
    {
        if (GameManager.isPaused)
            return;
        
        spawnRate -= Time.deltaTime;

        if (spawnRate <= 0)
        {
            Spawn();

            if (startSpawnRate > minSpawnRate)
            {
                startSpawnRate -= decreaseRate;
            }

            spawnRate = startSpawnRate;
        }
    }
    
    void Spawn()
    {
        GameObject debris = Instantiate(debrisArray[Random.Range(0, debrisArray.Length)], RandomPointOnCircleEdge(spawnRadius), Quaternion.identity);

        Vector2 busDir = (bus.position - debris.transform.position).normalized;

        debris.GetComponent<Rigidbody2D>().AddForce(busDir * Random.Range(minSpawnForce, maxSpawnForce), ForceMode2D.Impulse);
        debris.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-1f, 1f), ForceMode2D.Impulse);

        Debug.Log("Debris Spawned");
    }

    private void OnDebrisDestroyed(Vector3 pos)
    {
        var explosion = Instantiate(explosionEffect, pos, Quaternion.identity);
    }

    private Vector2 RandomPointOnCircleEdge(float radius)
    {
        var circlePoint = Random.insideUnitCircle.normalized * radius;
        return new Vector2(circlePoint.x, circlePoint.y);
    }

    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(bus.position, spawnRadius);
    }

    private void OnValidate() 
    {
        SetValues();
    }

    private void SetValues()
    {
        spawnRadius = settings.spawnRadius;
        maxSpawnForce = settings.maxSpawnForce;
        minSpawnForce = settings.minSpawnForce;
        defaultSpawnRate = settings.defaultSpawnRate;
        minSpawnRate = settings.minSpawnRate;
        decreaseRate = settings.decreaseRate;
    }
}
