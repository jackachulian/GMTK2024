using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// takes scale manager from level data and updates objects
public class ScaleManager : MonoBehaviour
{
    private LevelData levelData;
    private List<Transform> scalableObjects;
    private Vector3[] baseScales;

    private float lastFrameScale = 1f;

    void Start()
    {
        scalableObjects = new List<Transform>
        {
            GameObject.Find("Player").transform,
            GameObject.Find("CamParent").transform
        };
        var buildPreview = GameObject.Find("BuildPreview");
        if (buildPreview) scalableObjects.Add(buildPreview.transform);

        levelData = FindFirstObjectByType<LevelData>();

        baseScales = new Vector3[scalableObjects.Count];
        // store original scales
        for (int i = 0; i < scalableObjects.Count; i++)
        {
            baseScales[i] = scalableObjects[i].localScale;
        }
    }

    void Update()
    {
        if (lastFrameScale != levelData.playerScale)
        {
            for (int i = 0; i < scalableObjects.Count; i++)
            {
                if (scalableObjects[i]) scalableObjects[i].localScale = baseScales[i] * levelData.playerScale;
            }
        }

        lastFrameScale = levelData.playerScale;
    }
}
