using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType { Mana, Health, MaxHealth, Initiative, PhysicalDamage, Earth, Water, Air, Fire }

[System.Serializable]
public class ResourceData
{
    public ResourceType ResourceType;
    public int Value;

    public bool stopChange = false;

    public UnityEvent OnResourceBeforeChange = new UnityEvent();
    public UnityEvent OnResourceChange = new UnityEvent();

    public void ChangeResource(int newValue)
    {
        OnResourceBeforeChange?.Invoke();
        if(stopChange)
        {
            stopChange = false;
            return;
        }
        Value = newValue;
        OnResourceChange?.Invoke();
    }
}

[CreateAssetMenu(fileName = "BaseStats", menuName = "ScriptableObjects/Stats/BaseCharactsData", order = 1)]
public class BaseCharacterData : ScriptableObject
{
    public GameObject prefab;
    public GameObject characterReference;
    public ProgressBar healthBar;
    public List<ResourceData> Resources;

    public ResourceData GetResourdeOfType(ResourceType type)
    {
        ResourceData result = Resources.Where(r => r.ResourceType == type).FirstOrDefault();
        return result;
    }
}
