using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIInfinite
{
    public class UIInfiniteItemSlot : MonoBehaviour
    {
        private RectTransform rect;

        public float Width => rect.rect.width;
        public float Height => rect.rect.height;

        private void Awake()
        {
            if (rect == null)
                rect = GetComponent<RectTransform>();
        }

        public void UpdateItemSlot()
        {

        }
    }
}
