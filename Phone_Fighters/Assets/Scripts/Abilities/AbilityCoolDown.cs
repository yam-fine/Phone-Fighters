using UnityEngine;
using UnityEngine.UI;

public class AbilityCoolDown : MonoBehaviour
{

    //public string abilityButtonAxisName = "Fire1";
    Image darkMask;
    //public Text coolDownTextDisplay;

    Ability ability;
    Image myButtonImage;
    //AudioSource sound;
    float coolDownDuration;
    float nextReadyTime;
    float coolDownTimeLeft;
    Button button;
    Player player;
    //string abilityName = null;
    string triggerName;
    InputManager inputManager;

    void Awake()
    {
        player = NetworkManager.Instance.MyPlayer.GetComponent<Player>();
        inputManager = player.gameObject.GetComponent<InputManager>();
        button = GetComponent<Button>();
    }

    public void Initialize(Ability selectedAbility, int abilityNum) // called from "SetAbilities" script
    {
        GetComponent<Button>().onClick.AddListener(ButtonTriggered);

        ability = selectedAbility;
        //abilityName = ability.name;
        triggerName = ability.AnimatorParamName;
        myButtonImage = GetComponent<Image>();
        //sound = GetComponent<AudioSource>();        
        myButtonImage.sprite = ability.Icon;
        darkMask = GetComponentsInChildren<Image>()[1];
        darkMask.sprite = ability.Icon;
        coolDownDuration = ability.CoolDown;

        ability.Init();
        AbilityReady();
    }

    // Update is called once per frame
    void Update()
    {
        bool coolDownComplete = (Time.time > nextReadyTime);
        if (coolDownComplete)
        {
            AbilityReady();
            //if (Input.GetButtonDown(abilityButtonAxisName))
            //{
            //    ButtonTriggered();
            //}            
        }
        else
        {
            CoolDown();
        }
    }

    private void AbilityReady()
    {
        button.interactable = true;

        //coolDownTextDisplay.enabled = false;
        darkMask.enabled = false;
    }

    private void CoolDown()
    {
        button.interactable = false;

        coolDownTimeLeft -= Time.deltaTime;
        //float roundedCd = Mathf.Round(coolDownTimeLeft);
        //coolDownTextDisplay.text = roundedCd.ToString();
        darkMask.fillAmount = (coolDownTimeLeft / coolDownDuration);
    }

    private void ButtonTriggered()
    {
        if (!player.IsDead && !player.Immortal && !player.TakingDamage && !inputManager.IsDashing)
        {
            nextReadyTime = coolDownDuration + Time.time;
            coolDownTimeLeft = coolDownDuration;
            darkMask.enabled = true;
            //coolDownTextDisplay.enabled = true;
            player.AbilitySound = ability.Sound;
            player.MyAnimator.SetBool(triggerName, true);
        }
    }

    public void ActivateAbility() // called from Player
    {
        ability.TriggerAbility(player.transform);
        player.MyAnimator.SetBool(triggerName, false);
    }
}