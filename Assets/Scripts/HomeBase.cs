using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private Player player;

    [Header("Home Base Health")]
    [SerializeField] private float maxHealth = 100; // maximum health of HomeBase
    [SerializeField] private float currentHealth; // current health of HomeBase

    [Header("UI")]
    [SerializeField] private NumberUI healthUI;

    private void Start() {
        ResetProperties();
    }

    public void ResetProperties() {
        // Update Health
        currentHealth = maxHealth;
        healthUI.UpdateUI("", currentHealth);
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
