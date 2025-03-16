using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int player1Score = 0;
    [SerializeField] private int player2Score = 0;
    [SerializeField] private int scoreToWin = 7;

    private int winningPlayer;

    public event Action<int> OnGameEnded;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private bool multiPlayerLevel = false;
    [SerializeField] private TextMeshProUGUI winText;

    private void Awake()
    {
        // If there is already an instance and it's not this one, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set this as the active instance
        Instance = this;

        UpdateScoreUI();
        HideWinLoseScreens();
    }

    private void UpdateScoreUI()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "Player 1: " + player1Score.ToString();
        }
        if (player2ScoreText != null)
        {
            player2ScoreText.text = "Player 2: " + player2Score.ToString();
        }
    }

    private void UpdateScoreUITarget()
    {
        if (player1ScoreText != null)
        {
            player1ScoreText.text = "Score: " + player1Score.ToString();
        }
    }

    public void IncrementScoreForOppositionOf(int player)
    {
        //Called by player when they take damage
        if (player == 1)
        {
            player2Score++;
        }
        else if (player == 2)
        {
            player1Score++;
        }
        UpdateScoreUI();
        CheckWinConditionFirstToReachScore();
    }

    public void IncrementScoreForTarget()
    {
        player1Score++;
        UpdateScoreUITarget();
    }

    private void CheckWinConditionFirstToReachScore()
    {
        if (player1Score == scoreToWin)
        {
            Debug.Log("Player 1 wins!");
            winningPlayer = 1;
            ShowEndScreen(winScreen, winSound);
            EndGame();
        }
        else if (player2Score == scoreToWin)
        {
            Debug.Log("Player 2 wins!");
            winningPlayer = 2;
            ShowEndScreen(loseScreen, loseSound);
            EndGame();
        }
    }

    //Called when time runs out
    public void CheckWhoHasMostPoints()
    {
        if (player1Score > player2Score)
        {
            winningPlayer = 1;
            ShowEndScreen(winScreen, winSound);
            Debug.Log("Player 1 wins!");
        }
        else if (player2Score > player1Score)
        {
            winningPlayer = 2;
            ShowEndScreen(loseScreen, loseSound);
            Debug.Log("Player 2 wins!");
        }
        else
        {
            winningPlayer = 0;
            ShowEndScreen(loseScreen, loseSound);
            Debug.Log("It's a tie!");
        }
        EndGame();
    }
    private void ShowEndScreen(GameObject screen, AudioClip sound)
    {
        HideWinLoseScreens();
        if (multiPlayerLevel)
        {
            if (winningPlayer == 0)
            {
                winText.text = "DRAW!";
            }
            else
            {
                winText.text = "PLAYER " + winningPlayer + " WINS!";
            }
            winScreen.SetActive(true);
        }
        if (screen != null)
        {
            screen.SetActive(true);
        }
        PlaySound(sound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void HideWinLoseScreens()
    {
        if (winScreen != null) winScreen.SetActive(false);
        if (loseScreen != null) loseScreen.SetActive(false);
    }

    private void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        winningPlayer = 0;
        HideWinLoseScreens();
    }

    private void EndGame()
    {
        OnGameEnded?.Invoke(winningPlayer);

        if (winningPlayer == 1)
        {
            // Level progression logic remains the same.
            string currentScene = SceneManager.GetActiveScene().name;
            if (currentScene == "PvE")
            {
                Debug.Log("Level 2");
                SceneManager.LoadScene("PvE2");
            }
            else if (currentScene == "PvE2")
            {
                Debug.Log("Level 3");
                SceneManager.LoadScene("PvE3");
            }
        }
        else
        {
            // For a loss, delay the reset so the UI can stay visible.
            StartCoroutine(DelayedReset());
        }
    }

    private IEnumerator DelayedReset()
    {
        // Wait for 3 seconds before hiding the screen and resetting scores.
        yield return new WaitForSeconds(3f);
        ResetScores();
        UpdateScoreUI();
    }

}
