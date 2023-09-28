using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    // References to components
    public Rigidbody2D rb;
    public Animator anim;

    // Timing variables for getting hit
    private float nextGetHit;
    public float timeBetweenHit = 5f;

   

    // Health-related events
    public static event Action OnPlayerDamaged;
    public static event Action OnPlayerDeath;

    // Health values
    public float health, maxHealth;
    [SerializeField] private AudioSource deathSound;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        health = maxHealth;
    }

    // Function to take damage
    public void TakeDamage(float damage)
    {
        if (Time.time > nextGetHit)
        {
            nextGetHit = Time.time + timeBetweenHit;
            health -= damage;
            OnPlayerDamaged?.Invoke(); // Trigger the player damaged event
            anim.SetBool("GotHit", true); // Set the animation parameter for getting hit
            StartCoroutine(ResetGotHitAnimation()); // Start a coroutine to reset the "GotHit" animation

            if (health <= 0)
            {
                deathSound.Play(); // Play the death sound effect
                StartCoroutine(Die()); // Start a coroutine to handle player death
            }
        }
    }

    // Coroutine for player death
    private IEnumerator Die()
    {
        OnPlayerDeath?.Invoke(); // Trigger the player death event
      // Set the rigidbody to static to freeze the player's movement
        anim.SetTrigger("Death"); // Trigger the death animation
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds
        GameManager.Instance.GameOver(); // Call the GameManager's GameOver function
      
    }

    // Coroutine to reset the "GotHit" animation
    private IEnumerator ResetGotHitAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds (adjust the delay as needed)
        anim.SetBool("GotHit", false); // Reset the "GotHit" animation parameter
    }
}
