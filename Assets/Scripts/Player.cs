using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour {
    public static Player Instance { get; private set; }

    [Header("Components")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;

    [Header("Movement Variables")]
    private float _movementSpeed = 5;
    private Vector2 _smoothedMovementInput;
    private Vector2 _movementInputSmoothVelocity;

    [Header("Health Variables")]
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth = 100;

    [Header("UI")]
    [SerializeField] private NumberUI healthUI;

    private Rigidbody2D _rigidBody;


    private void Awake() {
        // Make sure there is only one Player instance
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance!");
        }
        Instance = this;
    }

    private void Start()
    {
        _rigidBody = transform.GetComponent<Rigidbody2D>();

        // Update Health
        ResetProperties();

        // Actions
        GameManager.Instance._gameInput.OnShootAction += _gameInput_OnShootAction;
    }

    public void ResetProperties() {
        // Update Health
        currentHealth = maxHealth;
        healthUI.UpdateUI("", currentHealth);
    }

    private void _gameInput_OnShootAction(object sender, EventArgs e) {
        //Debug.Log("Pressed space!");
    }

    private void Update() {
        // Temporary solution, Instantiating doesn't work otherwise
        if (Input.GetKeyDown(KeyCode.Space)) {
            HandleShooting();
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement() {
        Vector2 inputVector = GameManager.Instance._gameInput.GetPlayerMovementNormalized();

        // Smoothen the player movement
        _smoothedMovementInput = Vector2.SmoothDamp(
            _smoothedMovementInput,
            inputVector,
            ref _movementInputSmoothVelocity,
            0.1f
            );

        _rigidBody.velocity = _smoothedMovementInput * _movementSpeed;
    }

    void HandleShooting() {
        // Instantiate a new bullet object at the bullet spawn point position and rotation
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

        // Play SFX
        AudioManager.Instance.Play("Shoot");
    }

    public void TakeDamage(float amount) {

        // Play Damage SFX
        AudioManager.Instance.Play("PlayerDamage");

        // Do camera shake
        GameManager.Instance.cameraShake.Shake();

        // Reduce health
        currentHealth -= amount;
        healthUI.UpdateUI("", currentHealth);

        // Flash to show damage taken
        transform.GetComponent<SimpleFlash>().Flash();

        // Despawn the object if health reaches 0 or below
        if (currentHealth <= 0) {
            // Play Death SFX
            AudioManager.Instance.Play("Death");

            // Update Health
            currentHealth = 0;
            healthUI.UpdateUI("", currentHealth);

            // Update game state
            GameManager.Instance.UpdateGameState(GameState.Lose);
        }
    }
}
