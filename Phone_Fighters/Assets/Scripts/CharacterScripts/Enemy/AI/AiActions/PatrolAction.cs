using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Patrol")]
public class PatrolAction : AiAction
{

    public override void Act(StateController controller)
    {
        Patrol(controller);
        Debug.Log("Patrollin");
    }

    private void Patrol(StateController controller)
    {
        Enemy enemy = controller.enemy;

        if (controller.CheckIfCountDownElapsed(enemy.PatrolTime))
        {
            enemy.PatrolTime = Random.Range(1, 10) + controller.stateTimeElapsed;
            enemy.GoRight = Random.value > 0.5f;
            enemy.DoIdle = Random.value > 0.5f;
        }

        enemy.Patrol();
    }
}