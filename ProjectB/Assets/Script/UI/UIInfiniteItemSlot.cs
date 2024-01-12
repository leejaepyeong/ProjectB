using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UIInfinite
{
    public class UIInfiniteItemSlot : MonoBehaviour
    {
        public UnityEvent<int, GameObject> onUpdateSlotItem = new UnityEvent<int, GameObject>();

        public void UpdateItemSlot(int index)
        {
            onUpdateSlotItem?.Invoke(index, gameObject);
        }
        private void OnDestroy()
        {
            onUpdateSlotItem.RemoveAllListeners();
        }
    }
}
