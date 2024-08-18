using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("Collectable Trigger Enter");
        if (collider.gameObject.GetComponent<Player>() != null)
        {
            GameObject.Find("LevelData").GetComponent<LevelData>().updateCollectables(1);
            gameObject.SetActive(false);
        }

    }
}
