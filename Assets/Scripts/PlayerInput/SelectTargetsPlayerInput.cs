using SeonerTurnBasedRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SelectTargetsPlayerInput", menuName = "ScriptableObjects/PlayerInput/SelectTargetsPlayerInput", order = 1)]
public class SelectTargetsPlayerInput : BaseInputCondition<List<BaseCharacter>>
{

    public override List<BaseCharacter> Execute()
    {
        if(Input.GetMouseButtonDown(1))
        {
            return null;
        }
        return null;
    }
}
