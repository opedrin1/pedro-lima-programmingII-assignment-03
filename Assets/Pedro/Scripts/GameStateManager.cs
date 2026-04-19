using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState { MainMenu, Playing, Paused, Win, Lose }

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    public GameState CurrentGameState { get; private set; }

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

        GameUI.Instance.OnStateChanged(newState);
    }
}