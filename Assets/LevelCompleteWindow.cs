using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelCompleteWindow : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;
    [SerializeField] private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Open");
        // EventSystem.current.SetSelectedGameObject(firstSelected);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
    }
}
