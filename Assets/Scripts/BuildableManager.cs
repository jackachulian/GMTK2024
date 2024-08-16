using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableManager : MonoBehaviour
{
    [SerializeField] private GameObject[] buildables;
    private int currentSelectedBuildable = 0;

    [SerializeField] private MeshFilter buildPreviewMesh;
    [SerializeField] private GameObject buildPreview;

    public void CycleBuildableSelection()
    {
        // set index
        currentSelectedBuildable = (currentSelectedBuildable + 1) % buildables.Length;

        // update preview mesh
        buildPreviewMesh.sharedMesh = buildables[currentSelectedBuildable].GetComponent<MeshFilter>().sharedMesh;
    }

    public void PlaceBuildable()
    {
        // spawn gameobject
        Instantiate(buildables[currentSelectedBuildable], buildPreview.transform.position, buildPreview.transform.rotation);
    }
}
