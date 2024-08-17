using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeMenuCircleAnim : MonoBehaviour
{
    private float creation;
    public float speed,life;

    void Start(){
        creation = Time.time;
    }

    void Update(){
        transform.localScale = new Vector3(transform.localScale.x + speed, transform.localScale.y + speed, transform.localScale.z); 

        if(life + creation < Time.time){
            Destroy(gameObject);
        }
    }

}