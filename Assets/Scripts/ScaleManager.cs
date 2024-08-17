using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// takes scale manager from level data and updates objects

// ====
// JACK UPDATE: going to try to disable this scrpt entirely, objects can/probably should store and modify their scale via their transform. would be simpler

public class ScaleManager : MonoBehaviour
{
    [SerializeField] private PlayerScaleManager scaleManager;
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
        if (lastFrameScale != scaleManager.currentScale)
        {
            for (int i = 0; i < scalableObjects.Length; i++)
            {
                scalableObjects[i].localScale = baseScales[i] * scaleManager.currentScale;
            }
        }

        lastFrameScale = scaleManager.currentScale;
    }
}
