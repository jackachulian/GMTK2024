using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// controls y position of build preview
public class BuildPreview : MonoBehaviour
{
    private Renderer rend;
    [SerializeField] LevelData levelData;
    [SerializeField] Material validPlacementMat, invalidPlacementMat;
    private RaycastHit hit;
    public bool canPlace {get; private set;}

    // player layer
    [SerializeField] LayerMask groundMask;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        levelData = FindObjectOfType<LevelData>();
    }

    public void SnapToGround()
    {
        if (!rend) return;
        // set position to ground
        // RaycastHit hit;
        if (Physics.BoxCast(new Vector3(rend.bounds.center.x, rend.bounds.center.y + 4f * levelData.playerScale, rend.bounds.center.z), 
        new Vector3(rend.bounds.max.x - rend.bounds.min.x, rend.bounds.max.y - rend.bounds.min.y, rend.bounds.max.z - rend.bounds.min.z) * 0.45f, // slight amount of tolerance for placing in gaps
        Vector3.down,
        out hit,
        transform.rotation,
        Mathf.Infinity,
        layerMask: groundMask))
        {
            Vector3 pos = transform.position;
            pos.y = hit.point.y + Mathf.Abs(rend.bounds.max.y - rend.bounds.min.y) / 2;
            transform.position = pos;

            // check with a slightly smaller box size if the preview is intersecting with anything to allow some tolerance
            canPlace = !Physics.CheckBox(
                pos,
                new Vector3(rend.bounds.max.x - rend.bounds.min.x, rend.bounds.max.y - rend.bounds.min.y, rend.bounds.max.z - rend.bounds.min.z) * 0.25f, // much more tolerance for actually letting player if there is reasonable space
                transform.rotation,
                layerMask: groundMask
            );

            Material mat = canPlace ? validPlacementMat : invalidPlacementMat;
            if (rend.material != mat) rend.material = mat;
        }
    }
}

