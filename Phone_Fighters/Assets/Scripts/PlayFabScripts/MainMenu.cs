using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {


    [SerializeField]
    private FadeImage blackScreenCover;
    [SerializeField]
    TMP_Text username;
    [SerializeField]
    TMP_Text reviveNum;
    [SerializeField]
    TMP_Text timeTillRevive;
    [SerializeField]
    TMP_Text timeTillReviveNum;

    [SerializeField]
    Canvas store;
    string catalogName = "main";
    string storeName = "Consumables Store";
    List<PlayFab.ClientModels.StoreItem> storeItems = new List<PlayFab.ClientModels.StoreItem>();
    public List<PlayFab.ClientModels.StoreItem> StoreItems { get { return storeItems; }}
    [SerializeField]
    StoreScrollList storeScrollList;
    [SerializeField]
    GameObject playFabAuth;
    Item item;

    int maxRevives;
    int revives;
    int reviveTimer;
    VirtualCurrencyRechargeTime timeTillRev;
    DateTime nextFreeRevive = new DateTime();
    int coinsNumreq = 0;
    int gemsNumreq;
    int finishedLoadingCount = 0;

    bool offlineMode;
    string map;
    WaitForSeconds wait = new WaitForSeconds(0.1f);
    AsyncOperation ayo;

    void Awake()
    {
        UserInvRequest();
        GetAccountInfo();
        if (FindObjectsOfType<LoadingScreen>().Length > 1)
            StartCoroutine(FinishedLoading());
    }

    void Update()
    {
        if (maxRevives != revives)
        {
            if (nextFreeRevive.Subtract(DateTime.Now).TotalSeconds <= 0)
            {
                timeTillReviveNum.text = "Fetching timer...";
                UserInvRequest();
            }
            else
            {
                timeTillReviveNum.text = (Mathf.RoundToInt((float)nextFreeRevive.Subtract(DateTime.Now).TotalSeconds)).ToString();
                reviveNum.text = revives.ToString();
            }
        }
        else if (maxRevives != 0)
        {
            timeTillRevive.enabled = false;
            timeTillReviveNum.enabled = false;
            reviveNum.text = revives.ToString();
        }
    }

    IEnumerator FinishedLoading ()
    {
        while (!(finishedLoadingCount == 2))
            yield return null;
        yield return null;
        StartCoroutine(FindObjectOfType<LoadingScreen>().UnloadScene());
    }

    public void GetAccountInfo()
    {
        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest { }, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    // ACCOUNT INFO
    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        gameObject.SetActive(true);
        username.text = result.AccountInfo.Username;
        Debug.Log("got account");
        finishedLoadingCount++;
    }
    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    // USER INVENTORY
    private void OnGetInventorySuccess(GetUserInventoryResult result)
    {
        result.VirtualCurrency.TryGetValue("RV", out revives);
        result.VirtualCurrencyRechargeTimes.TryGetValue("RV", out timeTillRev);
        maxRevives = timeTillRev.RechargeMax;
        result.VirtualCurrency.TryGetValue("GP", out coinsNumreq);
        //result.VirtualCurrency.TryGetValue("RM", out gemsNumreq);

        //coins.text = coinsNumreq.ToString();
        //gems.text = gemsNumreq.ToString();

        if (timeTillRev != null)
        {
            nextFreeRevive = DateTime.Now.AddSeconds(timeTillRev.SecondsToRecharge);
        }

        Debug.Log("put loading gif here");

        finishedLoadingCount++;
    }
    private void OnGetInventoryError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void UserInvRequest()
    {
        var userInvRequest = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInvRequest, OnGetInventorySuccess, OnGetInventoryError);
    }

    void GetCatalogStore(string storeName)
    {
        var request = new GetStoreItemsRequest
        {
            CatalogVersion = catalogName,
            StoreId = storeName
        };
        PlayFabClientAPI.GetStoreItems(request, GetStoreItemsSuccess, GetStoreItemsFailure);
    }

    void GetStoreItemsSuccess(GetStoreItemsResult result)
    {
        storeItems = result.Store;
        if (!store.gameObject.activeInHierarchy)
        {
            ToStore();
        }
        if (!storeScrollList.Temp)
        {
            storeScrollList.RefreshDisplay();
        }
    }
    void GetStoreItemsFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    void ToStore()
    {
        store.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ExitStore()
    {
        store.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }

    public void OpenStore()
    {
        GetCatalogStore(storeName);
    }

    public void ConsumablesStore()
    {
        storeName = "Consumables Store";
        GetCatalogStore(storeName);
        storeScrollList.StoreChange(storeName);
    }

    public void CharacterStore()
    {
        storeName = "Character Store";
        GetCatalogStore(storeName);
        storeScrollList.StoreChange(storeName);
    }

    public void LoadPvP()
    {
        offlineMode = false;
        map = "PvP";
        PlayFabAuthenticator.OnAuthenticated += LoadLevel;
        if (!playFabAuth.activeInHierarchy)
            playFabAuth.SetActive(true);
    }

    public void PveButton(string scene)
    {
        GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
        ayo = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        WaitForSceneLoad(ayo);
    }

    public void UnloadScene()
    {
        StartCoroutine(FindObjectOfType<LoadingScreen>().UnloadScene());
    }

    void LoadLevel()
    {
        PhotonNetwork.offlineMode = offlineMode;
        PhotonNetwork.CreateRoom("");
        PhotonNetwork.LoadLevel(map);
    }

    IEnumerator WaitForSceneLoad(AsyncOperation async)
    {
        while (!async.isDone)
        {
            yield return wait;
        }
        StartCoroutine(FindObjectOfType<LoadingScreen>().UnloadScene());
    }
}
