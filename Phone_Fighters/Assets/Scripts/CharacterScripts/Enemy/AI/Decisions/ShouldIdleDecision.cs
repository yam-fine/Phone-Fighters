using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/ShouldIdle")]
public class ShouldIdleDecision : Decision
{

    public override bool Decide(StateController controller)
    {
        return controller.enemy.DoIdle;
    }
}
