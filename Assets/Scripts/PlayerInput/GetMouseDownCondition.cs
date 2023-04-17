using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GetMouseDownCondition", menuName = "ScriptableObjects/PlayerInput/GetMouseDownCondition", order = 1)]
public class GetMouseDownCondition : BaseInputCondition<bool>
{
    public int MouseIndex;
    public override bool Execute()
    {
        if (Input.GetMouseButtonDown(MouseIndex))
            return true;
        return false;
    }
}
