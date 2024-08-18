using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used by build preview
public class SnapToGround : MonoBehaviour
{
    private Renderer rend;
    [SerializeField] LevelData levelData;
    private RaycastHit hit;

    // player layer
    private int playerMask = 1 << 8;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // set position to ground
        // RaycastHit hit;
        if (Physics.BoxCast(new Vector3(rend.bounds.center.x, rend.bounds.center.y + 4f * levelData.playerScale, rend.bounds.center.z), 
        new Vector3(rend.bounds.max.x - rend.bounds.min.x, rend.bounds.max.y - rend.bounds.min.y, rend.bounds.max.z - rend.bounds.min.z) * 0.5f, 
        Vector3.down,
        out hit,
        Quaternion.identity,
        Mathf.Infinity,
        layerMask: ~playerMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + Mathf.Abs(rend.bounds.max.y - rend.bounds.min.y) / 2;
            transform.position = pos;
        }
    }

    public void OnDrawGizmos()
    {
        var r = GetComponent<Renderer>();
        if (r == null)
            return;
        var bounds = r.bounds;
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(new Vector3(bounds.center.x,  hit.point.y + (rend.bounds.max.y - rend.bounds.min.y) / 2, bounds.center.z), bounds.extents * 2);

        Gizmos.DrawWireSphere(hit.point, 1f);
    }
}
