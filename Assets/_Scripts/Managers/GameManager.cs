using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [field:SerializeField] public static bool isPaused { get; private set; }

    [SerializeField] private AudioClip gameMusic;
    [SerializeField] private AudioClip gameAmbience;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable() 
    {
        EventManager.OnGameWon += GameWon;
        EventManager.OnGameOver += GameOver;
        EventManager.OnGamePaused += Pause;
    }

    private void OnDisable() 
    {
        EventManager.OnGameWon -= GameWon;
        EventManager.OnGameOver -= GameOver;
        EventManager.OnGamePaused -= Pause;
    }

    private void Start() 
    {
        EventManager.OnGameStart?.Invoke();

        AudioManager.PlayMusic(gameMusic);
        AudioManager.PlayAmbience(gameAmbience);
    }

    public void GameWon()
    {
        Debug.Log("Game Won");

        SceneLoader.LoadScene(SceneLoader.Instance.GameWon);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");

        SceneLoader.LoadScene(SceneLoader.Instance.GameOver);
    }

    public void Pause()
    {
        isPaused = !isPaused;

        AudioListener.pause = isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
