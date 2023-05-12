using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    [Header("Game Manager")]
    public static GameManager Instance;

    public GameState currentState;

    public static event Action<GameState> OnGameStateChanged;

    [Header("Input")]
    public GameInput _gameInput;

    [Header("Player")]
    [SerializeField] private Player player;
    [SerializeField] private HomeBase homeBase;
    [SerializeField] private GameObject startPosition;

    [Header("UI")]
    // Maybe make an UI manager?
    [SerializeField] private Canvas hudCanvas;
    [SerializeField] private GameObject gameOverContainer;

    [Header("Camera Shake")]
    public CameraShake cameraShake;

    [Header("Highscore")]
    [SerializeField] private NumberUI scoreUI;
    [SerializeField] private GameObject newHighScore;
    public int currentScore { get; set; }
    private int highScore;


    // Spawn a healthpack from time to time

    private void Awake() {
        Instance = this;

        _gameInput.OnRestartAction += _gameInput_OnRestartAction;
    }
    private void Start() {
        cameraShake = GetComponent<CameraShake>();

        UpdateGameState(GameState.GameLoop);
    }

    // The player presses space when on the "Game Over" screen - start game loop again
    private void _gameInput_OnRestartAction(object sender, EventArgs e) {
        UpdateGameState(GameState.GameLoop);
    }

    public void UpdateGameState(GameState newState) {
        currentState = newState;

        switch(newState) {
            case GameState.MainMenu:
                HandleMainMenu();
                break;
            case GameState.GameLoop:
                HandleGameLoop();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnGameStateChanged?.Invoke(newState);
    }

    private void HandleMainMenu() { }

    private void HandleGameLoop() {
        // Reset stuff
        ResetControls(currentState);
        ResetGame();

        // Enable HUD with health and high score
        hudCanvas.gameObject.SetActive(true);

        // Make sure game over screen is off
        gameOverContainer.gameObject.SetActive(false);

        // Enable player
        player.gameObject.SetActive(true);
    }

    private void HandleLose() {
        // Reset stuff
        ResetControls(currentState);

        // Disable player instance
        player.gameObject.SetActive(false);

        // Handle the highscore
        HandleHighscore();

        // Show game over screen
        gameOverContainer.gameObject.SetActive(true);
    }

    public void IncreaseScore(int scoreToAdd) {
        currentScore += scoreToAdd;
        scoreUI.UpdateUI("Score: ", currentScore);
    }

    private void HandleHighscore() {
        if (currentScore > highScore) {
            highScore = currentScore;
            newHighScore.GetComponent<TextMeshProUGUI>().text = "New high score: " + highScore.ToString() + "!";
            newHighScore.gameObject.SetActive(true);
        } else {
            newHighScore.GetComponent<TextMeshProUGUI>().text = "High score: " + highScore.ToString();
            newHighScore.gameObject.SetActive(false);
        }
    }

    private void ResetControls(GameState state) {
        if (state == GameState.GameLoop) {
            _gameInput._playerActions.Player.Enable();

            _gameInput._playerActions.Menu.Disable();

        } else if (state == GameState.Lose) {
            _gameInput._playerActions.Player.Disable();

            _gameInput._playerActions.Menu.Enable();
        }

    }

    private void ResetGame() {
        // Reset player position
        player.transform.position = startPosition.transform.position;

        // Reset health properites
        player.ResetProperties();
        homeBase.ResetProperties();

        // Reset score
        currentScore = 0;
        scoreUI.UpdateUI("Score: ", 0);

        // Delete all comets in the list
        foreach (var item in SpawnFallingObjects.Instance.cometList) {
            Destroy(item.gameObject);
        }
        SpawnFallingObjects.Instance.cometList.Clear();
    }
}

public enum GameState {
    MainMenu,
    GameLoop,
    Lose
}
