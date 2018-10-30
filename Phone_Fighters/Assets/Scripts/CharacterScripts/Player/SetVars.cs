using UnityEngine;
using UnityEngine.UI;

public class SetVars : MonoBehaviour {

    Player player;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    [PunRPC]
    void Set_Team(string team)
    {
        gameObject.tag = team;
    }

    [PunRPC]
    void Set_Tags()
    {
        if (gameObject.tag == "team1")
        {
            player.TakeDamageSourcesTags.Clear();
            player.TakeDamageSourcesTags.Add("EnemyDmgSource");
        }
        else if (gameObject.tag == "team2")
        {
            player.TakeDamageSourcesTags.Clear();
            player.TakeDamageSourcesTags.Add("DmgSource");
        }
    }

    [PunRPC]
    void Set_UI()
    {
        if (gameObject.tag == "team1")
        {
            foreach (GameObject ui in GameObject.FindGameObjectsWithTag("UI"))
            {
                if (ui.name == "L_HealthBarBG")
                {
                    player.HealthBar = ui.GetComponent<BarHandler>();
                }
                else if (ui.name == "L_StaminaBarBG")
                {
                    player.StaminaBar = ui.GetComponent<BarHandler>();
                }
                /*else if (ui.name == "X")
                {
                    Button X = ui.GetComponent<Button>();
                    X.onClick.AddListener(player.Melee);
                }
                else if (ui.name == "Z")
                {
                    Button Z = ui.GetComponent<Button>();
                    Z.onClick.AddListener(GetComponent<InputManager>().Dash);
                }*/
            }
        }
        else if (gameObject.tag == "team2")
        {
            foreach (GameObject ui in GameObject.FindGameObjectsWithTag("UI"))
            {
                if (ui.name == "R_HealthBarBG")
                {
                    player.HealthBar = ui.GetComponent<BarHandler>();
                }
                else if (ui.name == "R_StaminaBarBG")
                {
                    player.StaminaBar = ui.GetComponent<BarHandler>();
                }
                /*else if (ui.name == "X")
                {
                    Button X = ui.GetComponent<Button>();
                    X.onClick.AddListener(player.Melee);
                }
                else if (ui.name == "Z")
                {
                    Button Z = ui.GetComponent<Button>();
                    Z.onClick.AddListener(GetComponent<InputManager>().Dash);
                }*/
            }
        }
        else // PvE
        {            
            foreach (GameObject ui in GameObject.FindGameObjectsWithTag("UI"))
            {
                if (ui.name == "HealthBarBG")
                {
                    player.HealthBar = ui.GetComponent<BarHandler>();
                }
                else if (ui.name == "StaminaBarBG") 
                {
                    player.StaminaBar = ui.GetComponent<BarHandler>();
                }
                /*else if (ui.name == "X")
                {
                    Button X = ui.GetComponent<Button>();
                    X.onClick.AddListener(player.Melee);
                }
                else if (ui.name == "Z")
                {
                    Button Z = ui.GetComponent<Button>();
                    Z.onClick.AddListener(GetComponent<InputManager>().Dash);
                }*/
            }
        }
    }
}
