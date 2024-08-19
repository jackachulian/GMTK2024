using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// shared data between all objects in a level
public class LevelData : MonoBehaviour
{
    [SerializeField] public GameObject[] availableBuildables;
    public int[] amounts;
    [System.NonSerialized] public int[] startAmounts;

    [SerializeField] public float scaleMin = 0.5f;
    [SerializeField] public float scaleMax = 3f;

    [System.NonSerialized] public float playerScale = 1f;
    // for this level only
    [System.NonSerialized] public int collectablesFound = 0;
    public GameObject spawnPoint;
    public int collectablesNeeded;
    public GameObject collectiblesParent;
    [SerializeField] private TMP_Text collectibleText;

    [SerializeField] private LevelCompleteWindow levelCompleteWindow;
    private PlayerInput playerInput;
    [Space]
    [SerializeField] public PressurePlate[] pressurePlates;
    [SerializeField] public MovingPlate[] movingPlates;

    void Awake()
    {
        startAmounts = new int[amounts.Length];
        System.Array.Copy(amounts, startAmounts, amounts.Length);
        UpdateCollectibleText();

    }

    void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        playerInput = FindFirstObjectByType<PlayerInput>();
        playerInput.enabled = false;
        Invoke("EnableInput", 0.1f);
    }

    void EnableInput()
    {
        if (playerInput) playerInput.enabled = true;
        else Debug.LogWarning("player input not found");
    }

    public void UpdateCollectibleText(){
        collectibleText.text = collectablesFound + "/" + collectablesNeeded;
    }

    public void updateCollectables(int delta)
    {
        collectablesFound += delta;
        UpdateCollectibleText();
        if (collectablesFound >= collectablesNeeded)
        {
            Debug.Log("All collectables found!");
            levelCompleteWindow.OpenMenu();
            playerInput.enabled = false;
        }
    }
    
}
