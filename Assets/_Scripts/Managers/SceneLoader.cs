using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public int MainMenu, GameScene, GameWon, GameOver;

    public static void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static Scene CurrentScene()
    {
        return SceneManager.GetActiveScene();
    }

    private void OnEnable() 
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    private void OnDisable() 
    {
        SceneManager.activeSceneChanged += OnSceneChanged;
    }

    void OnSceneChanged(Scene current, Scene next)
    {
        AudioManager.StopAmbience();
    }
}
