using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public float timer = 0f;
  
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 99f)
        {
            ScoreManager.Instance.CheckWhoHasMostPoints();
        }
    }
}
