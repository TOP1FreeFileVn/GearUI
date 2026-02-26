using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text levelText;      // Kéo Text vào
    public GameObject lockIcon;     // Kéo ổ khóa vào
    public Button btnButton;        // Kéo Button vào

    [Header("Settings")]


    private int _index;

    public void Setup(int index, bool isLocked)
    {
        _index = index;

        // 1. XỬ LÝ TEXT
        if (levelText != null)
        {

            levelText.gameObject.SetActive(!isLocked );

            if (levelText.gameObject.activeSelf)
            {
               
                if (!isLocked)
                    levelText.text = "Stage " + index;
                else
                    levelText.text = index.ToString();
            }
        }

        // 2. XỬ LÝ KHÓA
        if (lockIcon) lockIcon.SetActive(isLocked);

        // 3. XỬ LÝ NÚT BẤM
        if (btnButton)
        {
            // Chỉ cho bấm nếu không bị khóa
            btnButton.interactable = !isLocked;
            btnButton.onClick.RemoveAllListeners();
            if (!isLocked)
            {
                btnButton.onClick.AddListener(OnClicked);
            }
        }
    }

    void OnClicked()
    {
        // Khi click vào màn hiện tại, coi như hoàn thành để test logic ẩn màn cũ
        if (MapController.Instance != null)
        {
            MapController.Instance.OnLevelComplete(_index);
        }
    }
}