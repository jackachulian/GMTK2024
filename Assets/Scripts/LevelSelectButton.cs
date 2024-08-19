using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    [SerializeField] private int levelNum;
    // private bool unlocked;
    [SerializeField] private string levelScene;
    private Button b;

    void Start(){
        bool unlocked = PlayerPrefs.GetInt("LevelClear" + levelNum) == 1 || levelNum == 0; 
        b = GetComponent<Button>();
        if(unlocked){
            b.interactable = true;
        }
        else{
            b.interactable = false;
        }
    }

    public void OpenLevel(){
        SceneManager.LoadScene(levelScene);
    }
}
