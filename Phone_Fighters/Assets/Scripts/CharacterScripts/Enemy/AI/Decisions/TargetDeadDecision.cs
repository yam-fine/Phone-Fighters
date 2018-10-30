using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/TargetDead")]
public class TargetDeadDecision : Decision
{
    public override bool Decide(StateController controller)
    {
        bool chaseTargetIsActive = controller.chaseTarget.GetComponent<Character>().IsDead;
        return chaseTargetIsActive;
    }
}