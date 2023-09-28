using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MyEnemy : MonoBehaviour
{
    public float speed; // The movement speed of the enemy
    public Transform player; // Reference to the player's transform
    public Animator animator; // Reference to the animator component

    public Vector3 directionToPlayer; // Direction vector to the player

    public float damage; // Damage inflicted by the enemy

    public GameObject[] heart; // Array of heart GameObjects representing health
    public int health; // Current health of the enemy

    public bool isRange;
    // Distance settings
    public float distanceToShoot = 5f;
    public float distanceToStop = 3f;
    // Shot point for projectiles
    public Transform shotPoint;
    public GameObject EnemyProjectile;
    // Transform of the weapon for rotation
    public Transform weapon;

    // Shooting rate variables
    public float timeBetweenShots;
    float nextShotTime;

    public float offset; // Offset for weapon rotation
    private bool isCollided;
    // Called when the script component is initialized
    public virtual void Start()
    {
        player = FindObjectOfType<MyPlayerController>().transform; // Find the player's transform
        animator = GetComponent<Animator>(); // Get the animator component
        nextShotTime = timeBetweenShots;
    }

    // Called once per frame
    public virtual void Update()
    {
        if (!isRange)
        {
            RotateTowardsPlayer(); // Rotate enemy towards player
                                   // Move the enemy towards the player's position
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }

        if (isRange)
        {
            // Only engage if player is within shooting range
            if (Vector2.Distance(player.position, transform.position) > distanceToShoot)
            {
                RotateTowardsPlayer(); // Rotate enemy towards player
                                       // Move the enemy towards the player's position
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }

            // Check if it's time to shoot
            if (Time.time - nextShotTime >= timeBetweenShots)
            {
                WeaponRotation();
                Shoot();
                nextShotTime = Time.time;
            }
        }

        if (isCollided)
        {
            player.gameObject.GetComponent<PlayerLife>().TakeDamage(damage);
            Debug.Log("collide");
        }
    }

    private void Shoot()
    {
        if (Time.time > nextShotTime)
        {
            if (Vector2.Distance(player.position, transform.position) <= distanceToShoot)
            {
                Instantiate(EnemyProjectile, shotPoint.position, shotPoint.rotation);
                nextShotTime = timeBetweenShots;
            }
        }
        else
        {
            timeBetweenShots -= Time.deltaTime; // Reduce the time between shots
        }
    }

    // Rotates the weapon towards the player
    private void WeaponRotation()
    {
        Vector3 displacement = player.position - weapon.position; // Calculate displacement relative to weapon position

        // Check if the enemy is flipped
        float flipMultiplier = Mathf.Sign(transform.localScale.x); // +1 if not flipped, -1 if flipped

        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        angle += offset;

        // Adjust the angle based on the enemy's orientation
        if (flipMultiplier == -1)
        {
            angle = 180f - angle; // Flip the angle if the enemy is flipped
        }

        weapon.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    // Rotate the enemy towards the player's position
    public virtual void RotateTowardsPlayer()
    {
        directionToPlayer = player.position - transform.position; // Calculate direction to player
       
        float dotProduct = Vector3.Dot(directionToPlayer, transform.right); // Calculate dot product
        if (dotProduct > 0)
        {
            // Player is on the right side, flip the sprite to face right
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else
        {
            // Player is on the left side, flip the sprite to face left
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    // Called when the enemy collider triggers with another collider
    public  void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Projectile")
        {
            TakeDamage(other.GetComponent<Projectile>().damage); // Take damage from the projectile
        }
        if (health == 0)
        {
            speed = 0; // Set speed to 0 when health reaches 0
            animator.SetBool("isDeath", true); // Set death animation state
        }
        if (other.tag == "Player")
        {
            isCollided = true;
            // Inflict damage on the player
            
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            isCollided = false;
            
        }
    }

    // Reduce the enemy's health by a given damage amount
    public virtual void TakeDamage(int damageAmount)
    {
        health -= damageAmount; // Reduce health
        health = Mathf.Clamp(health, 0, heart.Length + 1); // Clamp health value

        // Disable heart GameObjects based on health
        for (int i = 0; i < heart.Length; i++)
        {
            if (i >= health)
            {
                heart[i].SetActive(false);
            }
        }
    }

    // Handle the enemy's death
    public virtual void Death()
    {
        // Check if health is less than or equal to 0
        if (health <= 0)
        {
            GameManager.Instance.IncreaseScore(damage); // Increase the player's score

            Destroy(gameObject); // Destroy the enemy

           // animator.SetBool("isDeath", false); // Reset death animation state
        }
    }
}
