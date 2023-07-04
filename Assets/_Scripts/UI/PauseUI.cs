using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    
    
    public void Restart()
    {
        EventManager.OnGamePaused.Invoke();
            
        SceneLoader.LoadScene(SceneLoader.Instance.GameScene);
    }

    public void MainMenu()
    {
        EventManager.OnGamePaused.Invoke();
            
        SceneLoader.LoadScene(SceneLoader.Instance.MainMenu);
    }
}
