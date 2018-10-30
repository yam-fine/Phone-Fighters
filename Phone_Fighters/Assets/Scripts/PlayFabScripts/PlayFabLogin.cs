using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using TMPro;

public class PlayFabLogin : MonoBehaviour {

    [SerializeField]
    string titleId;
    [SerializeField]
    TMP_InputField username;
    [SerializeField]
    TMP_InputField password;
    [SerializeField]
    TMP_Text errors;
    [SerializeField]
    MainMenu mainMenu;
    [SerializeField]
    GameObject loginRegister;

    public void Login () {

        var request = new LoginWithPlayFabRequest
        {
            Username = username.text,
            Password = password.text,
            TitleId = titleId
        };

        PlayFabClientAPI.LoginWithPlayFab(request, OnLoginSuccess, OnLoginFailure);
	}

    private void OnLoginSuccess(LoginResult result)
    {
        mainMenu.GetAccountInfo();
        Debug.Log(result);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        errors.text = error.GenerateErrorReport();
        errors.color = UnityEngine.Color.red;
    }

    public void GoBack()
    {
        loginRegister.SetActive(true);
        gameObject.SetActive(false);
    }
}
