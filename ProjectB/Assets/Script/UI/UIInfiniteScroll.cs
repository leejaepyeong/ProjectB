using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIInfinite
{
    [RequireComponent(typeof(ScrollRect))]
    public class UIInfiniteScroll : MonoBehaviour
    {
        public enum eDirection
        {
            Horizontal,
            Vertical,
        }

        public int totalItem = 10;
        public Vector2 offset;
        public eDirection direction = eDirection.Horizontal;
        public bool isGroupSlots;
        public UIInfiniteItemSlot protoItem;

        private ScrollRect scrollRect;
        private List<UIInfiniteItemSlot> itemSlotList = new List<UIInfiniteItemSlot>();

        private Rect scrollSize => scrollRect.GetComponent<RectTransform>().rect;
        private void Awake()
        {
            if(scrollRect == null)
                scrollRect = GetComponent<ScrollRect>();
            if (protoItem == null)
                Debug.LogError("You must add UIInfiniteItemSlot");
        }

        public void Set(int total)
        {
            totalItem = total;
            
            var rect = scrollRect.content.rect;
            rect.width = direction == eDirection.Horizontal ? (protoItem.Width + offset.x) * total : scrollSize.width;
            rect.height = direction == eDirection.Horizontal ? scrollSize.height : (protoItem.Height + offset.y) * total;

            int createCount = direction == eDirection.Horizontal ? 
                (int)(scrollSize.width / rect.width) + 2 : 
                (int)(scrollSize.height / rect.height) + 2;
            for (int i = 0; i < createCount; i++)
            {
                var item = Instantiate(protoItem, scrollRect.content);
                itemSlotList.Add(item);
            }

            SetStartPosition(0);
        }

        public void SetStartPosition(int index)
        {

        }

        private void Update()
        {
            if(direction == eDirection.Horizontal)
            {

            }
            else
            {

            }
        }

        private void OnValueChange()
        {

        }
    }
}
