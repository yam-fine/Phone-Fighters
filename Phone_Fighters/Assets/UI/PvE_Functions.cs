using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PvE_Functions : MonoBehaviour
{
    [SerializeField]
    Canvas menu;
    [SerializeField]
    Button resumeButton;

    void Start()
    {
        menu = menu.GetComponent<Canvas>();
        resumeButton = resumeButton.GetComponent<Button>();
        menu.enabled = false;   
    }

    public void Menu()
    {
        menu.enabled = true;
        Time.timeScale = 0;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetSceneAt(0).path);
    }

    public void Resume()
    {
        menu.enabled = false;
        Time.timeScale = 1;
    }

}
