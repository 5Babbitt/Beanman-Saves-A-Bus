using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class GameSettings : ScriptableObject
{
    [Header("Game Settings")]
    public float terminalVelocity = 35f;
    
    [Header("Bus Settings")]
    public float maxHealth;
    public float hotSpeed = 10f;
    public float defaultTemperature = 30f;
    public float maxTemperature = 100f;
    public float heatingRate = 1f;
    public float coolingRate = 2f;
    public AnimationCurve heatCurve;

    [Header("Debris Settings")]
    public float spawnRadius = 12f;
    public float maxSpawnForce = 1f;
    public float minSpawnForce = 5f;
    [Space(10)]
    public float defaultSpawnRate;
    public float minSpawnRate;
    public float decreaseRate;
}
