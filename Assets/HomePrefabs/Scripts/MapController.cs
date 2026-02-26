using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapController : MonoBehaviour
{
    public static MapController Instance;

    [Header("Prefabs")]
    public GameObject stagePrefab;
    public GameObject levelPrefab;

    [Header("UI Reference")]
    public Transform content;
    public ScrollRect scrollRect;

    [Header("Connector Settings")]
    public Sprite ropeSprite;       // Kéo hình PNG sợi dây vào đây
    public float ropeWidth = 30f;   // Độ rộng của dây
    public Color ropeColor = Color.white; // Màu nhuộm (nếu dùng PNG trắng)

    [Header("Config")]
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
    }

    void GenerateInitialMap()
    {
        foreach (Transform child in content) Destroy(child.gameObject);
        levelObjects.Clear();

        int targetSpawn = currentUnlockedLevel + initialBuffer;
        if (targetSpawn > totalLevels) targetSpawn = totalLevels;

        for (int i = currentUnlockedLevel; i <= targetSpawn; i++)
        {
            SpawnLevel(i);
        }
        currentMaxSpawned = targetSpawn;
    }

    public void SpawnLevel(int index)
    {
        if (levelObjects.ContainsKey(index)) return;

        GameObject prefabToUse = (index == currentUnlockedLevel) ? stagePrefab : levelPrefab;
        GameObject newObj = Instantiate(prefabToUse, content);

        newObj.name = "Level_" + index;
        newObj.transform.SetAsFirstSibling(); // Level lớn nằm trên

        LevelItem item = newObj.GetComponent<LevelItem>();
        if (item) item.Setup(index, index > currentUnlockedLevel);

        levelObjects.Add(index, newObj);

        // Cập nhật dây nối sau khi tạo xong
        Invoke("UpdateAllConnectors", 0.05f);
    }

    public void OnLevelComplete(int levelIndex)
    {
        if (levelIndex == currentUnlockedLevel)
        {
            // Xóa level cũ
            if (levelObjects.ContainsKey(levelIndex))
            {
                Destroy(levelObjects[levelIndex]);
                levelObjects.Remove(levelIndex);
            }

            currentUnlockedLevel++;
            PlayerPrefs.SetInt("UnlockedLevel", currentUnlockedLevel);

            // Cập nhật Stage mới
            if (levelObjects.ContainsKey(currentUnlockedLevel))
                UpdateToCurrentStage(currentUnlockedLevel);
            else
                SpawnLevel(currentUnlockedLevel);

            // Sinh thêm màn mới phía trước
            if (currentMaxSpawned < currentUnlockedLevel + initialBuffer)
            {
                int next = currentMaxSpawned + 1;
                if (next <= totalLevels) SpawnLevel(next);
                currentMaxSpawned = Mathf.Max(currentMaxSpawned, next);
            }

            Invoke("UpdateAllConnectors", 0.05f);
        }
    }

    void UpdateToCurrentStage(int index)
    {
        GameObject oldObj = levelObjects[index];
        int siblingIndex = oldObj.transform.GetSiblingIndex();
        Destroy(oldObj);
        levelObjects.Remove(index);

        GameObject newObj = Instantiate(stagePrefab, content);
        newObj.transform.SetSiblingIndex(siblingIndex);
        newObj.GetComponent<LevelItem>().Setup(index, false);
        levelObjects.Add(index, newObj);
    }

    // HÀM TẠO DÂY CHÍNH
    void UpdateAllConnectors()
    {
        foreach (var pair in levelObjects)
        {
            int currentIdx = pair.Key;
            // Đổi từ upperIdx = currentIdx + 1 sang lowerIdx = currentIdx - 1
            int lowerIdx = currentIdx - 1;

            // Nếu tồn tại level phía dưới, tạo dây từ level hiện tại trỏ xuống level đó
            if (levelObjects.ContainsKey(lowerIdx) && levelObjects[lowerIdx] != null)
            {
                CreateConnector(levelObjects[currentIdx], levelObjects[lowerIdx]);
            }
        }
    }

    void CreateConnector(GameObject fromObj, GameObject toObj)
    {
        // Kiểm tra xem đã có dây chưa, nếu chưa thì tạo
        Transform connectorTrans = fromObj.transform.Find("Rope");
        Image img;
        if (connectorTrans == null)
        {
            GameObject ropeObj = new GameObject("Rope", typeof(Image));
            ropeObj.transform.SetParent(fromObj.transform);
            ropeObj.transform.SetAsFirstSibling(); // Nằm dưới icon
            img = ropeObj.GetComponent<Image>();
        }
        else
        {
            img = connectorTrans.GetComponent<Image>();
        }

        // Cấu hình hình ảnh PNG
        img.sprite = ropeSprite;
        img.type = Image.Type.Tiled; // Lặp lại tấm hình
        img.color = ropeColor;

        RectTransform rt = img.rectTransform;

        // Tính toán vị trí giữa 2 điểm
        Vector2 startPos = fromObj.GetComponent<RectTransform>().anchoredPosition;
        Vector2 endPos = toObj.GetComponent<RectTransform>().anchoredPosition;

        // Đặt dây ở giữa
        rt.anchoredPosition = (endPos - startPos) / 2;

        // Tính chiều dài và hướng  
        Vector2 dir = endPos - startPos;
        float distance = dir.magnitude;
        rt.sizeDelta = new Vector2(ropeWidth, distance);

        // Xoay dây theo hướng level
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }
}