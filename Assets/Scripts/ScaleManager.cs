using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// takes scale manager from level data and updates objects
public class ScaleManager : MonoBehaviour
{
    private LevelData levelData;
    private Transform[] scalableObjects;
    private Vector3[] baseScales;

    private float lastFrameScale = 1f;

    void Start()
    {
        scalableObjects = new Transform[3];

        scalableObjects[0] = GameObject.Find("Player").transform;
        if (GameObject.Find("BuildPreview")) 
        {
            scalableObjects[1] = GameObject.Find("BuildPreview").transform;
            scalableObjects[2] = GameObject.Find("CamParent").transform;
        }
        else
        {
            scalableObjects[1] = GameObject.Find("CamParent").transform;
        }

        levelData = FindFirstObjectByType<LevelData>();

        baseScales = new Vector3[scalableObjects.Length];
        // store original scales
        for (int i = 0; i < scalableObjects.Length; i++)
        {
            baseScales[i] = scalableObjects[i].localScale;
        }
    }

    void Update()
    {
        if (lastFrameScale != levelData.playerScale)
        {
            for (int i = 0; i < scalableObjects.Length; i++)
            {
                if (scalableObjects[i]) scalableObjects[i].localScale = baseScales[i] * levelData.playerScale;
            }
        }

        lastFrameScale = levelData.playerScale;
    }
}
