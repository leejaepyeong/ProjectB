using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Projectiles
{
    public enum eProjectileType
    {
        Straight,
        StraightFollow,
        Parabolic,
    }

    public enum eProjectileHit
    {
        Normal,
        Penetration,
    }

    public class ProjectileEventNode : EventNode
    {
        public ProjectileEvent ProjectileEvent = new();
        public override EventNodeData EventData => ProjectileEvent;
    }

    #region

    public interface IProjectileData
    {
        public eProjectileType projectileType { get; }
    }


    [Serializable]
    public class Straight : IProjectileData
    {
        [SerializeField] private float maxDistance;
        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private AnimationCurve curveSpeed;

        public float MaxDistance => maxDistance;
        public float Radius => radius;
        public float Speed => speed;
        public AnimationCurve CurveSpeed => curveSpeed;
        
        public eProjectileType projectileType => eProjectileType.Straight;
    }

    [Serializable]
    public class StraightFollow : IProjectileData
    {
        [SerializeField] private float duration;
        [SerializeField] private float radius;
        [SerializeField] private float speed;
        [SerializeField] private AnimationCurve curveSpeed;
        [SerializeField] private AssetReferenceGameObject hitEffect;

        public float Duration => duration;
        public float Radius => radius;
        public float Speed => speed;
        public AnimationCurve CurveSpeed => curveSpeed;
        public object hitEffectKey => hitEffect.RuntimeKey;

        public eProjectileType projectileType => eProjectileType.StraightFollow;
    }

    [Serializable]
    public class Parabolic : IProjectileData
    {
        public eProjectileType projectileType => eProjectileType.Parabolic;
    }


    #endregion
}

