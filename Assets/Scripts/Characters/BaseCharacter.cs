using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName ="ScriptableObjects/Characters/BaseCharacter", order = 1)]
public class BaseCharacter : ScriptableObject
{
    public string Name;

    public List<BaseSpell> Spells;

    public BaseCharacterData Data;

    public List<BaseOverTimeEffect> OverTimeEffects;

}
