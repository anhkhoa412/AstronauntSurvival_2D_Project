using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HealthHeart; // Importing the HealthHeart's HeartStatus enum

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab; // Prefab for the heart UI element
    public PlayerLife playerLife;  // Reference to the PlayerLife component

    List<HealthHeart> hearts = new List<HealthHeart>(); // List to store heart UI elements

    private void OnEnable()
    {
        PlayerLife.OnPlayerDamaged += DrawHearts; // Subscribe to the OnPlayerDamaged event
    }

    private void OnDisable()
    {
        PlayerLife.OnPlayerDamaged -= DrawHearts; // Unsubscribe from the OnPlayerDamaged event
    }

    private void Start()
    {
        DrawHearts(); // Initial drawing of the hearts
    }

    public void DrawHearts()
    {
        ClearHearts(); // Clear existing hearts

        // Calculate the number of hearts to create based on player's max health
        float maxHealRemainder = playerLife.maxHealth % 2;
        int heartsToMake = (int)((playerLife.maxHealth / 2) + maxHealRemainder);

        // Create and set heart images based on player's health
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart(); // Create an empty heart UI element
        }

        for (int i = 0; i < hearts.Count; i++)
        {
            // Determine the heart status (empty, half, full) based on player's health
            int heartsStatusRemainder = (int)Mathf.Clamp(playerLife.health - (i * 2), 0, 2);
            hearts[i].SetHeartImage((HeartStatus)heartsStatusRemainder);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab); // Instantiate the heart prefab
        newHeart.transform.SetParent(transform); // Set the heart's parent

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();
        heartComponent.SetHeartImage(HealthHeart.HeartStatus.Empty); // Set the heart's image to empty
        hearts.Add(heartComponent); // Add the heart component to the list
    }

    public void ClearHearts()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject); // Destroy existing heart UI elements
        }
        hearts = new List<HealthHeart>(); // Clear the hearts list
    }
}
