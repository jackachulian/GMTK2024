using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScaleToFov : MonoBehaviour
{
    private PlayerScaleManager playerScaleManager;
    private Camera cam;
    private float scaleLastFrame = 1f;

    [SerializeField] private int min = 50;
    [SerializeField] private int max = 70;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        playerScaleManager = FindFirstObjectByType<PlayerScaleManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScaleManager.currentScale != scaleLastFrame)
        {
            cam.fieldOfView = Mathf.Lerp(min, max, Mathf.InverseLerp(playerScaleManager.currentScale, LevelData.current.minScale, LevelData.current.maxScale));
        }
        scaleLastFrame = playerScaleManager.currentScale;
    }
}
