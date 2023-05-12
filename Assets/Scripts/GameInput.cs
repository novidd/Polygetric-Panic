using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour {
    public PlayerInputActions _playerActions { get; private set; }

    public event EventHandler OnShootAction;
    public event EventHandler OnRestartAction;

    private void Awake() {
        _playerActions = new PlayerInputActions();
        _playerActions.Player.Enable();

        // Event actions
        _playerActions.Player.Shoot.performed += Shoot_performed; ;
        _playerActions.Menu.Restart.performed += Restart_performed;
    }

    private void Restart_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnRestartAction?.Invoke(this, EventArgs.Empty);
    }

    private void Shoot_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnShootAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetPlayerMovementNormalized() {
        Vector2 inputVector = _playerActions.Player.Move.ReadValue<Vector2>();

        // Normalize to prevent different faster movement speeds when moving diagonally
        inputVector = inputVector.normalized;
        
        return inputVector;
    }
}
