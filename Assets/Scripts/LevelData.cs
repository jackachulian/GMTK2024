using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shared data between all objects in a level
public class LevelData : MonoBehaviour
{
    // The current level being played, (last level data that Awake() has ran on)
    public static LevelData current;

    // All types of buildables that are shown in the on-screen list of buildables the player can place in this level
    [SerializeField] private GameObject[] _buildables;
    // Current amount of buildables remaining that the player can place, corresponds to index in buildables
    [SerializeField] private int[] _amounts;
    // min and max bounds that the player can scale themself to
    [SerializeField] private float _minScale = 0.5f, _maxScale = 3f;
    // amount of collectables required to clear this level
    [SerializeField] private int _collectablesNeeded;


    // copies buildable amounts at start of level, used to refer to what the initial amounts were
    public int[] startAmounts {get; private set;}
    // for this level only
    private int collectablesFound = 0;


    // getters
    public GameObject[] buildables => _buildables;
    public int[] amounts => _amounts;
    public float minScale => _minScale;
    public float maxScale => _maxScale;
    public int collectablesNeeded => _collectablesNeeded;
    

    void Awake()
    {
        current = this;
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
