using System.Collections;
using UnityEngine;

public class VFXEffect : BaseEffect
{
    public GameObject VFXEffectPrefab;
    public Vector3 Offset;
    public override IEnumerator Execute(BaseCharacter Target)
    {
        Vector3 position = Target.Data.characterReference.transform.position;
        position += Offset;
        GameObject vfx = GameObject.Instantiate(VFXEffectPrefab, position, Quaternion.identity);
        yield return null;
    }
}
