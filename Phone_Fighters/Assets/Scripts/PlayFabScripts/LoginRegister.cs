using UnityEngine;

public class LoginRegister : MonoBehaviour {

    [SerializeField]
    Canvas loginCanvas = null, registerCanvas = null;
    
	public void Login()
    {
        loginCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Register()
    {
        registerCanvas.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
