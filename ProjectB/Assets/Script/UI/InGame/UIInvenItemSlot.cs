using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using TMPro;

public class UIInvenItemSlot : UISlot
{
    [SerializeField, FoldoutGroup("")] private Image itemIcon;
    [SerializeField, FoldoutGroup("")] private Image equipIcon;

    public override void Open(int index)
    {
        base.Open(index);
    }

    public void Equip()
    {

    }

    public void UnEquip()
    {

    }
}
