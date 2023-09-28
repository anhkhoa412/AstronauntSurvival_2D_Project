using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public Rigidbody2D rb;   // Reference to the Rigidbody2D component
    public float speed;       // Projectile's speed
    public int damage;        // Damage inflicted by the projectile
    public float lifetime = 3f; // Time before the projectile is automatically destroyed

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component of the projectile
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 bulletMovement = speed * Time.deltaTime * transform.forward;

        // Calculate the movement vector for the projectile
       

        // Apply the movement to the Rigidbody2D's position
        rb.MovePosition(rb.position + bulletMovement);

        Invoke("DestroyObject", lifetime);
    }

    // Called when the projectile collides with a trigger collider
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag( "Player"))
        {
            // Inflict damage to the player's health
            other.gameObject.GetComponent<PlayerLife>().TakeDamage(damage);
        }
    }

    public void DestroyObject()
    {
        Destroy(rb);
    }
}
