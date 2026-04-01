using System;
using UnityEngine;

public enum GameState { MainMenu, Playing, Paused, Win, Lose }
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    
    public GameState CurrentGameState { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetState(GameState.MainMenu);
    }

    void Update()
    {
        if (CurrentGameState == GameState.MainMenu && Input.GetKeyDown(KeyCode.Space))
            SetState(GameState.Playing);

        if (CurrentGameState == GameState.Playing && Input.GetKeyDown(KeyCode.Escape))
            SetState(GameState.Paused);
        else if (CurrentGameState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape))
            SetState(GameState.Playing);
    }

    public void SetState(GameState newState)
    {
        CurrentGameState = newState;
        
        // freeze/unfreeze time
        Time.timeScale = newState == GameState.Playing ? 1f : 0f;
        
        GameUI.Instance.OnStateChanged(newState);
    }
}
