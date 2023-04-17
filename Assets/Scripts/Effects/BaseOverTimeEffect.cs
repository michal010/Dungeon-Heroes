using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BaseOverTimeEffect", menuName = "ScriptableObjects/Effects/BaseOverTimeEffect", order = 1)]
public class BaseOverTimeEffect : ScriptableObject
{
    public int Duration = 1;

    public List<BaseEffect> OnRoundStartActions;
    public List<BaseEffect> OnRoundMainActions;
    public List<BaseEffect> OnRoundEndedAction;

    public IEnumerator OnRoundStared(BaseCharacter Target)
    {
        foreach (var effect in OnRoundStartActions)
        {
            yield return effect.Execute(Target);
        }
    }

    public IEnumerator OnRoundMain(BaseCharacter Target)
    {
        foreach (var effect in OnRoundMainActions)
        {
            yield return effect.Execute(Target);
        }
    }
    public IEnumerator OnRoundEnded(BaseCharacter Target)
    {

        foreach (var effect in OnRoundEndedAction)
        {
            yield return effect.Execute(Target);
        }
        Duration--;
    }
}