using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/HasTarget")]
public class HasTargetDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        bool targetVisible = Look(controller);
        return targetVisible;
    }

    private bool Look(StateController controller)
    {
        if (controller.enemy.MyTarget)
        {
            controller.chaseTarget = controller.enemy.MyTarget.transform;
            return true;
        }            
        return false;
    }
}