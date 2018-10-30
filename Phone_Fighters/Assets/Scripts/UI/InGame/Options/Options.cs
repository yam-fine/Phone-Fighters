using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour {

    [SerializeField]
    GameObject optionsMenu;

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
    }
}
