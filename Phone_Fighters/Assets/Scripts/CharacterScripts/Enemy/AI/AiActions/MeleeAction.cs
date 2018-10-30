using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/MeleeAttack")]
public class MeleeAction : AiAction {

    public override void Act(StateController controller)
    {
        Attack(controller.enemy);
        Debug.Log("Melee action");
    }

    void Attack(Enemy enemy)
    {
        enemy.MeleeAbility();
    }
}
