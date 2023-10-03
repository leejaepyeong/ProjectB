using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SettingText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmp;
    [SerializeField] int seed;
    private void Start()
    {
        tmp.SetText(TableManager.Instance.stringTable.GetText(seed));
    }
}
