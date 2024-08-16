using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapToGround : MonoBehaviour
{
    private Renderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // set position to ground
        RaycastHit hit;
        if (Physics.BoxCast(new Vector3(rend.bounds.center.x, rend.bounds.center.y + 2f, rend.bounds.center.z), transform.localScale * 0.5f, Vector3.down, out hit))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + (rend.bounds.max.y - rend.bounds.min.y) / 2;
            transform.position = pos;
        }
    }
}
