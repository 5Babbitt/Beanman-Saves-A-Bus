using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuMusic;
    [SerializeField] private AudioClip[] ambience;

    void Start()
    {
        AudioManager.PlayMusic(mainMenuMusic);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneLoader.LoadScene(SceneLoader.Instance.GameScene);
    }
}
