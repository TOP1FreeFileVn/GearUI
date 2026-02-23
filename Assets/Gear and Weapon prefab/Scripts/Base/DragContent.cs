using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragContent : MonoBehaviour, IDragHandler,IBeginDragHandler,IEndDragHandler
{
    [SerializeField] private RectTransform _content;
    [SerializeField] private RectTransform _viewport;
    [SerializeField]private float minY;
    [SerializeField]private float maxY;
    [SerializeField] private Vector2 velocity;
    [SerializeField] private bool isDragging;
    private void Awake()
    {
        _viewport = GetComponent<RectTransform>();

        RectTransform[] rects = GetComponentsInChildren<RectTransform>();

        foreach (RectTransform rect in rects)
        {
            if (rect != _viewport)
            {
                _content = rect;
                break;
            }
        }
    }
    private void Start()
    {
        float contentHeight = _content.rect.height;
        float viewporttHeight = _viewport.rect.height;
        maxY = 300;
        minY = 0;
    }
    private void Update()
    {
        if (!isDragging) {
            _content.anchoredPosition += velocity * Time.deltaTime;
            velocity *= 0.9f;
            Vector2 pos = _content.anchoredPosition;
            pos.y = Mathf.Clamp(pos.y, minY, maxY);
            _content.anchoredPosition = pos; 
        }
    }
    public void OnBeginDrag(PointerEventData eventData) {
        isDragging = true;
        velocity = Vector2.zero;
    }
    public void OnDrag(PointerEventData eventData)
    {
        _content.anchoredPosition += new Vector2(0, eventData.delta.y);
        Vector2 pos = _content.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y, minY, maxY);
        _content.anchoredPosition = pos;
        velocity = new Vector2(0,eventData.delta.y * 15f);
    }
    public void OnEndDrag(PointerEventData eventData) {
        isDragging = false;
    }

}
