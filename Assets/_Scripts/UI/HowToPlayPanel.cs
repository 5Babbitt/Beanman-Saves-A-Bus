using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayPanel : MonoBehaviour
{
    [SerializeField] private Sprite[] contentSprites;
    [SerializeField] private Image contentImage;

    [SerializeField] private int currentIndex = 0;

    void Start()
    {
        contentImage.sprite = contentSprites[currentIndex];
    }

    public void TriggerPanel()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
    
    public void Next()
    {
        //currentIndex = (currentIndex < contentSprites.Length) ? currentIndex++ : 0;

        currentIndex++;

        contentImage.sprite = contentSprites[currentIndex % contentSprites.Length];
    }

    public void Back()
    {
        TriggerPanel();
    }
}
