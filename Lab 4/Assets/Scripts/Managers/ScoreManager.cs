using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int player1Score;
    [SerializeField] private int player2Score;
    [SerializeField] private int scoreToWin = 7;

    private int _winningPlayer;

    public event Action<int> OnGameEnded;
    [SerializeField] private Text player1ScoreText;
    [SerializeField] private Text player2ScoreText;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip loseSound;

    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    [SerializeField] private bool multiPlayerLevel;
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
        if (player1ScoreText)
        {
            player1ScoreText.text = "Player 1: " + player1Score;
        }
        if (player2ScoreText)
        {
            player2ScoreText.text = "Player 2: " + player2Score;
        }
    }

    private void UpdateScoreUITarget()
    {
        if (player1ScoreText)
        {
            player1ScoreText.text = "Score: " + player1Score;
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
            _winningPlayer = 1;
            ShowEndScreen(winScreen, winSound);
            EndGame();
        }
        else if (player2Score == scoreToWin)
        {
            Debug.Log("Player 2 wins!");
            _winningPlayer = 2;
            ShowEndScreen(loseScreen, loseSound);
            EndGame();
        }
    }

    public int GetWinningPlayer()
    {
        if (player1Score > player2Score)
        {
            _winningPlayer = 1;
        }
        else if (player2Score > player1Score)
        {
            _winningPlayer = 2;
        }
        else
        {
            _winningPlayer = 0;
        }
        return _winningPlayer;
    }

    //Called when time runs out
    public void CheckWhoHasMostPoints()
    {
        if (player1Score > player2Score)
        {
            _winningPlayer = 1;
            ShowEndScreen(winScreen, winSound);
            Debug.Log("Player 1 wins!");
        }
        else if (player2Score > player1Score)
        {
            _winningPlayer = 2;
            ShowEndScreen(loseScreen, loseSound);
            Debug.Log("Player 2 wins!");
        }
        else
        {
            _winningPlayer = 0;
            ShowEndScreen(loseScreen, loseSound);
            Debug.Log("It's a tie!");
        }
        EndGame();
    }
    private void ShowEndScreen(GameObject screen, AudioClip sound)
    {
        HideWinLoseScreens();
        if (_winningPlayer == 0)
        {
            winText.text = "DRAW!";
            winScreen.SetActive(true);
        }
        else if (multiPlayerLevel)
        {
            winText.text = "PLAYER " + _winningPlayer + " WINS!";
            winScreen.SetActive(true);
        }
        else if (screen )
        {
            screen.SetActive(true);
        }
        PlaySound(sound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource  && clip )
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void HideWinLoseScreens()
    {
        if (winScreen ) winScreen.SetActive(false);
        if (loseScreen ) loseScreen.SetActive(false);
    }

    private void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        _winningPlayer = 0;
        HideWinLoseScreens();
    }

    private void EndGame()
    {
        OnGameEnded?.Invoke(_winningPlayer);

        if (_winningPlayer == 1)
        {
            // Level progression logic remains the same.
            var currentScene = SceneManager.GetActiveScene().name;
            switch (currentScene)
            {
                case "PvE":
                    Debug.Log("Level 2");
                    SceneManager.LoadScene("PvE2");
                    break;
                case "PvE2":
                    Debug.Log("Level 3");
                    SceneManager.LoadScene("PvE3");
                    break;
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
