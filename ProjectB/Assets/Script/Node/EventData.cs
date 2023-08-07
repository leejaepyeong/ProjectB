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

    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.Straight")] 
    private Projectiles.Straight straight;
    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.StraightFollow")] 
    private Projectiles.StraightFollow straightFollow;
    [SerializeField, ShowIf("@projectileType == Projectiles.eProjectileType.Parabolic")] 
    private Projectiles.Parabolic parabolic;

    [SerializeField]
    private bool isEvent;
    [BoxGroup("NextEvent"), ShowInInspector, ShowIf("@isEvent == true")]
    private List<EventNodeData> eventNodes;

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
