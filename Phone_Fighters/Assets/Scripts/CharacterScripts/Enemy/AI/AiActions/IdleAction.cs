using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Idle")]
public class IdleAction : AiAction {

    public override void Act(StateController controller)
    {
        Idle(controller);
        Debug.Log("Idle");
    }

    void Idle(StateController controller)
    {
        Enemy enemy = controller.enemy;

        if (controller.CheckIfCountDownElapsed(enemy.PatrolTime))
        {
            enemy.PatrolTime = Random.Range(1, 10);
            enemy.DoIdle = false;
        }
    }
}
