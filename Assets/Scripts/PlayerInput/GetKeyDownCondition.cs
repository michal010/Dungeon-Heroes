using UnityEngine;

[CreateAssetMenu(fileName = "BaseInputCondition", menuName = "ScriptableObjects/PlayerInput/BaseInputCondition", order = 1)]
public class GetKeyDownCondition : BaseInputCondition<bool>
{
    public KeyCode Key;
    public override bool Execute()
    {
        if (Input.GetKeyDown(Key))
            return true;
        else return false;
    }
}
