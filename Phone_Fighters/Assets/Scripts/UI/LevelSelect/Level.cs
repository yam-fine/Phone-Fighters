using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour {

    [SerializeField]
    int progressNeeded;
    public int ProgressNeeded { get { return progressNeeded; } }
    [SerializeField]
    Sprite unlockedImage;

    public void OpenDoor()
    {
        GetComponent<Image>().sprite = unlockedImage;
        GetComponent<Button>().interactable = true;
    }
}
