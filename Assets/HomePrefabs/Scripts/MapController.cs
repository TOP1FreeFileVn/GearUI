using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public static MapController Instance;

    [Header("Prefab")]
    public GameObject stagePrefab; 
    public GameObject levelPrefab; 
    
    [Header("UI Reference")]
    public Transform content;
    public ScrollRect scrollRect;

    [Header("Cấu hình")]
    public int totalLevels = 1000;
    public int currentUnlockedLevel = 1;
    public int initialBuffer = 10; 

    private int currentMaxSpawned = 0; 
    private Dictionary<int, GameObject> levelObjects = new Dictionary<int, GameObject>();

    void Awake()
    {
        Instance = this;
        currentUnlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    void Start()
    {
        GenerateInitialMap();

        if (scrollRect != null)
        {
            scrollRect.onValueChanged.AddListener(OnScroll);
        }
    }

    void GenerateInitialMap()
    {
        foreach (Transform child in content) Destroy(child.gameObject);
        levelObjects.Clear();

        int targetSpawn = currentUnlockedLevel + initialBuffer;
        
        for (int i = 1; i <= targetSpawn; i++)
        {
            SpawnLevel(i);
        }

        StartCoroutine(ScrollToBottom());
    }


    void OnScroll(Vector2 pos)
    {        if (pos.y > 0.9f && currentMaxSpawned < totalLevels)
        {
            SpawnNextBatch();
        }
    }

    void SpawnNextBatch()
    {

        int nextTarget = currentMaxSpawned + 10;
        if (nextTarget > totalLevels) nextTarget = totalLevels;

        for (int i = currentMaxSpawned + 1; i <= nextTarget; i++)
        {
            SpawnLevel(i);
        }
    }


    void SpawnLevel(int index)
    {
        if (levelObjects.ContainsKey(index)) return;

        GameObject prefabToUse = (index == currentUnlockedLevel) ? stagePrefab : levelPrefab;
        
        GameObject newObj = Instantiate(prefabToUse, content);

        newObj.name = (index == currentUnlockedLevel) ? "Stage_" + index : "Level_" + index;

        newObj.transform.SetAsFirstSibling();

        LevelItem item = newObj.GetComponent<LevelItem>();
        if (item)
        {
            bool isLocked = (index > currentUnlockedLevel);
            item.Setup(index, isLocked);
        }

        levelObjects.Add(index, newObj);

        if (index > currentMaxSpawned) currentMaxSpawned = index;
    }

    public void OnLevelComplete(int levelIndex)
    {
        if (levelIndex == currentUnlockedLevel)
        {
            currentUnlockedLevel++;
            PlayerPrefs.SetInt("UnlockedLevel", currentUnlockedLevel);
            PlayerPrefs.Save();

            SwapVisual(levelIndex, levelPrefab);
            SwapVisual(currentUnlockedLevel, stagePrefab);

            if (currentMaxSpawned < currentUnlockedLevel + initialBuffer)
            {
                SpawnNextBatch();
            }
        }
    }

    void SwapVisual(int index, GameObject targetPrefab)
    {
        if (levelObjects.ContainsKey(index))
        {
            GameObject oldObj = levelObjects[index];
            int oldIndex = oldObj.transform.GetSiblingIndex();

            Destroy(oldObj);
            levelObjects.Remove(index);

            GameObject newObj = Instantiate(targetPrefab, content);
            newObj.name = targetPrefab.name + "_" + index;

            LevelItem item = newObj.GetComponent<LevelItem>();
            if (item) item.Setup(index, false);

            newObj.transform.SetSiblingIndex(oldIndex);
            levelObjects.Add(index, newObj);

             LayoutRebuilder.ForceRebuildLayoutImmediate(content.GetComponent<RectTransform>());
        }
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        if (scrollRect) scrollRect.verticalNormalizedPosition = 0f;
    }
}