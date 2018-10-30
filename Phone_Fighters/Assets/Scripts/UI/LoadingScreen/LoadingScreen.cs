using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

    [SerializeField]
    private FadeImage blackScreenCover;
    [SerializeField]
    GameObject[] destroyMeSenpai;

    public void LoadScene(string sceneName)
    {
        // Load level async
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    public IEnumerator UnloadScene()
    {
        Debug.Log("unloading");
        // Fade to black
        yield return StartCoroutine(blackScreenCover.FadeIn());

        // Hide loading screen        
        foreach (GameObject i in destroyMeSenpai)
        {
            Destroy(i);
        }

        //Fade to new screen and destroy this object
        yield return StartCoroutine(blackScreenCover.FadeOut(gameObject));
    }
}
