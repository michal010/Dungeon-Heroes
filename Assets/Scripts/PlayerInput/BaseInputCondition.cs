using UnityEngine;

public abstract class BaseInputCondition<T> : ScriptableObject
{
    public abstract T Execute();
}
