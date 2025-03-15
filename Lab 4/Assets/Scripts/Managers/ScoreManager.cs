using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    private void Awake()
    {
        // If there is already an instance and it�s not this one, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set this as the active instance
        Instance = this;

        UpdateScoreUI();
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
    }

    private void CheckWinConditionFirstToReachScore()
    {
        if (player1Score == scoreToWin)
        {
            Debug.Log("Player 1 wins!");
            winningPlayer = 1;
            EndGame();
        }
        else if (player2Score == scoreToWin)
        {
            Debug.Log("Player 2 wins!");
            winningPlayer = 2;
            EndGame();
        }
    }

    //Called when time runs out
    public void CheckWhoHasMostPoints()
    {
        if (player1Score > player2Score)
        {
            winningPlayer = 1;
            Debug.Log("Player 1 wins!");
        }
        else if (player2Score > player1Score)
        {
            winningPlayer = 2;
            Debug.Log("Player 2 wins!");
        }
        else
        {
            winningPlayer = 0;
            Debug.Log("It's a tie!");
        }
        EndGame();
    }

    private void ResetScores()
    {
        player1Score = 0;
        player2Score = 0;
        winningPlayer = 0;
    }

    private void EndGame()
    {
        if (audioSource != null && !audioSource.isPlaying){
            audioSource.Play();
        }
        OnGameEnded?.Invoke(winningPlayer);

        if (winningPlayer == 1)
        {
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

        ResetScores();
        UpdateScoreUI();


    }
}
