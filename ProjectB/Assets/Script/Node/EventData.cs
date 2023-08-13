using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum eGameTime
{
    Local,
    External,
    World,
}

[Serializable]
public class ParticleEvent : EventNodeData
{
    public AssetReferenceGameObject paricleObject;
    public string referenceBone;
    public bool boneBinding;
    public Vector3 localPosition;
    public Vector3 localEular;
    public Vector3 localScale;
    public float duration = 5f;
    public float smoothStop = 2f;
    public eGameTime gameTime = eGameTime.Local;

    public override string TitleName { get; } = "Particle Event";
    public override string SubjectName { get; } = string.Empty;
}

[Serializable]
public class TimeScaleEvent : EventNodeData
{
    public AnimationCurve animationCurve;

    public override string TitleName { get; } = "TimeScale Event";
    public override string SubjectName { get; } = string.Empty;
}

[Serializable]
public class SoundEvent : EventNodeData
{
    public enum eSoundType
    {
        Effect,
        Bgm,
    }

    public eSoundType soundType;
    public AssetReferenceT<AudioClip> soundClip;
    public float volume;
    public float duration;
    public override string TitleName { get; } = "Sound Event";
    public override string SubjectName { get; } = string.Empty;
}

[Serializable]
public class ProjectileEvent : EventNodeData
{
    public Projectiles.eProjectileType projectileType;
    public Projectiles.eProjectileHit projectileHit;

    [SerializeField] private AssetReferenceGameObject projectileEffect;
    [SerializeField] private ParticleEvent hitEvent;
    public object projectileEffectKey => projectileEffect.RuntimeKey;
    public ParticleEvent HitEvent => hitEvent;

    [SerializeField, ShowIf("@projectileHit == eProjectileHit.Penetration")]
    private int maxHitCount;
    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.Straight")] 
    private Projectiles.Straight straight;
    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.StraightFollow")] 
    private Projectiles.StraightFollow straightFollow;
    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.Parabolic")] 
    private Projectiles.Parabolic parabolic;

    public int MaxHitCount => maxHitCount;

    public Projectiles.IProjectileData GetProjectileData()
    {
        switch (projectileType)
        {
            case Projectiles.eProjectileType.Straight:
                return straight;
            case Projectiles.eProjectileType.StraightFollow:
                return straightFollow;
            case Projectiles.eProjectileType.Parabolic:
                return parabolic;
            default:
                return null;
        }
    }

    public override string TitleName { get; } = "Projectile Event";
    public override string SubjectName { get; } = string.Empty;
}

[Serializable]
public class HitEvent : EventNodeData
{
    public HitEvenet.eHitRange hitRange;
    public HitEvenet.eHitType hitType;

    public Vector2 startPos;
    public Vector3 startRot;

    [SerializeField, ShowIf("@hitRange == eHitRange.Circle")]
    private HitEvenet.Circle circle;
    [SerializeField, ShowIf("@hitRange == eHitRange.Rect")]
    private HitEvenet.Rect rect;
    [SerializeField, ShowIf("@hitRange == eHitRange.FanShape")]
    private HitEvenet.FanShape fanShape;

    public HitEvenet.IHitData GetHitData()
    {
        switch (hitRange)
        {
            case HitEvenet.eHitRange.Circle:
                return circle;
            case HitEvenet.eHitRange.Rect:
                return rect;
            case HitEvenet.eHitRange.FanShape:
                return fanShape;
            default:
                return null;
        }
    }

    public override string TitleName { get; } = "Hit Event";
    public override string SubjectName { get; } = string.Empty;
}
