using System.Linq;
using UnityEngine;

public class PlayerBuildManager : MonoBehaviour {
    [SerializeField] private GameObject buildPreview;
    [SerializeField] private MeshFilter buildPreviewMesh;


    [SerializeField] private float buildDistance = 2f;

    private int currentSelectedBuildable = 0;



    private PlayerMovement movement;
    private PlayerScaleManager scaleManager;

    private void Awake() {
        movement = GetComponent<PlayerMovement>();
        scaleManager = GetComponent<PlayerScaleManager>();
    }

    private bool disabled = false;

    private void FixedUpdate() {
        UpdateBuildPosition();
    }

    public void CycleBuildableSelection(int delta = 1)
    {
        if (disabled) return;

        // find and set next index
        bool found = false;
        int iterations = 0;

        // check the whole list at least once to find the next avaialable buildable. 
        // if whole list is traversed and none have available counts, break loop and disable this script
        while (iterations < LevelData.current.buildables.Length) {
            currentSelectedBuildable += delta;
            if (currentSelectedBuildable >= LevelData.current.buildables.Length) {
                currentSelectedBuildable = 0;
            } else if (currentSelectedBuildable < 0) {
                currentSelectedBuildable = LevelData.current.buildables.Length - 1;
            }

            if (LevelData.current.amounts[currentSelectedBuildable] > 0) {
                found = true;
                break;
            }

            iterations++;
        }

        if (!found) {
            Debug.LogWarning("buildable not found, disabling player build manager");
            DisableSelf();
        }

        // currentSelectedBuildable = (currentSelectedBuildable + 1) % buildables.Length;

        // update preview mesh
        buildPreviewMesh.sharedMesh = LevelData.current.buildables[currentSelectedBuildable].GetComponent<MeshFilter>().sharedMesh;
    }

    public void UpdateBuildPosition() {
        // set lateral position of build preview. vertical position is set in own script
        buildPreview.transform.position = transform.position;
        buildPreview.transform.rotation = transform.rotation;
        buildPreview.transform.localScale = transform.localScale;

        buildPreview.transform.Translate(movement.transform.forward * buildDistance);
    }

    public void PlaceBuildable()
    {
        if (disabled) return;

        // spawn gameobject
        Instantiate(LevelData.current.buildables[currentSelectedBuildable], buildPreview.transform, true);
        LevelData.current.amounts[currentSelectedBuildable] -= 1;
        BuildableUI.instance.RefreshItem(currentSelectedBuildable);
        CycleBuildableSelection(0);
    }

    private void DisableSelf()
    {
        disabled = true;
        buildPreview.SetActive(false);
    }
}