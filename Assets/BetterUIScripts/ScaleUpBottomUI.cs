using System.Collections;
using UnityEngine;

public class ScaleUpBottomUI : MonoBehaviour
{
    private RectTransform icon;
    private RectTransform text;

    [Header("Settings")]
    public float duration = 0.2f;
    public Vector3 targetScale = new Vector3(1.3f, 1.3f, 1.3f);

    private Coroutine scaleCoroutine;

    void Start()
    {
        icon = transform.GetChild(0).GetComponent<RectTransform>();
        text = transform.GetChild(1).GetComponent<RectTransform>();
    }

    public void UIisON()
    {
        SetPivot(icon, new Vector2(0.5f, -0.1f)); // pivot đáy
        StopPreviousAndStart(ScaleRoutine(targetScale, true));
    }

    public void UIisOFF()
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);

        SetPivot(icon, new Vector2(0.5f, 0.5f)); // pivot giữa lại
        icon.localScale = Vector3.one;
        text.gameObject.SetActive(false);
    }

    private void StopPreviousAndStart(IEnumerator routine)
    {
        if (scaleCoroutine != null) StopCoroutine(scaleCoroutine);
        scaleCoroutine = StartCoroutine(routine);
    }

    private IEnumerator ScaleRoutine(Vector3 target, bool showText)
    {
        Vector3 initialScale = icon.localScale;
        float elapsed = 0;

        if (showText) text.gameObject.SetActive(true);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);
            icon.localScale = Vector3.Lerp(initialScale, target, t);
            yield return null;
        }

        icon.localScale = target;
    }

  
    private void SetPivot(RectTransform rect, Vector2 newPivot)
    {
        Vector2 size = rect.rect.size;
        Vector2 deltaPivot = rect.pivot - newPivot;
        Vector3 deltaPosition = new Vector3(
            deltaPivot.x * size.x,
            deltaPivot.y * size.y
        );

        rect.pivot = newPivot;
        rect.localPosition -= deltaPosition;
    }
}