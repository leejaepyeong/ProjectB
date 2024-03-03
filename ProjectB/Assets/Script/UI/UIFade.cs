using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIFade : MonoBehaviour
{
    [SerializeField] private Image fadeImg;
    protected UnityAction endAction;
    private bool isFadeAct;
    public bool IsFadeAct => isFadeAct;

    public virtual void Fade(UnityAction action = null)
    {
        endAction = action;
        fadeImg.color = new Color(0, 0, 0, 1);
        fadeImg.raycastTarget = true;
        isFadeAct = true;
        StartCoroutine(CoFade());
    }

    IEnumerator CoFade()
    {
        float time = 0;
        while(time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(0,1,time));
            time += Time.deltaTime;
            yield return null;
        }

        time = 0;
        while (time <= 1)
        {
            fadeImg.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time));
            time += Time.deltaTime;
            yield return null;
        }

        isFadeAct = false;
        endAction?.Invoke();
        fadeImg.raycastTarget = false;
    }
}
