using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/RangedAttack")]
public class RangedAction : AiAction {

    public override void Act(StateController controller)
    {
        Attack(controller.enemy);
        Debug.Log("Ranged action");
    }

    void Attack(Enemy enemy)
    {
        enemy.RangedAbility();
    }
}
