using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGUI : Singleton<DebugGUI>
{
    private Rect debugRect;
    public GUIStyle style;
    public GUIStyle headingStyle;

    private Bus bus;
    private GameManager controller;
    private HeroController hero;

    [Header("Debug Heights")]
    [SerializeField] private float settingsHeight;
    [SerializeField] private float headingHeight;
    [SerializeField] private float busSettingsY, heroSettingsY, gameSettingsY;

    protected override void Awake() 
    {
        base.Awake();
        
        
    }
    
    void Start()
    {
        debugRect = new Rect(10, 10, 500f, 100f);

        style = new GUIStyle();

        style.fontSize = 28;
        style.normal.textColor = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI() 
    {
        bus = Bus.Instance;
        controller = GameManager.Instance;
        hero = HeroController.Instance;
        
        if (!(Application.isEditor || Debug.isDebugBuild))
            return;

        GUI.Label(new Rect(10, busSettingsY, 200f, headingHeight), "Bus Values", headingStyle);
        GUI.Label(new Rect(10, busSettingsY + headingHeight, 200f, settingsHeight), 
        $"Temperature:\nHealth:\nCurrent Up Force:\nIs Going Up:\n", style);
        GUI.Label(new Rect(360, busSettingsY + headingHeight, 200f, settingsHeight), 
        $"{bus.busTemperature}\n{bus.busHealth}\n{bus.currentUpForce}\n{bus.isGoingUp}\n", style);

        GUI.Label(new Rect(10, heroSettingsY, 200f, headingHeight), "Hero Values", headingStyle);
        GUI.Label(new Rect(10, heroSettingsY + headingHeight, 200f, settingsHeight), 
        $"Is Pressing Up:\n", style);
        GUI.Label(new Rect(360, heroSettingsY + headingHeight, 200f, settingsHeight), 
        $"{hero.isPressingUp}", style);

        GUI.Label(new Rect(10, gameSettingsY, 200f, headingHeight), "Game Values", headingStyle);
        GUI.Label(new Rect(10, gameSettingsY + headingHeight, 200f, settingsHeight), 
        $"Current Speed:\nCurrent Height:\n", style);
        //GUI.Label(new Rect(360, gameSettingsY + headingHeight, 200f, settingsHeight), 
        //$"{controller.currentSpeed}\n{controller.currentHeight}\n", style);
    }
}
