using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCursorOnLoad : MonoBehaviour
{
    [SerializeField] private CursorLockMode mode;    
    void Start()
    {        
        UnityEngine.Cursor.lockState = mode;
    }
}
