using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabAuthenticator : MonoBehaviour
{

    private string _playFabPlayerIdCache;
    public delegate void AuthenticatedAction();
    public static event AuthenticatedAction OnAuthenticated;
    [SerializeField]
    bool test = false;
    [SerializeField]
    GameObject networkManager;

    //Run the entire thing on awake
    public void Awake()
    {
        if (!test)
            OnAuthenticated += () => FindObjectOfType<LoadingScreen>().LoadScene("MainMenu");
        AuthenticateWithPlayFab();

        if (test)
        {
            OnAuthenticated += (() =>
            {
                if (networkManager != null)
                    networkManager.SetActive(true);
                else
                    Debug.Log("no network manager was given");
                gameObject.SetActive(false);
            }
            );
        }
    }

    private void AuthenticateWithPlayFab()
    {
        LogMessage("PlayFab authenticating...");

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { }, RequestPhotonToken, OnPlayFabError);
    }

    /*
    * Step 2
    * We request Photon authentication token from PlayFab.
    * This is a crucial step, because Photon uses different authentication tokens
    * than PlayFab. Thus, you cannot directly use PlayFab SessionTicket and
    * you need to explicitely request a token. This API call requires you to 
    * pass Photon App ID. App ID may be hardcoded, but, in this example,
    * We are accessing it using convenient static field on PhotonNetwork class
    * We pass in AuthenticateWithPhoton as a callback to be our next step, if 
    * we have acquired token succesfully
    */
    private void RequestPhotonToken(GetAccountInfoResult obj)
    {
        LogMessage("PlayFab authenticated. Requesting photon token...");

        //We can player PlayFabId. This will come in handy during next step
        _playFabPlayerIdCache = obj.AccountInfo.PlayFabId;

        PlayFabClientAPI.GetPhotonAuthenticationToken(new GetPhotonAuthenticationTokenRequest()
        {
            PhotonApplicationId = PhotonNetwork.PhotonServerSettings.AppID
        }, AuthenticateWithPhoton, OnPlayFabError);
    }

    /*
     * Step 3
     * This is the final and the simplest step. We create new AuthenticationValues instance.
     * This class describes how to authenticate players inside Photon environment.
     */
    private void AuthenticateWithPhoton(GetPhotonAuthenticationTokenResult obj)
    {
        LogMessage("Photon token acquired: " + obj.PhotonCustomAuthenticationToken + "  Authentication complete.");

        //We set AuthType to custom, meaning we bring our own, PlayFab authentication procedure.
        var customAuth = new AuthenticationValues { AuthType = CustomAuthenticationType.Custom };

        //We add "username" parameter. Do not let it confuse you: PlayFab is expecting this parameter to contain player PlayFab ID (!) and not username.
        customAuth.AddAuthParameter("username", _playFabPlayerIdCache);    // expected by PlayFab custom auth service

        //We add "token" parameter. PlayFab expects it to contain Photon Authentication Token issues to your during previous step.
        customAuth.AddAuthParameter("token", obj.PhotonCustomAuthenticationToken);

        //We finally tell Photon to use this authentication parameters throughout the entire application.
        PhotonNetwork.AuthValues = customAuth;

        if(OnAuthenticated != null)
        OnAuthenticated();
    }

    private void OnPlayFabError(PlayFabError obj)
    {
        LogMessage(obj.GenerateErrorReport());
    }

    public void LogMessage(string message)
    {
        Debug.Log("PlayFab + Photon Example: " + message);        
    }
}