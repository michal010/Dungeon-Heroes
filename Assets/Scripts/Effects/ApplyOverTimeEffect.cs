using System.Collections;
using UnityEngine;

[System.Serializable]
public class ApplyOverTimeEffect : BaseEffect
{
    public BaseOverTimeEffect Effect;

    public override IEnumerator Execute(BaseCharacter Target)
    {
        Target.OverTimeEffects.Add(Effect);
        yield return null;
    }
}
