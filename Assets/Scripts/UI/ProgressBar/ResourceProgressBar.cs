using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceProgressBar : ProgressBar
{
    const string PrefabPath = "UI/ResourceProgressBar/ResourceProgressBar";

    ResourceData max;
    ResourceData current;

    void Start()
    {
        current.OnResourceChange.AddListener(OnCurrentChanged);
    }

    public static ResourceProgressBar Create(Transform parent, Vector2 position, ResourceData max, ResourceData current)
    {
        GameObject go = GameObject.Instantiate(Resources.Load(PrefabPath)) as GameObject;
        go.transform.position = position;
        go.transform.parent = parent;
        var RPP = go.GetComponent<ResourceProgressBar>();
        RPP.max = max;
        RPP.current = current;
        return RPP;
    }

    public void OnCurrentChanged()
    {
        UpdateHealthBar(current.Value, max.Value);
    }
}
