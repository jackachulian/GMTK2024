using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildableUI : MonoBehaviour
{
    public static BuildableUI instance;


    [SerializeField] LevelData levelData;
    [SerializeField] GameObject listContainer;
    [SerializeField] GameObject uiItemPrefab;
    private BuildableUIItem[] items;

    private void Awake() {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        items = new BuildableUIItem[levelData.buildables.Length];
        for (int i = 0; i < levelData.buildables.Length; i++)
        {
            items[i] = Instantiate(uiItemPrefab, listContainer.transform).GetComponent<BuildableUIItem>();
            items[i].Setup(i, levelData);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RefreshItem(int index)
    {
        items[index].Refresh();
    }
}
