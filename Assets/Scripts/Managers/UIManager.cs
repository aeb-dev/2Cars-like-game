using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Image startPanel;
    public Button playButton;

    public RectTransform playPanel;
    public Text scoreText;
    public Button pauseButton;

    public Image pausePanel;
    public Text pauseTitle;
    public Button resumeButton;

    public Image gameOverPanel;
    public Text gameOverTitle;
    public Text gameOverScoreText;
    public Button restartButton;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(this);

        startPanel.gameObject.SetActive(true);
        playPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void Play()
    {
        startPanel.gameObject.SetActive(false);
        playPanel.gameObject.SetActive(true);
    }

    public void Pause()
    {
        pauseButton.gameObject.SetActive(false);
        pausePanel.gameObject.SetActive(true);
    }

    public void Resume()
    {
        pauseButton.gameObject.SetActive(true);
        pausePanel.gameObject.SetActive(false);
    }

    public void Restart()
    {
        playPanel.gameObject.SetActive(true);
        gameOverPanel.gameObject.SetActive(false);

        scoreText.text = "Score: 0";
    }

    public void GameOver(int score)
    {
        playPanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(true);

        gameOverScoreText.text = $"Score: {score}";
        gameOverScoreText.gameObject.SetActive(true);
    }
}
