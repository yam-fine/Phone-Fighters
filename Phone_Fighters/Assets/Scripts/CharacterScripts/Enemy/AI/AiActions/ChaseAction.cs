using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : AiAction {

    [Tooltip("distance from target in which the enemy starts targeting the target")]
    [SerializeField]
    float closeHorizontalDistance = 20;
    [SerializeField]
    float chaseDelay = 0.5f;
    float closeGapDelta = 5;

    public override void Act(StateController controller)
    {
        Chase(controller);
        Debug.Log("Chasing");
    }

    void Chase(StateController controller)
    {
        if (controller.CheckIfCountDownElapsed(chaseDelay))
        {
            Vector3 pos = controller.transform.position;
            Enemy enemy = controller.enemy;
            Vector3 targetPos = controller.chaseTarget.position;
            Vector3 destination;

            if (Mathf.Abs(pos.x - targetPos.x) > closeHorizontalDistance) //far from target
            {
                if (pos.x - targetPos.x < 0)
                    destination = new Vector3(pos.x + closeGapDelta, targetPos.y);
                else
                    destination = new Vector3(pos.x - closeGapDelta, targetPos.y);
            }
            else                                                          //close to target
            {
                if (!enemy.InRange)
                {
                    if (pos.x == targetPos.x && pos.y == targetPos.y)
                    {
                        return;
                    }
                    else if (pos.x > targetPos.x)
                    {
                        destination = new Vector3(targetPos.x /*+ enemy.StoppingDistance*/, targetPos.y);
                    }
                    else
                    {
                        destination = new Vector3(targetPos.x /*- enemy.StoppingDistance*/, targetPos.y);
                    }
                }
                else
                {
                    //destination = new Vector3(pos.x, targetPos.y);
                    return;
                }
            }

            enemy.Move(destination, enemy.Speed * enemy.ChaseSpeedMultiplier * Time.deltaTime);
        }
    }
}
