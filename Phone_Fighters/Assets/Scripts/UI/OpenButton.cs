using UnityEngine;

public class OpenButton : MonoBehaviour {

    [SerializeField]
    GameObject enableOnClick;
    [SerializeField]
    GameObject disableOnClick;

    public void OnClick()
    {
        if (enableOnClick)
            enableOnClick.SetActive(true);
        if (disableOnClick)
            disableOnClick.SetActive(false);
    }
}
