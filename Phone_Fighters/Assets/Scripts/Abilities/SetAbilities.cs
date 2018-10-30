using UnityEngine;
using UnityEngine.UI;

public class SetAbilities : MonoBehaviour {

    AbilityCoolDown[] coolDownButtons;
    GameObject player;

    void Start()
    {
        coolDownButtons = GetComponentsInChildren<AbilityCoolDown>();
        player = NetworkManager.Instance.MyPlayer;

        CharacterSelect(player.GetComponent<Player>()); // set character's abilities

        transform.Find("Dash").GetComponent<Button>().onClick.AddListener(player.GetComponent<InputManager>().Dash);        
    }

    void CharacterSelect(Player player)
    {
        for(int i = 0; i < coolDownButtons.Length; i++)
        {
            coolDownButtons[i].Initialize(player.Abilities[i], i);
            switch (i)
            {
                case 0:
                    player.OnAbility0 += (() => coolDownButtons[0].ActivateAbility());
                    break;
                case 1:
                    player.OnAbility1 += (() => coolDownButtons[1].ActivateAbility());
                    break;
                case 2:
                    player.OnAbility2 += (() => coolDownButtons[2].ActivateAbility());
                    break;
                case 3:
                    player.OnAbility3 += (() => coolDownButtons[3].ActivateAbility());
                    break;
            }
        }
    }
}
