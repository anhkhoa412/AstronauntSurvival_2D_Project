using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

public class MyPlayerController : MonoBehaviour
{
    public float speed = 5f; // Movement speed of the player
    public Transform weapon; // Reference to the weapon's transform
    public float offset;     // Offset for weapon rotation

    public Rigidbody2D rb;   // Player's Rigidbody2D component

    public Animator animator; // Player's Animator component

    public Transform shotPoint;     // Position where projectiles are shot from
    public GameObject projectile;   // Prefab of the projectile to shoot
   

    public GameObject shootEffect;       // Particle effect when shooting
    public GameObject slideEffect;
    private int score;              // Player's score

    public float timeBetweenShots;  // Time delay between shots
    float nextShotTime;             // Time of the next available shot

    private bool isSlide = false;   // Is the player currently sliding?

    [SerializeField] private AudioSource walkingSound; // Walking sound effect
    [SerializeField] private AudioSource shootingSound; // Shooting sound effect
    [SerializeField] private AudioSource slideSound;    // Slide sound effect

    Vector2 movement; // Stores movement input from the player

    public SpriteRenderer spriteRender; // Player's sprite renderer

    // Start is called before the first frame update
    void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        slideEffect.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
     
        Movement();
        Slide();
        // Rotate weapon and handle shooting
        WeaponRotation();
        Shotting();
      
    }
    public void Movement()
    {
        // Get movement input from the player
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement.Normalize();
        //Get mouse Position
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Calculate the direction vector from the player to the mouse cursor
        Vector2 direction = (mousePosition - transform.position).normalized;
        // Calculate the angle in degrees
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Update animator parameters based on movement
        if (movement != Vector2.zero)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
        else
        {
            // Use the angle to set the horizontal and vertical parameters when the player is not moving
            animator.SetFloat("Horizontal", Mathf.Cos(angle * Mathf.Deg2Rad));
            animator.SetFloat("Vertical", Mathf.Sin(angle * Mathf.Deg2Rad));
            animator.SetFloat("Speed", 0f);
        }
    }
    public void Slide()
    {
        // Check for slide input
        if (Input.GetKeyDown(KeyCode.Space) && !isSlide)
        {
            isSlide = true;
            speed = 10f;

            slideEffect.SetActive(true);



            // Play slide sound
            slideSound.Play();
        }

        // Update animator parameter for sliding
        animator.SetBool("isSlide", isSlide);
    }
    // Called by animation event to end sliding animation
    public void OnSliceAnimationEnd()
    {
        isSlide = false;
        animator.SetBool("isSlide", isSlide);
        slideEffect.SetActive(false);
        speed = 5f; // Reset speed after sliding
    }

    private void FixedUpdate()
    {
        //  Calculates the new position the Rigidbody2D should be moved to by adding the product of the normalized movement vector and the speed, scaled by the time that has passed. 
        rb.MovePosition(rb.position + movement.normalized * speed * Time.fixedDeltaTime);
    }

    private void Shotting()
    {
        // Shooting logic
        if (Input.GetMouseButtonDown(0))
        {
            // Check if enough time has passed to shoot again
            if (Time.time > nextShotTime)
            {
                // Set the next available shot time
                nextShotTime = Time.time + timeBetweenShots;

                // Instantiate the projectile and effect
                GameObject bullet = ObjectPool.instance.GetPooledObject();

                if (bullet != null)
                {
                    bullet.transform.position = shotPoint.position;
                    bullet.transform.rotation = shotPoint.rotation;
                    bullet.SetActive(true);
                    //Show shooting effect
                    shootEffect.GetComponent<Renderer>().enabled = true;
                    bullet.GetComponent<Rigidbody2D>().AddForce(shotPoint.forward * speed * 30000, ForceMode2D.Impulse);
                    Invoke("DisableEffect", 0.3f);
                }

                //    // Play shooting sound
                    shootingSound.Play();
            }

        }
       
    }

    private void WeaponRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // Calculate the displacement between the weapon's position and the mouse cursor's world position
        Vector3 displacement = weapon.position - mousePosition;
        // Calculate the angle of rotation based on the displacement
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        // Rotate the weapon to the calculated angle, considering an offset
        weapon.rotation = Quaternion.Euler(0f, 0f, angle + offset);

        // Flip the Gun sprite based on the mouse position
        if (mousePosition.x < weapon.position.x)
        {
            weapon.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            weapon.localScale = new Vector3(1f, -1f, 1f);
        }

  
    }

    private void PlayWalkSound()
    {
        if (!walkingSound.isPlaying)
        {
            walkingSound.Play();
        }
    }

    private void DisableEffect()
    {
        shootEffect.GetComponent<Renderer>().enabled = false;
    }
}
