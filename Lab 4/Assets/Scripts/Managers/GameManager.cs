using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the ScoreManager's game over event.
        ScoreManager.Instance.OnGameEnded += HandleGameEnded;
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
            ScoreManager.Instance.OnGameEnded -= HandleGameEnded;
    }

    // Called when the game ends
    private void HandleGameEnded(int winningPlayer)
    {
        Debug.Log($"Game Over! Winning player: {winningPlayer}. Reloading scene in 3 seconds...");
        StartCoroutine(DelayedReset());
    }

    // Coroutine that waits 3 seconds before reloading the scene
    private IEnumerator DelayedReset()
    {
        yield return new WaitForSeconds(3f);
        ResetScene();
    }

    // Reloads the current scene
    private void ResetScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }
}