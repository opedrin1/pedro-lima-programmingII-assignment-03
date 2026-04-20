using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance;

    [Header("HUD")]
    public TMP_Text chestCounterText;
    public TMP_Text timerText;
    public Image[] heartImages;
    public Sprite heartFull;
    public Sprite heartEmpty;

    [Header("Settings")]
    public float timeLimit = 300f;
    
    [Header("Menus")]
    public GameObject mainMenuPanel;
    public GameObject hudPanel;
    public GameObject pausePanel;
    public GameObject winPanel;
    public GameObject losePanel;
    
    [Header("References")]
    public MouseBehaviour mouseBehaviour;
    public PlayerHealth playerHealth;

    private int _totalChests;
    private int _collectedChests;
    private float _timeRemaining;
    private bool _timerRunning = true;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _totalChests = FindObjectsByType<ChestInteractable>(FindObjectsSortMode.None).Length;
        _timeRemaining = timeLimit;
        UpdateChestUI();
        
        playerHealth.OnLivesChanged += UpdateHeartsUI;
        playerHealth.OnPlayerDeath += () => GameStateManager.Instance.SetState(GameState.Lose);
    }

    void Update()
    {
        if (!_timerRunning) return;

        _timeRemaining -= Time.deltaTime;

        if (_timeRemaining <= 0)
        {
            _timeRemaining = 0;
            _timerRunning = false;
            GameStateManager.Instance.SetState(GameState.Lose);
        }
        
        UpdateTimerUI();
    }
    
    public void OnStateChanged(GameState state)
    {
        // hide all panels first
        mainMenuPanel.SetActive(false);
        hudPanel.SetActive(false);
        pausePanel.SetActive(false);
        winPanel.SetActive(false);
        losePanel.SetActive(false);

        switch (state)
        {
            case GameState.MainMenu:
                mainMenuPanel.SetActive(true);
                _timerRunning = false;
                mouseBehaviour.ShowMouse(true);
                break;

            case GameState.Playing:
                hudPanel.SetActive(true);
                _timerRunning = true;
                mouseBehaviour.ShowMouse(false);
                break;

            case GameState.Paused:
                hudPanel.SetActive(true); 
                pausePanel.SetActive(true);
                _timerRunning = false;
                mouseBehaviour.ShowMouse(true);
                break;

            case GameState.Win:
                winPanel.SetActive(true);
                _timerRunning = false;
                mouseBehaviour.ShowMouse(true);
                break;

            case GameState.Lose:
                losePanel.SetActive(true);
                _timerRunning = false;
                mouseBehaviour.ShowMouse(true);
                break;
        }
    }

    public void RegisterChestCollected()
    {
        _collectedChests++;
        UpdateChestUI();
        
        if (_collectedChests >= _totalChests)
        {
            GameStateManager.Instance.SetState(GameState.Win);
        }
    }

    void UpdateChestUI()
    {
        chestCounterText.text = $"Chests: {_collectedChests} / {_totalChests}";
    }

    void UpdateHeartsUI(int currentLives)
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].sprite = i < currentLives ? heartFull : heartEmpty;
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(_timeRemaining / 60);
        int seconds = Mathf.FloorToInt(_timeRemaining % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";

        // turn red when under 30 seconds
        timerText.color = _timeRemaining <= 30f ? Color.red : Color.white;
    }
    
    public void OnRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitButton()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    
}
