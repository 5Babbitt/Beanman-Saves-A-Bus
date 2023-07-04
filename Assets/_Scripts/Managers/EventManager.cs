using System;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    // Bus Events
    public Action OnBusTakeDamage;
    public Action<float> BusTemperature;
    public Action<float> BusHealth;
    public Action<float> BusSpeed;
    public Action<float> BusHeight;

    // Debris Events
    public Action<Vector3> OnDebrisDestroyed;
    public Action OnDebrisSpawned;

    // Hero Events
    public Action OnHeroShoot;
    public Action OnHeroDash;

    // Game Events
    public static Action OnGameStart;
    public static Action OnGameWon;
    public static Action OnGameOver;
    public static Action OnGamePaused;
}
