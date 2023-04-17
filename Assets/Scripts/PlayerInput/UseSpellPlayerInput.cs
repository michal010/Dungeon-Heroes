using SeonerTurnBasedRPG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseSpellPlayerInput", menuName = "ScriptableObjects/PlayerInput/UseSpellPlayerInput", order = 1)]
public class UseSpellPlayerInput : BasePlayerInput
{
    public BaseInputCondition<bool> CancelSpellInput;
    public BaseInputCondition<List<BaseCharacter>> SelectTargetSpellInput;

    public BaseSpell Spell;
    public BaseCharacter Caster;
    public List<BaseCharacter> Targets;

    //TODO:!!!! Solve this target selecting headache.

    public override IEnumerator Execute(IPlayerInputContext InputContext)
    {
        Caster = (InputContext as FightManager).currentCharacter;
        yield return AwaitForPlayerInput();
        //yield return Spell.Execute(Caster, Targets);

    }

    private IEnumerator AwaitForPlayerInput()
    {
        Debug.Log("Awaiting for player select Targets...");
        while(!CancelSpellInput.Execute())
        {
            Debug.Log("loop going...");
            Targets = SelectTargetSpellInput.Execute();
        }
        Debug.Log("Targets has been selected");
        yield return null;
    }
}
