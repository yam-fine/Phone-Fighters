using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class UnLinkedLogin : MonoBehaviour {

    [SerializeField]
    string titleId = "9293";
    [SerializeField]
    GameObject setActiveOnLogin;
    //[SerializeField]
    //string mainMenuSceneName;

    void Start ()
    {
        Login();
	}

    void Login()
    {
        var request = new LoginWithAndroidDeviceIDRequest
        {
            CreateAccount = true,
            AndroidDeviceId = AndroidDeviceID(),
            AndroidDevice = SystemInfo.deviceModel,
            TitleId = titleId
        };

        PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        //GetComponent<LoadingScreen>().LoadScene(mainMenuSceneName);
        setActiveOnLogin.SetActive(true);
        Debug.Log(result);
    }

    private void OnLoginFailure(PlayFabError error)
    {
        //errors.text = error.GenerateErrorReport();
        //errors.color = UnityEngine.Color.red;
    }

    string AndroidDeviceID()
    {
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass secure = new AndroidJavaClass("android.provider.Settings$Secure");
        return secure.CallStatic<string>("getString", contentResolver, "android_id");
    }
}
