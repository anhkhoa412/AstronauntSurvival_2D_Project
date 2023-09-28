using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    // References to heart sprites
    public Sprite fullHeart, halfHeart, emptyHeart;

    Image heartImage; // Reference to the Image component of the heart UI

    // Enumeration for heart status
    public enum HeartStatus
    {
        Empty = 0, // Heart is empty
        Half = 1,  // Heart is half-full
        Full = 2   // Heart is full
    }

    // Called when the component is initialized
    private void Awake()
    {
        heartImage = GetComponent<Image>(); // Get the Image component of the heart UI
    }

    // Set the heart image based on the provided status
    public void SetHeartImage(HeartStatus status)
    {
        switch (status)
        {
            case HeartStatus.Empty:
                heartImage.sprite = emptyHeart; // Set the heart image to the empty sprite
                break;
            case HeartStatus.Half:
                heartImage.sprite = halfHeart; // Set the heart image to the half sprite
                break;
            case HeartStatus.Full:
                heartImage.sprite = fullHeart; // Set the heart image to the full sprite
                break;
        }
    }
}
