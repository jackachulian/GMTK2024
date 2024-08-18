using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    [SerializeField] Material pressedMaterial,notPressedMaterial;
    [SerializeField] MovingPlate plateToMove;
    [SerializeField] Renderer matRenderer;
    private int pressed;

    void Start(){
        pressed = 0;
    }

    void Update(){
        if(pressed > 0){
            matRenderer.material = pressedMaterial;
            plateToMove.isMoving = true;
        }
        else{
            matRenderer.material = notPressedMaterial;
            plateToMove.isMoving = false;
        }
    }

    void OnCollisionEnter(Collision c){
        if(c.gameObject.GetComponent<CanTriggerSwitches>() != null){
            pressed += 1;
        }
    }

    void OnCollisionExit(Collision c){
        if(c.gameObject.GetComponent<CanTriggerSwitches>() != null){
            pressed -= 1;
        }
    }

}
