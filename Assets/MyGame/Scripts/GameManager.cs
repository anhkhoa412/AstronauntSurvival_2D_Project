using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance of the GameManager

    public GameObject gameOverCanvas; // Reference to the game over UI canvas
    public Text gameOverScoreText;    // Text displaying the game over score
    public PlayerLife playerLife;     // Reference to the player's health

    public float startTime = 0f;      // The starting time for the count-up timer
    private float elapsedTime;        // Time elapsed since the game started
    public bool isStart = false;

    [SerializeField] private Texture2D cusor;

    private float currentScore = 0;    // The current game score

    public Text timerText;            // Text to display the elapsed time
    public Text scoreText;            // Text to display the current score
    public Text highScore;

    [SerializeField] private AudioSource backgroundSound; // Reference to the background music audio source

   // Initialize slider and text values
    public Slider volumeSlider;       // Slider to control the volume
    public Text volumeText;           // Text to display the volume percentage

   
    public Button pauseButton;
    public Button resumeButton;
    public GameObject pausePanel;
    private void Awake()
    {
        isStart = true;
        if (Instance == null)
        {
            Instance = this; // Set the instance to this GameManager
        }
    }

    void Start()
    {
        // Set the background music volume and display
        backgroundSound.volume = volumeSlider.value;
        volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";

        elapsedTime = startTime; // Initialize the elapsed time
        gameOverCanvas.SetActive(false); // Hide the game over UI
        pausePanel.SetActive(false);
        UpdateHighScore();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime; // Update the elapsed time
        UpdateTimerDisplay(); // Update the timer display
        SetCussor();
        PlayerCheck();
    }

    public void UpdateVolume(float volume)
    {
        // Update the background music volume
        backgroundSound.volume = volumeSlider.value;

        // Update the volume text
        volumeText.text = Mathf.RoundToInt(volumeSlider.value * 100) + "%";
    }

    public void GameOver()
    {
        Time.timeScale = 0; // Pause the game
       
        gameOverCanvas.SetActive(true); // Show game over UI
        isStart = false;
        UpdateHighScore();
    }
    private void PlayerCheck()
    {
        if (!isStart)
        {
            FindObjectOfType<MyPlayerController>().enabled = false;
            FindObjectOfType<PlayerLife>().enabled = false;
        } else
        {
            FindObjectOfType<MyPlayerController>().enabled = true;
            FindObjectOfType<PlayerLife>().enabled = true;
        }

    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Unpause the game
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart scene
        isStart=true;
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void IncreaseScore(float amount)
    {
        currentScore += amount;
        UpdateScoreUI();
    }

    void UpdateScoreUI()
    {
        scoreText.text = " " + currentScore; // Update the score text UI
        Debug.Log(currentScore); // Log the current score (for debugging)
    }

    void SetCussor()
    {
        if(isStart)
        {
            //  cusor.SetPixel(64, 64, Color.white);
            Cursor.SetCursor(cusor, Vector3.zero, CursorMode.ForceSoftware);
        } else
        {
            Cursor.SetCursor(default, Vector3.zero, CursorMode.ForceSoftware);
        }
    }

    private void UpdateHighScore()
    {
        // Load and update the high score from player preferences
        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);
        
        if (currentScore > hiscore)
        {
            hiscore = currentScore;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

       highScore.text = Mathf.FloorToInt(hiscore).ToString();
    }

    public void OnPauseButton()
    {
        if (isStart)
        {
            pausePanel.SetActive(true);
            isStart = false;
        }

        PauseGame();
    }

    void PauseGame()
    {
        if (!isStart)
        {
            Time.timeScale = 0f;
        }
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isStart = true;
    }
}
