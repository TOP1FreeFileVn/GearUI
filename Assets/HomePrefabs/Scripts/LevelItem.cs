using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelItem : MonoBehaviour
{
    [Header("UI")]
    public TMP_Text levelText;      // Kéo Text vào
    public GameObject lockIcon;     // Kéo ổ khóa vào
    public Button btnButton;        // Kéo Button vào

    private int _index;

    public void Setup(int index, bool isLocked)
    {
        _index = index;

        // 1. XỬ LÝ TEXT
        if (levelText != null)
        {
            // Nếu bị khóa -> Ẩn Text luôn
            if (isLocked)
            {
                levelText.gameObject.SetActive(false);
            }
            else
            {
                // Nếu mở -> Hiện Text
                levelText.gameObject.SetActive(true);
                
                // Set nội dung text
                if (gameObject.name.Contains("Stage"))
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
            btnButton.interactable = !isLocked;
            btnButton.onClick.RemoveAllListeners();
            if (!isLocked) btnButton.onClick.AddListener(OnClicked);
        }
    }

    void OnClicked()
    {
        MapController.Instance.OnLevelComplete(_index);
    }
}