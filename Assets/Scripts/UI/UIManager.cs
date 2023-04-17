using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Transform ProgressBarsDeck;


    public ResourceProgressBar SpawnResourceProgressBar(Vector3 position, ResourceData max, ResourceData min)
    {
        ResourceProgressBar progressBar = ResourceProgressBar.Create(ProgressBarsDeck, GetScreenPoint(position), max, min);
        return progressBar;
    }

    //Move to other class
    private Vector2 GetScreenPoint(Vector3 worldPoint)
    {
        return Camera.main.WorldToScreenPoint(worldPoint);
    }
}
