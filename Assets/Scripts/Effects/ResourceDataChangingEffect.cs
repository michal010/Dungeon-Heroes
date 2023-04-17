using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Flags]
public enum ResourceChangeType { Add, Substract, Set, Multiply, Divide}

[System.Flags]
public enum AffectedPosition { Caster = 1, Target = 2, A1 = 4, A2 = 8, A3 = 16, A4 = 32, E1 =64, E2 = 128, E3 = 256, E4 = 512, Plus1 = 1024, Plus2 = 2048, Minus1 = 4096, Minus2 = 8192}

public class ResourceDataChangingEffect : BaseEffect
{
    public ResourceType ResourceType;
    public ResourceChangeType ChangeType = ResourceChangeType.Add;
    public int Value;

    public override IEnumerator Execute(BaseCharacter Target)
    {
        ResourceData affectedResource = Target.Data.Resources.Where(r => r.ResourceType == ResourceType).FirstOrDefault();
        if (affectedResource != null)
            ChangeResource(affectedResource);
        yield return null;
    } 
    
    private void ChangeResource(ResourceData resource)
    {     
        switch (ChangeType)
        {
            case ResourceChangeType.Add:
                resource.Value += Value;
                break;
            case ResourceChangeType.Substract:
                resource.Value -= Value;
                break;
            case ResourceChangeType.Set:
                resource.Value = Value;
                break;
            case ResourceChangeType.Multiply:
                resource.Value *= Value;
                break;
            case ResourceChangeType.Divide:
                resource.Value /= Value;
                break;
        }
    }
}