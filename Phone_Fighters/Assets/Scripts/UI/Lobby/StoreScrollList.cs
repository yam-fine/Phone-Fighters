using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

[System.Serializable]
public class StoreItem
{
    public string itemName;
    public Sprite icon;
    public int priceGP = 1;
    public int priceRM = 0;
}

public class StoreScrollList : MonoBehaviour
{
    [SerializeField]
    MainMenu mainMenu;
    [SerializeField]
    Transform contentPanel;
    [SerializeField]
    SimpleObjectPool buttonObjectPool;
    [SerializeField]
    TMP_Text gpText;
    List<PlayFab.ClientModels.StoreItem> storeItems;
    bool temp = true;
    public bool Temp { get { return temp; } }
    int gp;
    public int GP { get { return gp; } }
    uint tempGP;

    [SerializeField]
    string catalogVersion = "main";
    string store;
    [SerializeField]
    GameObject confirmPanel;
    string itemName;
    int priceGp;

    void Awake()
    {
        store = "Consumables Store";
    }

    void Start()
    {
        RefreshDisplay();
        UserInvRequest();
    }

    public void RefreshDisplay()
    {
        storeItems = mainMenu.StoreItems;
        RemoveButtons();
        AddButtons();
        temp = false;
    }

    private void RemoveButtons()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(toRemove);
        }
    }

    private void AddButtons()
    {
        for (int i = 0; i < storeItems.Count; i++)
        {
            StoreItem item = new StoreItem();
            item.itemName = storeItems[i].ItemId;
            if (storeItems[i].VirtualCurrencyPrices.ContainsKey("GP"))
            {
                storeItems[i].VirtualCurrencyPrices.TryGetValue("GP", out tempGP);
                item.priceGP = (int)tempGP;
            }
            
            GameObject newButton = buttonObjectPool.GetObject();
            newButton.transform.SetParent(contentPanel, false);
            Item sampleButton = newButton.GetComponent<Item>();
            sampleButton.Setup(item, this);
        }
    }

    void UserInvRequest()
    {
        var userInvRequest = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(userInvRequest, OnGetInventorySuccess, OnGetInventoryError);
    }

    void OnGetInventorySuccess(GetUserInventoryResult result)
    {
        result.VirtualCurrency.TryGetValue("GP", out gp);
        gpText.text = gp.ToString();
    }
    void OnGetInventoryError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void ConfirmPurchase()
    {
        PurchaseRequest(catalogVersion, itemName, priceGp, store, "GP");
    }
    public void DeclinePurchase()
    {
        confirmPanel.SetActive(false);
    }

    public void PurchaseRequest(string catalogVersion, string item, int price, string storeID, string virtualCurrency)
    {
        var purchaseItemRequest = new PurchaseItemRequest
        {
            CatalogVersion = catalogVersion,
            ItemId = item,
            Price = price,
            StoreId = storeID,
            VirtualCurrency = virtualCurrency
        };
        PlayFabClientAPI.PurchaseItem(purchaseItemRequest, PurchaseSuccess, PurchaseError);
    }

    void PurchaseSuccess(PurchaseItemResult result)
    {
        Debug.Log("purchase success!");
        UserInvRequest();
        if (confirmPanel.activeInHierarchy)
        {
            confirmPanel.SetActive(false);
        }
    }
    void PurchaseError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }

    public void StoreChange(string storeName)
    {
        store = storeName;
    }

    public void Confirmation(string name, int price)
    {
        confirmPanel.SetActive(true);
        itemName = name;
        priceGp = price;
        Debug.Log("make all other buttons unpressable");
    }

    public void GoBack()
    {
        mainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}