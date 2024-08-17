using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    private Renderer rend;
    public float scaleMult, uiScaleMult;
    // Start is called before the first frame update
    void Start()
    {
        // rend = GetComponent<Renderer>();
        // // set position to ground
        // RaycastHit hit;
        // if (Physics.BoxCast(rend.bounds.center, transform.localScale * 0.5f, Vector3.down, out hit))
        // {
        //     transform.position = new Vector3(hit.point.x, hit.point.y + (rend.bounds.max.y - rend.bounds.min.y) / 2, hit.point.z);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
