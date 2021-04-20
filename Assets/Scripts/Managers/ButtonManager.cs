using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonManager : MonoBehaviour
{
    public void OnRestart()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPlayGame()
    {
        SceneManager.LoadScene(1);
    }
}
