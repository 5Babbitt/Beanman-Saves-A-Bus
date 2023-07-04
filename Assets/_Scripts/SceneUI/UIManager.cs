using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class UIManager : MonoBehaviour
{
    private HeroInput input;
    [SerializeField] GameSettings settings;
    
    private EventManager events;

    [SerializeField] private ProgressBar temperatureMeter;
    [SerializeField] private ProgressBar healthMeter;
    [SerializeField] private HollowProgressBar altitude;

    [SerializeField] private GameObject pauseUI;

    [SerializeField] private Gradient temperatureGradient;

    float currentHeight;
    float currentSpeed;
    float currentHealth;
    float currentTemperature;

    void Awake()
    {
        input = new HeroInput();

        input.UI.Pause.started += Pause;
    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Pause Pressed");
        
            EventManager.OnGamePaused.Invoke();
            pauseUI.SetActive(GameManager.isPaused);
        }
    }

    void OnEnable()
    {
        input.Enable();
        
        events = EventManager.Instance;
        
        events.BusSpeed += SetSpeedValue;
        events.BusHeight += SetHeightValue;
        events.BusHealth += SetHealthValue;
        events.BusTemperature += SetTemperature;
    }

    void OnDisable()
    {
        input.Disable();
        
        events.BusSpeed -= SetSpeedValue;
        events.BusHeight -= SetHeightValue;
        events.BusHealth -= SetHealthValue;
        events.BusTemperature -= SetTemperature;
    }

    void Start()
    {
        SetStartValues();
    }

    void Update()
    {
        if (GameManager.isPaused)
            return;
        
        temperatureMeter.current = currentTemperature;
        healthMeter.current = currentHealth;
        altitude.current = currentHeight;
        altitude.SetMarkerText($"{Mathf.Round(currentSpeed)} m/s");

        temperatureMeter.color = temperatureGradient.Evaluate(1 - ((temperatureMeter.current - temperatureMeter.minimum) / (temperatureMeter.maximum - temperatureMeter.minimum)));
    }

    void OnValidate()
    {
        SetStartValues();
    }

    public void SetStartValues()
    {
        temperatureMeter.maximum = settings.maxTemperature;
        temperatureMeter.minimum = settings.defaultTemperature;

        healthMeter.maximum = settings.maxHealth;
        healthMeter.minimum = 0;

        altitude.maximum = 1060;
        altitude.minimum = 0;
    }

    void SetSpeedValue(float value)
    {
        currentSpeed = value;
    } 

    void SetHeightValue(float value)
    {
        currentHeight = value;
    }

    void SetHealthValue(float value)
    {
        currentHealth = value;
    }

    void SetTemperature(float value)
    {
        currentTemperature = value;
    }
}
