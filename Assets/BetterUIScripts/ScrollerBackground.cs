using UnityEngine;
using UnityEngine.UI;

public class ScrollerBackground : MonoBehaviour
{
    [Header("UI Reference")]
    public RawImage backgroundImage;

    [Header("Scroll Speed")]
    public float speedX = 0.05f; 
    public float speedY = 0.05f; 

    void Update()
    {
        if (backgroundImage != null)
        {

            Rect currentUV = backgroundImage.uvRect;


            currentUV.x += speedX * Time.deltaTime;
            currentUV.y += speedY * Time.deltaTime;

            backgroundImage.uvRect = currentUV;
        }
    }
}