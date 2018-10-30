using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour {

    [SerializeField]
    Text itemName;
    [SerializeField]
    Text price;
    [SerializeField]
    Text GP, RM;
    [SerializeField]
    Image image;

    StoreItem item;
    StoreScrollList scrollList;
    int priceInt;

    private void Awake()
    {
        scrollList = new StoreScrollList();
    }

    void Start ()
    {
        GetComponent<Button>().onClick.AddListener(() => scrollList.Confirmation(itemName.text, priceInt));
    }

    public void Setup(StoreItem currentItem, StoreScrollList currentScrollList)
    {
        item = currentItem;
        itemName.text = item.itemName;
        image.sprite = item.icon;
        priceInt = item.priceGP;
        price.text = priceInt.ToString();
        scrollList = currentScrollList;
    }
}
