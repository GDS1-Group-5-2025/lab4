using UnityEngine;
using UnityEngine.SceneManagement;

public class SPSMenu : MonoBehaviour
{
    public void ClickPVE(){
        SceneManager.LoadScene("PvE");
    }

    public void ClickPVT(){
        SceneManager.LoadScene("PvT");
    }
}
