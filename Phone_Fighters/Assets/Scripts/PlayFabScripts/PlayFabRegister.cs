using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayFabRegister : MonoBehaviour {

    [SerializeField]
    string titleId;
    [SerializeField]
    TMP_InputField username;
    [SerializeField]
    TMP_InputField password;
    [SerializeField]
    TMP_InputField repeatPassword;
    [SerializeField]
    TMP_InputField email;
    [SerializeField]
    TMP_Text errors;
    [SerializeField]
    Canvas mainMenu;
    [SerializeField]
    GameObject loginRegister;

    public void OK()
    {
        if(password.text != repeatPassword.text)
        {
            errors.color = UnityEngine.Color.red;
            errors.text = "passwords don't match";
            throw new Exception("passwords don't match");
        }

        var request = new RegisterPlayFabUserRequest
        {
            Username = username.text,
            Password = password.text,
            Email = email.text,
            RequireBothUsernameAndEmail = true,
            TitleId = titleId
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(RegisterPlayFabUserResult result)
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
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
