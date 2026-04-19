using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState { MainMenu, Playing, Paused, Win, Lose }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentGameState { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private float menuMusicVolume = 0.05f;
    [SerializeField] private float gameMusicVolume = 0.3f;
    
    private InputAction _startAction;
    private InputAction _pauseAction;

    void Awake()
    {
        Instance = this;
        
        _startAction = new InputAction("Start", binding: "<Keyboard>/enter");
        _pauseAction = new InputAction("Pause", binding: "<Keyboard>/escape");
    }

    void OnEnable()
    {
        _startAction.performed += OnStartPressed;
        _pauseAction.performed += OnPausePressed;

        _startAction.Enable();
        _pauseAction.Enable();
    }

    void OnDisable()
    {
        _startAction.performed -= OnStartPressed;
        _pauseAction.performed -= OnPausePressed;

        _startAction.Disable();
        _pauseAction.Disable();
    }

    void Start()
    {
        if (musicSource != null)
        {
            musicSource.loop = true;
            musicSource.Play();
        }
        
        SetState(GameState.MainMenu);
    }

    private void OnStartPressed(InputAction.CallbackContext ctx)
    {
        if (CurrentGameState == GameState.MainMenu)
            SetState(GameState.Playing);
    }

    private void OnPausePressed(InputAction.CallbackContext ctx)
    {
        if (CurrentGameState == GameState.Playing)
            SetState(GameState.Paused);
        else if (CurrentGameState == GameState.Paused)
            SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        CurrentGameState = newState;

        Time.timeScale = newState == GameState.Playing ? 1f : 0f;

        if (musicSource != null)
        {
            musicSource.volume = newState == GameState.Playing ? gameMusicVolume : menuMusicVolume;
        }

        GameUI.Instance.OnStateChanged(newState);
    }
}