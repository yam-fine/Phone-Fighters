using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour {

    [SerializeField]
    string[] boolParameterNames;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        foreach (string param in boolParameterNames)
        {
            animator.SetBool(param, false);
        }

        animator.GetComponent<Character>().Attack = true;

        if (GameManager.Instance.IsPvp)
        {
            if (animator.tag == "team1")
            {
                Player.Instance.MyRigidbody.velocity = Vector2.zero;
            }
            else if (animator.tag == "team2")
            {
                Player.Instance.MyRigidbody.velocity = Vector2.zero;
            }
            else
                Debug.LogWarning("unknown tag!");
        }
        else
        {
            if (animator.tag == "Player")
            {
                //Player.Instance.MyRigidbody.velocity = Vector2.zero;
            }
            else if (animator.tag == "Enemy")
            {
                animator.SetFloat("Run", 0);
            }
            else
                Debug.LogWarning("unknown tag!");
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	    if (animator.tag != "Enemy") // only for basic attack
        {
            Player.OnResetAttack += (() => animator.SetBool("ContinueCombo", true));
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<Character>().Attack = false;
        //animator.SetBool("Ranged", false);
        Player.OnResetAttack -= (() => animator.SetBool("ContinueCombo", true));
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
