using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering;

public class LevelCompleteWindow : MonoBehaviour
{
    [SerializeField] private GameObject firstSelected;
    [SerializeField] private Animator animator;
    [SerializeField] private TMP_Text winText1,winText2;
    [SerializeField] private GameObject levelSelectButton, continueButton;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        //Debug.Log("This is level "+ (SceneManager.GetActiveScene().buildIndex - 1));
    }

    void OnEnable(){
        if(SceneManager.GetActiveScene().buildIndex - 1 == 7){
            firstSelected = levelSelectButton;
            // continueButton.SetActive(false);
            winText1.text = "  YOU";
            winText2.text = " WIN!";
        }
        else{
            continueButton.SetActive(true);
            firstSelected = continueButton;
            winText1.text = "LEVEL";
            winText2.text = "CLEAR!";
        }
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Open");
        // EventSystem.current.SetSelectedGameObject(firstSelected);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        // subtract 1 to account for menus in scene build index
        PlayerPrefs.SetInt("LevelClear" + (SceneManager.GetActiveScene().buildIndex - 1), 1);
    }
}
