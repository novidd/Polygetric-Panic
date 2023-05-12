using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GridBrushBase;

public class Comet : MonoBehaviour {
    [SerializeField] private float maxHealth = 15; // maximum health
    [SerializeField] private float damageToDealHomeBase = 10; // damage dealt to HomeBase
    [SerializeField] private float damageToDealPlayer = 15; // damage dealt to Player
    [SerializeField] private int scoreToGive = 125; // Score to give
    
    private float speed = 2f; // movement speed
    private float currentHealth; // current health

    private Rigidbody2D rb2d;

    void Start() {
        currentHealth = maxHealth;

        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update() {
        // move the object down with speed
        Vector2 pos = rb2d.position;
        pos.y -= speed * Time.deltaTime;
        rb2d.MovePosition(pos);
    }

    public void TakeDamage(float amount) {

        // Play Comet Damage SFX
        AudioManager.Instance.Play("PlayerDamage");

        // Reduce health
        currentHealth -= amount;

        // Increment score on each bullet hit
        GameManager.Instance.IncreaseScore(scoreToGive);

        // Flash to show damage taken
        transform.GetComponent<SimpleFlash>().Flash();

        // Destroy the comet if health reaches 0 or below
        if (currentHealth <= 0) {
            // Play Comet Destroy SFX
            AudioManager.Instance.Play("Death");

            // Didn't get it to work, so the list of comets gets filled with empty game objects till you lose :(
            //for (int i = 0; i <= SpawnFallingObjects.Instance.cometList.Count; i++) {
            //    if (SpawnFallingObjects.Instance.cometList[i] == transform.gameObject) {
            //        Destroy(SpawnFallingObjects.Instance.cometList[i]);
            //        //SpawnFallingObjects.Instance.cometList.RemoveAt(i);
            //        break;
            //    }
            //}
            //Destroy(gameObject);
            CleanCometFromCometList();
        }
    }

    private void CleanCometFromCometList() {
        // Didn't get it to work, so the list of comets gets filled with empty game objects till you lose :(
        //for (int i = 0; i < SpawnFallingObjects.Instance.cometList.Count; i++) {
        //    if (SpawnFallingObjects.Instance.cometList[i] == transform.gameObject) {
        //        SpawnFallingObjects.Instance.cometList.RemoveAt(i);
        //        Destroy(gameObject);
        //        return;
        //    }
        //}

        //if (SpawnFallingObjects.Instance.cometList.Contains(gameObject)) {
        //    SpawnFallingObjects.Instance.cometList.Remove(gameObject);
        //    Destroy(gameObject);
        //}
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        
        // On bullet collision nothing happens
        if (other.GetComponent<Bullet>() != null) return;

        if (other.GetComponent<HomeBase>()) {
            // Deal damage to home base
            other.GetComponent<HomeBase>().TakeDamage(damageToDealHomeBase);

            // Destroy the comet
            //Destroy(gameObject);
            CleanCometFromCometList();
        }
        else if (other.GetComponent<Player>()) {
            // Deal damage to player
            other.GetComponent<Player>().TakeDamage(damageToDealPlayer);

            // Destroy the comet
            //Destroy(gameObject);
            CleanCometFromCometList();
        }
    }
}
