using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuBG : MonoBehaviour
{
    [SerializeField] float circleTimer1, circleTimer2;
    private float lastCircle1, lastCircle2;
    [SerializeField] GameObject circle1Spawner,circle2Spawner;
    [Space]
    [SerializeField] GameObject circleObject;

    void Start(){
        lastCircle1 = lastCircle2 = Time.time;
    }

    void Update(){
        if(Time.time > circleTimer1 + lastCircle1){
            Instantiate(circleObject, circle1Spawner.transform.position, circle1Spawner.transform.rotation);
            lastCircle1 = Time.time;
        }
        if(Time.time > circleTimer2 + lastCircle2){
            Instantiate(circleObject, circle2Spawner.transform.position, circle2Spawner.transform.rotation);
            lastCircle2 = Time.time;
        }
    }
}
