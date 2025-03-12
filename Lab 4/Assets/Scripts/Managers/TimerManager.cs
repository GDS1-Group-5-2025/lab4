using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    public float timer = 99f;
    public TextMeshProUGUI timerText;
  
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();
        }
        else 
        {
            ScoreManager.Instance.CheckWhoHasMostPoints();
        }


    }
}
