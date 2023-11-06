using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Mosframe
{
    public class UIGroupSlot : UIBehaviour, IDynamicScrollViewItem
    {
        public enum eDirection
        {
            Vertical,
            Horizontal,
        }

        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private eDirection direction;
        [SerializeField] private int constraintCount;
        [SerializeField] private UISlot protoTypeSlot;

        public int ConstraintCount => constraintCount;
        private List<UISlot> slotList = new();
        protected override void Awake()
        {
            gridLayoutGroup.constraint = direction == eDirection.Vertical ? GridLayoutGroup.Constraint.FixedColumnCount : GridLayoutGroup.Constraint.FixedRowCount;
            gridLayoutGroup.constraintCount = constraintCount;

            Init();
        }

        public void Init()
        {
            for (int i = 0; i < slotList.Count; i++)
            {
                slotList[i].gameObject.SetActive(i < constraintCount);
            }

            for (int i = 0; i < constraintCount; i++)
            {
                UISlot uiSlot;

                if (slotList.Count <= i)
                {
                    uiSlot = Instantiate(protoTypeSlot);
                    slotList.Add(uiSlot);
                }
                else
                    uiSlot = slotList[i];

                uiSlot.transform.SetParent(transform);
                uiSlot.Init();
            }
        }

        public void SetConstraintCount(int value)
        {
            constraintCount = value;
        }

        public void onUpdateItem(int index)
        {
            //Init();

            for (int i = 0; i < constraintCount; i++)
            {
                slotList[i].Open(index * constraintCount + i);
            }
        }
    }
}
