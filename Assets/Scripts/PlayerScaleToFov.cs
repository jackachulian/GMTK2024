using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaleToFov : MonoBehaviour
{
    private LevelData levelData;
    private Camera cam;
    private float scaleLastFrame = 1f;

    [SerializeField] private int min = 50;
    [SerializeField] private int max = 70;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        levelData = FindFirstObjectByType<LevelData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelData.playerScale != scaleLastFrame)
        {
            cam.fieldOfView = Mathf.Lerp(min, max, (levelData.playerScale + 0.5f) / 3.5f);
        }
        scaleLastFrame = levelData.playerScale;
    }
}
