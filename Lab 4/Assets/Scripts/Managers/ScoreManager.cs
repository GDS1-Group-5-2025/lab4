using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [SerializeField] private int player1Score = 0;
    [SerializeField] private int player2Score = 0;
    [SerializeField] private int scoreToWin = 7;

    private int winningPlayer;

    private void Awake()
    {
        // If there is already an instance and it’s not this one, destroy this duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Otherwise, set this as the active instance
        Instance = this;
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

        CheckWinConditionFirstToReachScore();
    }

    private void CheckWinConditionFirstToReachScore()
    {
        if (player1Score == scoreToWin)
        {
            Debug.Log("Player 1 wins!");
            winningPlayer = 1;
            //End Game
        }
        else if (player2Score == scoreToWin)
        {
            Debug.Log("Player 2 wins!");
            winningPlayer = 2;
            //End Game
        }
    }

    //Called when time runs out
    private void CheckWhoHasMostPoints()
    {
        if (player1Score > player2Score)
        {
            Debug.Log("Player 1 wins!");
            winningPlayer = 1;
            //End Game
        }
        else if (player2Score > player1Score)
        {
            Debug.Log("Player 2 wins!");
            winningPlayer = 2;
            //End Game
        }
        else
        {
            Debug.Log("It's a tie!");
            winningPlayer = 0;
            //End Game
        }
    }
}
