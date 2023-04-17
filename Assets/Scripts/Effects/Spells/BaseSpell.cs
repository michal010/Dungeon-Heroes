using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum AllowedSpellTarget { Ally, Enemy, Self, AllyNotSelf, None}

[Flags]
public enum EffectTarget { Targets = 1, Caster = 2}

[System.Serializable]
public class Effect
{
    public EffectTarget Target;
    [SerializeReference] public BaseEffect Action;
}

//Add interface to show description.
[CreateAssetMenu(fileName = "BaseSpell", menuName = "ScriptableObjects/Spells/BaseSpell", order = 1)]
public class BaseSpell : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;
    
    public AllowedSpellTarget AllowedTarget;

    [SerializeReference] public List<SpellResource> SpellResources;
    [SerializeReference] public List<Effect> Effects;

    public IEnumerator Execute(BaseCharacter Caster, List<BaseCharacter> Targets)
    {
        Debug.Log("Executing spell...");
        var effectTargetTypes = Enum.GetValues(typeof(EffectTarget));
        foreach (Effect e in Effects)
        {
            foreach (EffectTarget target in effectTargetTypes)
            {
                if ((e.Target & target) == target)
                {
                    switch (target)
                    {
                        case EffectTarget.Targets:
                            Debug.Log("Effect applied to targets...");
                            yield return e.Action.Execute(Targets);
                            break;
                        case EffectTarget.Caster:
                            Debug.Log("Effect applied to caster...");
                            yield return e.Action.Execute(Caster);
                            break;
                    }
                }
            }
        }
    }

    #region Menu functions
    [ContextMenu(nameof(AddSpellResource))]
    void AddSpellResource()
    {
        SpellResources.Add(new SpellResource());
    }

    [ContextMenu(nameof(AddBaseEffect))]
    void AddBaseEffect()
    {
        Effects.Add(new Effect());
    }

    [ContextMenu(nameof(AddAnimationEffect))]
    void AddAnimationEffect()
    {
        Effects.Add(new Effect() { Action = new AnimationPlayEffect() });
    }

    [ContextMenu(nameof(AddVFXEffect))]
    void AddVFXEffect()
    {
        Effects.Add(new Effect() { Action = new VFXEffect() });
    }

    [ContextMenu(nameof(AddOverTimeEffect))]
    void AddOverTimeEffect()
    {
        Effects.Add(new Effect() { Action = new ApplyOverTimeEffect() });
    }

    [ContextMenu(nameof(ResourceDataChangingEffect))]
    void ResourceDataChangingEffect()
    {
        Effects.Add(new Effect() { Action = new ResourceDataChangingEffect() });
    }

    #endregion

}
