using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterBox : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    private void Awake()
    {
        canvas.worldCamera = UIManager.Instance.UiCamera;
    }
}
