using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UILoading : UIBase
{
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textGuage;
    [SerializeField, FoldoutGroup("Center")] private TextMeshProUGUI textDest;
    [SerializeField, FoldoutGroup("Center")] private Image loadingGauge;
    [SerializeField, FoldoutGroup("Center")] private AnimationCurve animationCurve;

    public Canvas canvas;

    private float elaspedTime;
    private bool isSet;

    public override void Open()
    {
        base.Open();
        isSet = true;
        elaspedTime = 0;
    }

    public override void Close()
    {
        isSet = false;
        Manager.Instance.Fade.Open(base.Close);
    }

    private void Update()
    {
        if (isSet == false) return;
        elaspedTime += Time.deltaTime / 3f;
        loadingGauge.fillAmount = animationCurve.Evaluate(elaspedTime);
        textGuage.SetText($"{loadingGauge.fillAmount * 100f:N1}%");

        if (elaspedTime >= 1)
            Close();
    }
}
