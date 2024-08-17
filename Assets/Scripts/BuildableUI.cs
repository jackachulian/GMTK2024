using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableUI : MonoBehaviour
{
    [SerializeField] LevelData levelData;
    [SerializeField] GameObject listContainer;
    [SerializeField] GameObject uiItemPrefab;
    private BuildableUIItem[] items;
    // Start is called before the first frame update
    void Start()
    {
        items = new BuildableUIItem[levelData.availableBuildables.Length];
        for (int i = 0; i < levelData.availableBuildables.Length; i++)
        {
            items[i] = Instantiate(uiItemPrefab, listContainer.transform).GetComponent<BuildableUIItem>();
            items[i].Setup(i, levelData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Refresh(int i)
    {
        items[i].Refresh();
    }
}
