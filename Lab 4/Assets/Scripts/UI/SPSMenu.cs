using UnityEngine;
using UnityEngine.SceneManagement;

public class SPSMenu : MonoBehaviour
{
    public void ClickPVE(){
        SceneManager.LoadScene("BotLevelSelect");
    }

    public void ClickPVT(){
        SceneManager.LoadScene("PvT");
    }

    public void ClickPVE1()
    {
        SceneManager.LoadScene("PvE");
    }

    public void ClickPVE2()
    {
        SceneManager.LoadScene("PvE2");
    }

    public void ClickPVE3()
    {
        SceneManager.LoadScene("PvE3");
    }




}
