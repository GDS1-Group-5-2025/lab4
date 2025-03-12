using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the ScoreManager's game over event.
        ScoreManager.Instance.OnGameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks or unexpected behavior.
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnGameEnded -= HandleGameEnded;
    }

    //Game Ends
    private void HandleGameEnded(int winningPlayer)
    {
        Debug.Log($"Game Over! Winning player: {winningPlayer}. Reloading scene...");
        ResetScene();
    }

    //Reloads scene
    private void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}