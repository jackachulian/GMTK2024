using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildableUIItem : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Renderer rend;
    [SerializeField] private RawImage renderTextureImage;
    [SerializeField] TMPro.TextMeshProUGUI amountText;
    [SerializeField] private Camera cam;
    // index of displayed item in leveldata
    private int index;
    private LevelData levelData;

    public void Setup(int buildableIndex, LevelData levelData)
    {
        index = buildableIndex;
        this.levelData = levelData;

        // set up render texture
        RenderTexture tex = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        tex.Create();
        cam.targetTexture = tex;
        renderTextureImage.texture = tex;

        // set up model
        GameObject buildable = levelData.availableBuildables[buildableIndex];
        meshFilter.sharedMesh = buildable.GetComponent<MeshFilter>().sharedMesh;
        // meshRenderer.materials[0] = buildable.GetComponent<MeshRenderer>().materials[0];
        rend.material = new Material(buildable.GetComponent<Renderer>().sharedMaterial);

        // set up text
        setAmountText(levelData.amounts[buildableIndex]);
    }

    public void Refresh()
    {
        setAmountText(levelData.amounts[index]);
    }

    private void setAmountText(int amount)
    {
        amountText.text = "x " + amount;
    }
}
