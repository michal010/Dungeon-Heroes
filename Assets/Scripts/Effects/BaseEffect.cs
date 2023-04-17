using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BaseEffect
{
    public virtual IEnumerator Execute(BaseCharacter Target) { yield return null; }
    public IEnumerator Execute(List<BaseCharacter> Targets)
    {
        foreach (var target in Targets)
        {
            yield return Execute(target);
        }
    }
}
