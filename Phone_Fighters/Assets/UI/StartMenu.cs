using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenu : MonoBehaviour {

    [SerializeField]
    Canvas quitMenu;
    [SerializeField]
    Canvas creditMenu;
    [SerializeField]
    Button playButton;
    [SerializeField]
    Button exitButton;

    // Use this for initialization
    void Start ()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        creditMenu = creditMenu.GetComponent<Canvas>();
        playButton = playButton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
        quitMenu.enabled = false;
        creditMenu.enabled = false;
    }

    public void ExitPress()
    {
        quitMenu.enabled = true;
        playButton.enabled = false;
        exitButton.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        playButton.enabled = true;
        exitButton.enabled = true;
    }

    public void PlayPress()
    {
        SceneManager.LoadScene(1);
    }

    public void YesPress()
    {
        Application.Quit();
    }

    public void CreditPress()
    {
        creditMenu.enabled = true;
        playButton.enabled = false;
        exitButton.enabled = false;
    }

    public void CreditCoolPress()
    {
        creditMenu.enabled = false;
        playButton.enabled = true;
        exitButton.enabled = true;
    }
}
