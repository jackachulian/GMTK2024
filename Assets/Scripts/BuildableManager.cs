using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableManager : MonoBehaviour
{
    private GameObject[] buildables;
    private int[] buildableAmounts;
    private int currentSelectedBuildable = 0;

    [SerializeField] private MeshFilter buildPreviewMesh;
    [SerializeField] private GameObject buildPreview;
    [SerializeField] public BuildableUI buildUI;

    [SerializeField] private LevelData levelData;

    public bool disabled = false;

    void Start()
    {
        buildables = levelData.availableBuildables;
        Debug.Log(buildables);
        buildableAmounts = levelData.amounts;
        
    }

    public void CycleBuildableSelection(int delta = 1)
    {
        if (disabled) return;

        // find and set next index
        bool found = false;
        for (int i = currentSelectedBuildable + delta; i < buildables.Length + currentSelectedBuildable + delta; i++)   
        {
            if (buildableAmounts[i % buildables.Length] > 0)
            {
                found = true;
                currentSelectedBuildable = i % buildables.Length;
                break;
            }
        }

        if (!found) DisableSelf();

        // currentSelectedBuildable = (currentSelectedBuildable + 1) % buildables.Length;

        // update preview mesh
        buildPreviewMesh.sharedMesh = buildables[currentSelectedBuildable].GetComponent<MeshFilter>().sharedMesh;

        float mul = levelData.playerScale * buildables[currentSelectedBuildable].GetComponent<Buildable>().scaleMult;
        buildPreview.transform.localScale = new Vector3(mul,mul,mul);
        //buildPreviewMesh.transform.localScale = new Vector3(buildables[currentSelectedBuildable].GetComponent<Buildable>().scaleMult,buildables[currentSelectedBuildable].GetComponent<Buildable>().scaleMult,buildables[currentSelectedBuildable].GetComponent<Buildable>().scaleMult);
    }

    public void PlaceBuildable(Transform parent)
    {
        if (disabled) return;

        // spawn gameobject
        Transform b = Instantiate(buildables[currentSelectedBuildable], buildPreview.transform.position, buildPreview.transform.rotation, parent).transform;
        // buildables are always effected by player scale
        b.localScale = b.localScale *= levelData.playerScale * buildables[currentSelectedBuildable].GetComponent<Buildable>().scaleMult;
        buildableAmounts[currentSelectedBuildable] -= 1;
        buildUI.Refresh(currentSelectedBuildable);
        CycleBuildableSelection(0);
    }

    private void DisableSelf()
    {
        disabled = true;
        buildPreview.SetActive(false);
    }
}
