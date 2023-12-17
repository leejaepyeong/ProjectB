using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AssetReferenceGameObject projectileEffect;
        [SerializeField] private ParticleEvent hitEvent;

        public AnimationCurve CurveSpeed => curveSpeed;
        public object projectileEffectKey => projectileEffect.RuntimeKey;
        public ParticleEvent HitEvent => hitEvent;

        public eHitRange hitRange => eHitRange.Circle;
    }
    [Serializable]
    public class Rect : IHitData
    {
        [SerializeField] private Vector2 range;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AssetReferenceGameObject projectileEffect;
        [SerializeField] private ParticleEvent hitEvent;

        public Vector2 Range => range;
        public AnimationCurve CurveSpeed => curveSpeed;
        public object projectileEffectKey => projectileEffect.RuntimeKey;
        public ParticleEvent HitEvent => hitEvent;

        public eHitRange hitRange => eHitRange.Rect;
    }

    [Serializable]
    public class FanShape : IHitData
    {
        [SerializeField] private float angle;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AssetReferenceGameObject projectileEffect;
        [SerializeField] private ParticleEvent hitEvent;

        public float Angle => angle;
        public AnimationCurve CurveSpeed => curveSpeed;
        public object projectileEffectKey => projectileEffect.RuntimeKey;
        public ParticleEvent HitEvent => hitEvent;

        public eHitRange hitRange => eHitRange.FanShape;
    }
    #endregion
}