using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.GetComponent<PlayerMovement>() != null)
        {
            GameObject.Find("LevelData").GetComponent<LevelData>().updateCollectables(1);
            Destroy(gameObject);
        }

    }
}
