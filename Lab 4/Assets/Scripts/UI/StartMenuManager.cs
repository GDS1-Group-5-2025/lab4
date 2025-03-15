using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour
{

    public void ClickSPS(){
        SceneManager.LoadScene("SinglePlayerSelect");
    }

    public void ClickMP(){
        SceneManager.LoadScene("PvP");
    }
}
