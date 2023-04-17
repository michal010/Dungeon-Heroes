using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightUIManager : MonoBehaviour
{
    public GameObject SpellButtonPrefab;
    public Transform SpellsDocker;
    public IPlayerInputContext InputContext;

    public void AddSpellButtons()
    {

    }

    public void AddSpellButton(BaseSpell Spell, BaseCharacter Caster, List<BaseCharacter> Targets)
    {
        GameObject SpellButton = Instantiate(SpellButtonPrefab);
        InputButton inputButton = SpellButton.GetComponent<InputButton>();

        UseSpellPlayerInput useSpellPlayerInput = new UseSpellPlayerInput();
        useSpellPlayerInput.Spell = Spell;

        inputButton.Setup(useSpellPlayerInput, InputContext);

        SpellButton.transform.parent = SpellsDocker;
    }
}
