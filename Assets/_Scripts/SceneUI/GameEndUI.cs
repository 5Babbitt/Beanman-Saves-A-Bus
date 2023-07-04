using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndUI : MonoBehaviour
{
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip[] ambience;

    public void Restart()
    {
        SceneLoader.LoadScene(SceneLoader.Instance.GameScene);
    }

    public void MainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.Instance.MainMenu);
    }

    void Start()
    {
        AudioManager.PlayMusic(music);
        
        foreach (var clip in ambience)
        {
            AudioManager.PlayAmbience(clip);
        }
    }
}
