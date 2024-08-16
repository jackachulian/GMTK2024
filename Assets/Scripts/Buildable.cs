using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buildable : MonoBehaviour
{
    private Renderer renderer;
    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        // set position to ground
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = new Vector3(hit.point.x, hit.point.y + (renderer.bounds.max.y - renderer.bounds.min.y) / 2, hit.point.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
