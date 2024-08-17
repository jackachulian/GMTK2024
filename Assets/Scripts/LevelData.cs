using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shared data between all objects in a level
public class LevelData : MonoBehaviour
{
    [SerializeField] public GameObject[] availableBuildables;
    [SerializeField] public int[] amounts;
    [System.NonSerialized] public int[] startAmounts;

    [System.NonSerialized] public float playerScale = 1f;
    // for this level only
    [System.NonSerialized] public int collectablesFound = 0;
    public int collectablesNeeded;

    void Awake()
    {
        startAmounts = new int[amounts.Length];
        System.Array.Copy(amounts, startAmounts, amounts.Length);

    }

    public void updateCollectables(int delta)
    {
        collectablesFound += delta;
        if (collectablesFound >= collectablesNeeded)
        {
            Debug.Log("All collectables found!");
        }
    }
    
}
