using UnityEngine;

public class PlayerHitPoint : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private PlayerHealth playerHealth;
    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        playerHealth = GetComponentInParent<PlayerHealth>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Explosion") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyAttack"))
        {
            boxCollider.enabled = false;
            PlayerMusicScript.instance.PlayDeathSound();
            playerHealth.PlayerDeath();
            if (GameManager.instance == null) return;
            GameManager.instance.ResetPowerUps();
        }
    }
}
