using Unity.VisualScripting;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    //public AudioClip hitSound; // The sound to play when the arrow hits an enemy
    public GameObject hitParticlePrefab; // The particle effect to spawn when the arrow hits an enemy
    public int damage;
    public float arrowSpeed;
    private Rigidbody2D rb;
    [SerializeField] private float targetYOffset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 1f);

    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        // Check if the arrow collides with an object tagged "Enemy"
        if (other.gameObject.CompareTag("Player"))
        {
            // Play the hit sound
            //AudioSource.PlayClipAtPoint(hitSound, transform.position);
            if (!PlayerController.Instance.GetComponent<RecoveryCounter>().recovering)
            {
                // Spawn the hit particle effect
                if (hitParticlePrefab != null)
                {
                    Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
                    Destroy(hitParticlePrefab, 0.4f);
                }

                if (!PlayerController.Instance.isBlocking)
                {
                    PlayerController.Instance.stats.PlayerTakeDamage(damage);
                    if(PlayerController.Instance.stats.health <= 0)
                    {
                        HUD.Instance.ShowDeathMenu();
                    }
                    PlayerController.Instance.GetComponent<RecoveryCounter>().ResetCounter();
                }
                else
                {
                    rb.AddForce(new Vector2(0, Random.Range(0.09f, 0.2f)), ForceMode2D.Impulse);
                }
            }
            Destroy(this.gameObject, 1f);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.CompareTag("Player attack") || PlayerController.Instance.isBlocking)
    //     {
    //         if (hitParticlePrefab != null)
    //         {
    //             Instantiate(hitParticlePrefab, transform.position, Quaternion.identity);
    //             Destroy(hitParticlePrefab, 0.4f);
    //         }
    //         rb.AddForce(new Vector2(0, Random.Range(0.09f, 0.2f)), ForceMode2D.Impulse);
    //     }
    // }



    public void SetDamage(int a)
    {
        damage = a;
    }
}