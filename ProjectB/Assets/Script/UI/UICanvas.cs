using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class UICanvas : MonoBehaviour
{
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private Canvas _canvas;
    [SerializeField] private GraphicRaycaster _raycaster;
    [SerializeField] private CanvasGroup _canvasGroup;

    public eWidgetType widgetType;
    public RectTransform rectTransform => _rectTransform;
    public Canvas canvas => _canvas;
    public GraphicRaycaster raycaster => _raycaster;
    public CanvasGroup canvasGroup => _canvasGroup;

    public void SetActive(bool active) => gameObject.SetActive(active);

    [ShowInInspector] public UIBase component { get; set; }

    private void Awake()
    {
        _canvas.overrideSorting = true;
    }
}
