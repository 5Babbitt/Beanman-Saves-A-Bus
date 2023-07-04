using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundHolder : Singleton<BackgroundHolder>
{
    [field:SerializeField] public Rigidbody2D RB { get; private set; }
    [field:SerializeField] public Transform START { get; private set; }
    [field:SerializeField] public Transform END { get; private set; }

    [field:SerializeField] public float DISTANCE { get; private set; } 
    
    protected override void Awake() 
    {
        base.Awake();

        SetValues();
    }

    void SetValues()
    {
        RB = GetComponent<Rigidbody2D>();

        foreach (var i in GetComponentsInChildren<GameStateTransforms>())
        {
            if (i.isEnd) END = i.transform;
            if (!i.isEnd) START = i.transform;
        }

        DISTANCE = Vector3.Distance(START.position, END.position);
    }

    private void OnValidate() 
    {
        SetValues();
    }
}
