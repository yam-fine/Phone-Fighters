using UnityEngine;

public class StateController : MonoBehaviour
{

    public State currentState;
    [HideInInspector]
    public Enemy enemy;
    public State remainState;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public float stateTimeElapsed = 0;

    private bool aiActive = true;

    void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void Start()
    {
        chaseTarget = enemy.MyTarget.transform;
    }

    void Update()
    {
        if (!aiActive || enemy.IsDead)
            return;
        currentState.UpdateState(this);
    }

    void OnDrawGizmos()
    {
        if (currentState != null)
        {
            Gizmos.color = currentState.sceneGizmoColor;
        }
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            OnExitState();
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        stateTimeElapsed += Time.deltaTime;
        return (stateTimeElapsed >= duration);
    }

    private void OnExitState()
    {
        stateTimeElapsed = 0;
        enemy.PatrolTime = 0;
    }
}