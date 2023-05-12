using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    [SerializeField] private BulletSO bulletSO;

    public BulletSO GetBulletSO() { return bulletSO; }

    private void Update() {
        // Move the bullet forward over time
        transform.Translate(Vector3.up * GetBulletSO().bulletSpeed * Time.deltaTime);

        // Destroy the bullet if it goes out of the screen
        if (!GetComponent<Renderer>().isVisible) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // If the bullet collides with a comet, damage it and destroy the bullet
        Comet comet = other.GetComponent<Comet>();
        if (comet != null) {
            comet.TakeDamage(GetBulletSO().bulletDamage);
            Destroy(gameObject);
        }
    }
}
