using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
[System.Serializable]
public class LayerSprite
{
    public int Order;
    public Vector3 LocalOffset;
    public Sprite Sprite;
    public Vector3 LocalScale;
}

public class BackgroundManager : MonoBehaviour
{
    public List<LayerSprite> layerSprites = new List<LayerSprite>();
    public Transform Background;

    [SerializeField] private string fileName;
    [SerializeField] private string dataSavePath = "Resources/Data/MapSegments/Backgrounds/";

    private List<Transform> layerHolders;
    private string layerName = "Layer_";

    public BackgroundData testData;

    private void Awake()
    {
        //if no background then grab
        if (Background == null)
            Background = GameObject.Find("Background").transform;
        if (Background == null)
            Background = transform;
    }
    public void SwapLayers(BackgroundData Data)
    {
        layerSprites = new List<LayerSprite>(Data.layerSprites);
        layerSprites.OrderBy(o => o.Order);
        UpdateVisuals();
    }

    [ContextMenu("SwapLayers with test data")]
    public void SwapLayersWithTestData()
    {
        SwapLayers(testData);
    }

    [ContextMenu("Copy current layout")]
    public void CopyLayout()
    {
#if UNITY_EDITOR
        if (Background == null)
            Background = GameObject.Find("Background").transform;
        if(Background == null)
        {
            Debug.LogError("There is no Background gameobject in the scene to copy data from.");
            return;
        }

        List<LayerSprite> copiedLayers = new List<LayerSprite>();

        foreach(Transform layerHolder in Background)
        {
            foreach (Transform layer in layerHolder)
            {
                SpriteRenderer sR = layer.GetComponent<SpriteRenderer>();
                if(sR != null)
                {
                    copiedLayers.Add(new LayerSprite() { LocalOffset = layer.localPosition, Order = sR.sortingOrder, Sprite = sR.sprite, LocalScale = layer.localScale});
                }
            }
        }
        BackgroundData backgroundData = ScriptableObject.CreateInstance<BackgroundData>();
        backgroundData.layerSprites = copiedLayers;

        AssetDatabase.CreateAsset(backgroundData, dataSavePath + fileName + ".asset");
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = backgroundData;
            #endif
    }

    private void UpdateVisuals()
    {
#if UNITY_EDITOR
        DeleteOldEditor();
#else
        DeleteOld();
#endif
        foreach (var layerSprite in layerSprites)
        {
            string layerHolderFullName = layerName + layerSprite.Order;
            Transform layerHolder = layerHolders.Where(o => o.name == layerHolderFullName).FirstOrDefault();
            if(layerHolder == null)
            {
                layerHolder = CreateLayerHoldersHolder(layerSprite.Order);
            }
            //Create actual layer
            CreateLayer(layerHolder, layerSprite);
        }
    }

    private Transform CreateLayer(Transform parent, LayerSprite layerSprite)
    {
        GameObject layer = new GameObject();
        layer.transform.parent = parent;
        SpriteRenderer sR = layer.AddComponent<SpriteRenderer>();
        sR.sprite = layerSprite.Sprite;
        sR.sortingOrder = layerSprite.Order;
        layer.transform.localPosition = layerSprite.LocalOffset;
        layer.transform.localScale = layerSprite.LocalScale;
        return layer.transform;
    }

    private Transform CreateLayerHoldersHolder(int order)
    {
        GameObject layerHolder = new GameObject(layerName + order);
        layerHolder.transform.parent = Background;
        layerHolder.transform.localPosition = Vector3.zero;
        layerHolders.Add(layerHolder.transform);
        return layerHolder.transform;
    }

    private void DeleteOld()
    {
        layerHolders = new List<Transform>();
        int childCount = Background.childCount - 1;
        for (int i = childCount; i >= 0; i--)
        {
            Destroy(Background.GetChild(i).gameObject);
        }
    }

    [ContextMenu("Destroy Layers")]
    private void DeleteOldEditor()
    {
        layerHolders = new List<Transform>();
        int childCount = Background.childCount - 1;
        for (int i = childCount; i >= 0; i--)
        {
            DestroyImmediate(Background.GetChild(i).gameObject);
        }
    }
}
