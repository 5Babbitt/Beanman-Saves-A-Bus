using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    [SerializeField] private ParticleSystem speedLines;
    [SerializeField] private float speed;
    [SerializeField] private float defaultParticleSpeed = 20f;
    [SerializeField] private float maxParticleSpeed = 100f;

    void OnEnable()
    {
        EventManager.Instance.BusSpeed += SetSpeed;
    }

    void OnDisable()
    {
        EventManager.Instance.BusSpeed -= SetSpeed;
    }

    void Start()
    {
        
    }

    void SetSpeed(float value)
    {
        float _speed = (value - 2f) / (30f - 2f);
        
        speed = Mathf.Clamp(_speed * 100f, defaultParticleSpeed, maxParticleSpeed);
    }

    void Update()
    {
        var main = speedLines.main;
        main.startSpeed = speed;
    }
}
