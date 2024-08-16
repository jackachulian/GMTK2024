using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// shared data between all objects in a level
public class LevelData : MonoBehaviour
{
    [SerializeField] public GameObject[] availableBuildables;
    [SerializeField] public int[] amounts;

    public float playerScale = 1f;
}
