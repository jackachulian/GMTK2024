using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls y position of build preview
public class BuildPreview : MonoBehaviour
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

    public void SnapToGround()
    {
        if (!rend) return;
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
}

