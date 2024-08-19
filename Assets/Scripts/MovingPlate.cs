using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlate : MonoBehaviour
{
    [SerializeField] GameObject[] movePoints;
    public bool isMoving;
    private int current;
    [SerializeField] int speed;
    private Vector3 spawnPoint;

    void Awake(){
        current = 0;
        spawnPoint = transform.position;
    }

    public void ResetPosition(){
        transform.position = spawnPoint;
    }

    void FixedUpdate(){
        if(isMoving){
            
            if(transform.position == movePoints[current].transform.position){
                current += 1;
                if(movePoints.Length <= current){
                    current = 0;
                }
            }

            transform.position = Vector3.MoveTowards(transform.position, movePoints[current].transform.position,speed*Time.deltaTime);

        }
    }



}
