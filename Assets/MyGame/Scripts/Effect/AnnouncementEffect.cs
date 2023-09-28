using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnnouncementEffect : MonoBehaviour
{
    public float announcementDuration = 3.0f; // Duration of the announcement display
    public GameObject announcementObject;     // Reference to the GameObject to manipulate
    public float scaleFactor = 1.5f;          // Scale factor for the effect
    public float fadeDuration = 0.1f;         // Duration of the fade effect

    private Vector3 originalScale;           // Store the original scale of the GameObject
    private Color originalColor;             // Store the original color of the GameObject

    private void Start()
    {
        // Store the original scale and color of the GameObject
        originalScale = announcementObject.transform.localScale;
        originalColor = announcementObject.GetComponent<Renderer>().material.color;

        // Start the coroutine to play the announcement effect
        StartCoroutine(PlayAnnouncementEffect());
    }

    public IEnumerator PlayAnnouncementEffect()
    {
        // Scale up the announcementObject
        announcementObject.transform.localScale = originalScale * scaleFactor;

        // Fade in effect
        float startTime = Time.time;
        float endTime = startTime + fadeDuration;
        while (Time.time < endTime)
        {
            // Calculate the normalized time within the fade duration
            float normalizedTime = (Time.time - startTime) / fadeDuration;

            // Calculate the new color with fading alpha value
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(0f, originalColor.a, normalizedTime);

            // Update the GameObject's material color
            announcementObject.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(announcementDuration);

        // Scale down the announcementObject
        announcementObject.transform.localScale = originalScale;

        // Fade out effect
        startTime = Time.time;
        endTime = startTime + fadeDuration;
        while (Time.time < endTime)
        {
            // Calculate the normalized time within the fade duration
            float normalizedTime = (Time.time - startTime) / fadeDuration;

            // Calculate the new color with fading alpha value
            Color newColor = originalColor;
            newColor.a = Mathf.Lerp(originalColor.a, 0f, normalizedTime);

            // Update the GameObject's material color
            announcementObject.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }

        // Remove the AnnouncementEffect GameObject
        Destroy(gameObject);
    }
}
