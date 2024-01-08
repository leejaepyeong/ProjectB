using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

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

        private float contentPosition { get {return direction == eDirection.Horizontal ? contentRect.anchoredPosition.x : -contentRect.anchoredPosition.y; } 
            set {contentRect.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(value, contentRect.anchoredPosition.y): new Vector2(contentRect.anchoredPosition.x, -value); } }
        private float contentSize => direction == eDirection.Horizontal ? contentRect.rect.height : contentRect.rect.width;
        private float prevPosition;
        private float itemSize => direction == eDirection.Horizontal ? protoItem.rect.width + offset.x: protoItem.rect.height + offset.y;
        private float itemSize_Other => direction == eDirection.Vertical ? protoItem.rect.width + offset.x : protoItem.rect.height + offset.y;

        [FoldoutGroup("Setting")] public int totalItem = 10;
        [SerializeField, FoldoutGroup("Setting")] private Vector2 offset;
        [SerializeField, FoldoutGroup("Setting")] private eDirection direction = eDirection.Horizontal;
        [SerializeField, FoldoutGroup("Setting")] private RectTransform protoItem;

        [SerializeField, FoldoutGroup("Setting")] private bool isGroupSlots;
        [ShowIf("@isGroupSlots == true"), SerializeField, FoldoutGroup("Setting")] private bool checkLineCount;
        [ShowIf("@isGroupSlots == true && checkLineCount == true"), SerializeField, FoldoutGroup("Setting")] private int lineCount;

        private ScrollRect scrollRect;
        private RectTransform viewportRect;
        private RectTransform contentRect;
        private LinkedList<RectTransform> itemSlotList = new LinkedList<RectTransform>();
        private int nextItemNumber = 0;
        private int groupLineCount = 1;

        private Rect scrollSize => scrollRect.GetComponent<RectTransform>().rect;
        private void Awake()
        {
            if (protoItem == null)
            {
                Debug.LogError("You must add protoItem");
                return;
            }
            scrollRect = GetComponent<ScrollRect>();
            viewportRect = scrollRect.viewport;
            contentRect = scrollRect.content;
        }

        private void Start()
        {
            protoItem.gameObject.SetActive(false);

            if (isGroupSlots)
                SetGroupLine();
            else
                SetOneLine();

            ReSizeContent();
        }

        private void SetOneLine()
        {
            int createCount = direction == eDirection.Horizontal ?
                (int)(scrollSize.width / itemSize) + 3 :
                (int)(scrollSize.height / itemSize) + 3;
            for (int i = 0; i < createCount; i++)
            {
                var item = Instantiate(protoItem);
                item.SetParent(contentRect, false);
                item.name = i.ToString();
                item.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(itemSize * i, 0) : new Vector2(0, -itemSize * i);
                itemSlotList.AddLast(item);
                item.gameObject.SetActive(true);

                UpdateItem(i, item.gameObject);
            }
        }

        private void SetGroupLine()
        {
            int createCount = direction == eDirection.Horizontal ?
                (int)(scrollSize.width / itemSize) + 3 :
                (int)(scrollSize.height / itemSize) + 3;
            if (checkLineCount)
                groupLineCount = lineCount;
            else
                groupLineCount = direction == eDirection.Horizontal ?
                (int)(scrollSize.height / itemSize_Other) :
                (int)(scrollSize.width / itemSize_Other);
            createCount *= groupLineCount;

            for (int i = 0; i < createCount; i++)
            {
                var item = Instantiate(protoItem);
                item.SetParent(contentRect, false);
                item.name = i.ToString();
                item.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(itemSize * (i / groupLineCount), -itemSize_Other * (i % groupLineCount)) : new Vector2(itemSize_Other * (i % groupLineCount), -itemSize * (i / groupLineCount));
                itemSlotList.AddLast(item);
                item.gameObject.SetActive(true);

                UpdateItem(i, item.gameObject);
            }
        }


        private void ReSizeContent()
        {
            var size = contentRect.rect.size;
            if(isGroupSlots)
            {
                int line = totalItem / groupLineCount + (totalItem % groupLineCount == 0 ? 0 : 1);

                size.x = direction == eDirection.Horizontal ? itemSize * line : scrollSize.width;
                size.y = direction == eDirection.Horizontal ? scrollSize.height : itemSize * line;
            }
            else
            {
                size.x = direction == eDirection.Horizontal ? itemSize * totalItem : scrollSize.width;
                size.y = direction == eDirection.Horizontal ? scrollSize.height : itemSize * totalItem;
            }

            var pivot = contentRect.pivot;
            var dist = size - contentRect.rect.size;
            contentRect.offsetMin = contentRect.offsetMin - new Vector2(dist.x * pivot.x, dist.y * pivot.y);
            contentRect.offsetMax = contentRect.offsetMax + new Vector2(dist.x * (1 - pivot.x), dist.y * (1 - pivot.y));
        }

        public void Set(int total)
        {
            totalItem = total;

            SetStartPosition(0);
        }

        public void Refresh()
        {
            int index = 0;
            if (contentPosition != 0)
                index = (int)(-this.contentPosition / itemSize);

            foreach (var item in itemSlotList)
            {
                var movePos = itemSize * index;
                item.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(movePos, 0) : new Vector2(0, -movePos);
                UpdateItem(index, item.gameObject);
                ++index;
            }


        }

        public void SetStartPosition(int index)
        {
            var itemLen = contentSize / totalItem;
            var pos = itemLen * index;
            contentPosition = -pos;
        }

        private void Update()
        {
            if(isGroupSlots)
                UpdateGroupLine();
            else
                UpdateOneLine();
        }

        private void UpdateOneLine()
        {
            while (contentPosition - prevPosition < -itemSize * 2)
            {
                prevPosition -= itemSize;
                var first = itemSlotList.First;
                if (first == null) break;
                var tempItem = first.Value;
                itemSlotList.RemoveFirst();
                itemSlotList.AddLast(tempItem);

                float movePos = itemSize * (itemSlotList.Count + nextItemNumber);
                tempItem.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(movePos, 0) : new Vector2(0, -movePos);

                UpdateItem(itemSlotList.Count + nextItemNumber, tempItem.gameObject);
                nextItemNumber++;
            }
            while (contentPosition - prevPosition > 0)
            {
                prevPosition += itemSize;
                var last = itemSlotList.Last;
                if (last == null) break;
                var tempItem = last.Value;
                itemSlotList.RemoveLast();
                itemSlotList.AddFirst(tempItem);

                nextItemNumber--;

                float movePos = itemSize * nextItemNumber;
                tempItem.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(movePos, 0) : new Vector2(0, -movePos);

                UpdateItem(nextItemNumber, tempItem.gameObject);
            }
        }
        private void UpdateGroupLine()
        {
            while (contentPosition - prevPosition < -itemSize * 2)
            {
                prevPosition -= itemSize;

                float movePos = itemSize * ((itemSlotList.Count / groupLineCount) + nextItemNumber);
                for (int i = 0; i < groupLineCount; i++)
                {
                    var first = itemSlotList.First;
                    if (first == null) break;
                    var tempItem = first.Value;
                    itemSlotList.RemoveFirst();
                    itemSlotList.AddLast(tempItem);

                    tempItem.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(movePos, tempItem.anchoredPosition.y) : new Vector2(tempItem.anchoredPosition.x, -movePos);
                    UpdateItem(itemSlotList.Count + (nextItemNumber * groupLineCount) + i, tempItem.gameObject);
                }
                nextItemNumber++;
            }
            while (contentPosition - prevPosition > 0)
            {
                prevPosition += itemSize;

                nextItemNumber--;

                float movePos = itemSize * nextItemNumber;
                for (int i = 1; i <= groupLineCount; i++)
                {
                    var last = itemSlotList.Last;
                    if (last == null) break;
                    var tempItem = last.Value;
                    itemSlotList.RemoveLast();
                    itemSlotList.AddFirst(tempItem);

                    tempItem.anchoredPosition = direction == eDirection.Horizontal ? new Vector2(movePos, tempItem.anchoredPosition.y) : new Vector2(tempItem.anchoredPosition.x, -movePos);
                    UpdateItem(((nextItemNumber + 1) * groupLineCount) - i, tempItem.gameObject);
                }
            }
        }


        private void UpdateItem(int index, GameObject itemObj)
        {

            if (index < 0 || index >= totalItem)
            {
                itemObj.SetActive(false);
            }
            else
            {

                itemObj.SetActive(true);

                var item = itemObj.GetComponent<UIInfiniteItemSlot>();
                if (item != null) item.UpdateItemSlot(index);
            }
        }
    }
}
