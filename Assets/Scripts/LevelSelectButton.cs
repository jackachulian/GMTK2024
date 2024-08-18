using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{
    public bool unlocked;
    [SerializeField] private string levelScene;
    private Button b;

    void Awake(){
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
