using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour {

    public void ReturnToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Resume()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }
}
