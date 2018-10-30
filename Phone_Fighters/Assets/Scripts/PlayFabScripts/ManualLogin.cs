using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class ManualLogin : MonoBehaviour
{

    [SerializeField]
    string titleId = "9293";
    [SerializeField]
    string username = "yaml";
    [SerializeField]
    string password = "123123";
    [SerializeField]
    GameObject setActiveOnLogin;
    //[SerializeField]
    //string mainMenuSceneName;
    [SerializeField]
    bool fromLoadingScreen = false;

    public void Start()
    {
        Login();
    }

    public void Login()
    {

        var request = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password,
            TitleId = titleId
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        setActiveOnLogin.SetActive(true);

        //GetComponent<LoadingScreen>().LoadScene(mainMenuSceneName);
        if (!fromLoadingScreen)
        {
            gameObject.SetActive(false);
        }
        Debug.Log(result);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.Log(error);
    }
}
