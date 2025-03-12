using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timer = 99f;
    public TextMeshProUGUI timerText;
  
    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = Mathf.FloorToInt(timer).ToString();

        if (timer <= 0f)
        {
            ScoreManager.Instance.CheckWhoHasMostPoints();
        }
    }
}
