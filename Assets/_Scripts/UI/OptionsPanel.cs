using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsPanel : MonoBehaviour
{
    public void TriggerOptionsPanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
