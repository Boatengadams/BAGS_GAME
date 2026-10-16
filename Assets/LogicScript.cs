using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int pointsPerSpeedIncrease = 10;
    public float speedIncreaseAmount = 1f;
    public Text scoreText;
    public GameObject gameOverScreen;
    private GUIStyle fallbackScoreStyle;
    private bool gameIsOver;
    private AudioSource musicSource;
    private AudioSource soundEffectsSource;
    private AudioClip deathSound;
    private int nextSpeedIncreaseAt;

    private void Awake()
    {
        Time.timeScale = 1f;
        PipeMoveScript.ResetSpeedBonus();
        nextSpeedIncreaseAt = Mathf.Max(1, pointsPerSpeedIncrease);

        if (gameOverScreen == null)
        {
            gameOverScreen = GameObject.Find("GameOver");
        }

        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString();
        }

        AudioSource[] existingSources = GetComponents<AudioSource>();
        musicSource = existingSources.Length > 0 ? existingSources[0] : gameObject.AddComponent<AudioSource>();
        soundEffectsSource = existingSources.Length > 1 ? existingSources[1] : gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.volume = 0.3f;
        soundEffectsSource.playOnAwake = false;
        soundEffectsSource.spatialBlend = 0f;

        AudioClip backgroundMusic = Resources.Load<AudioClip>("Audio/Fameye GenZ");
        deathSound = Resources.Load<AudioClip>("Audio/Death");
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
        }
        else
        {
            Debug.LogError("Could not load Audio/Fameye GenZ from Resources.", this);
        }
    }

    private void Start()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    private void OnGUI()
    {
        if (scoreText != null)
        {
            return;
        }

        if (fallbackScoreStyle == null)
        {
            fallbackScoreStyle = new GUIStyle(GUI.skin.label)
            {
                fontSize = 32,
                fontStyle = FontStyle.Bold,
                normal = { textColor = Color.white }
            };
        }

        GUI.Label(new Rect(20f, 20f, 240f, 50f), "Score: " + playerScore, fallbackScoreStyle);
    }

    public void AddScore()
    {
        playerScore++;

        while (playerScore >= nextSpeedIncreaseAt)
        {
            PipeMoveScript.IncreaseSpeed(speedIncreaseAmount);
            nextSpeedIncreaseAt += Mathf.Max(1, pointsPerSpeedIncrease);
        }

        if (scoreText != null)
        {
            scoreText.text = playerScore.ToString();
        }
    }

    public void GameOver()
    {
        if (gameIsOver)
        {
            return;
        }

        gameIsOver = true;
        if (musicSource != null)
        {
            musicSource.Stop();
        }

        if (deathSound != null)
        {
            soundEffectsSource.PlayOneShot(deathSound);
        }

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
