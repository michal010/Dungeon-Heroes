using System.Collections;
using UnityEngine;

public class AnimationPlayEffect : BaseEffect
{
    public string ClipName;

    public override IEnumerator Execute(BaseCharacter Target)
    {
        Animator affectedAnimator = Target.Data.characterReference.GetComponent<Animator>();
        if (affectedAnimator != null)
            affectedAnimator.Play(ClipName);
        yield return null;
    }
}
