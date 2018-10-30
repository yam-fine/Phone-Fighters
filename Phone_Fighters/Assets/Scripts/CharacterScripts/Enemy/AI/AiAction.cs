using UnityEngine;

public abstract class AiAction : ScriptableObject
{
    public abstract void Act(StateController controller);
}