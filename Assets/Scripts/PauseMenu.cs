using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseMenu : MonoBehaviour
{
    private PlayerInput playerInput;
    [SerializeField] private PlayerPrefSetter[] prefSetters;
    void Start()
    {
        gameObject.SetActive(false);
        playerInput = FindFirstObjectByType<PlayerInput>();
    }
    void OnEnable()
    {
        if (playerInput) playerInput.enabled = false;
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Array.ForEach(prefSetters, pref => pref.Sync());
    }

    void OnDisable()
    {
        if (playerInput) playerInput.enabled = true;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }
}
