using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Sirenix.OdinInspector;

namespace HitEvenet
{
    public enum eHitRange
    {
        Rect,
        Circle,
        FanShape,
    }

    public enum eHitType
    {
        Normal,
        Wave,
    }
    public class HitEventNode : EventNode
    {
        [LabelWidth(160)]
        public HitEvent HitEvent = new();
        public override EventNodeData EventData => HitEvent;
    }

    #region

    public interface IHitData
    {
        public eHitRange hitRange { get; }
    }


    [Serializable]
    public class Circle : IHitData
    {
        public eHitRange hitRange => eHitRange.Circle;
    }
    [Serializable]
    public class Rect : IHitData
    {
        [SerializeField] private Vector2 range;

        public Vector2 Range => range;

        public eHitRange hitRange => eHitRange.Rect;
    }

    [Serializable]
    public class FanShape : IHitData
    {
        [SerializeField] private float angle;

        public float Angle => angle;

        public eHitRange hitRange => eHitRange.FanShape;
    }
    #endregion
}