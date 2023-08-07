using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISlot_Test : UISlot
{
    [SerializeField] private TextMeshProUGUI textIndex;

    public override void Open(int index)
    {
        if (index > 22)
        {
            Close();
            return;
        }

        base.Open(index);
        textIndex.SetText(index.ToString());
    }
}
