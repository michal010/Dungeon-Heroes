using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundData", menuName = "ScriptableObjects/MapSegment/Background", order = 1)]
public class BackgroundData : ScriptableObject
{
    public List<LayerSprite> layerSprites;
}
