using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// takes scale manager from level data and updates objects
public class ScaleManager : MonoBehaviour
{
    [SerializeField] private LevelData levelData;
    [SerializeField] private Transform[] scalableObjects;
    [System.NonSerialized] private Vector3[] baseScales;

    private float lastFrameScale = 1f;

    void Start()
    {
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
                scalableObjects[i].localScale = baseScales[i] * levelData.playerScale;
            }
        }

        lastFrameScale = levelData.playerScale;
    }
}
