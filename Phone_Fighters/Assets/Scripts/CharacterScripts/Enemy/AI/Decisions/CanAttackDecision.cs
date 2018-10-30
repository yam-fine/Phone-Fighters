using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/CanAttack")]
public class CanAttackDecision : Decision {

    public override bool Decide(StateController controller)
    {
        return CanAttack(controller);
    }

    bool CanAttack(StateController controller)
    {
        if (controller.enemy.InRange && controller.CheckIfCountDownElapsed(controller.enemy.AttackRate) /*&&*/
            /*IsInRange(controller)*/)
        {
            controller.enemy.MyAnimator.SetFloat("Run", 0);
            return true;
        }
        return false;
    }

    //bool IsInRange(StateController controller)
    //{
    //    Vector2 myPos = controller.transform.position;
    //    Vector2 targetPos = controller.enemy.MyTarget.transform.position;
    //    // in vertical range
    //    if (myPos.y <= targetPos.y + 0.5 ||
    //        myPos.y >= targetPos.y - 0.5)
    //    {
    //        // in horizontal range
    //        if (targetPos.x <= myPos.x + controller.enemy.StoppingDistance ||
    //            targetPos.x >= myPos.x - controller.enemy.StoppingDistance)
    //        {
    //            controller.enemy.InRange = true;
    //            return true;
    //        }
    //    }
    //    controller.enemy.InRange = false;
    //    return false;
    //}
}