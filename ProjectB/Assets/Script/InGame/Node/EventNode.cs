using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[Serializable]
public abstract class EventNodeData
{
    public abstract string TitleName { get; }
    public abstract string SubjectName { get; }

    private SkillInfo skillInfo;
    public SkillInfo SkillInfo { get { return skillInfo; } set { skillInfo = value; } }
}
[NodeWidth(320)]
public abstract class EventNode : Node
{
    #region Animation Time
    public const float FRAME_RATE = 60f;
    public enum ETimeBase
    {
        TimeElapsed,
        FrameCount,
    }
    [SerializeField, BoxGroup("Animation Time")]
    public ETimeBase TimeBase = ETimeBase.FrameCount;
    [SerializeField, BoxGroup("Animation Time"), ShowIf("@TimeBase == ETimeBase.TimeElapsed")]
    public float FrameElapsed;
    [SerializeField, BoxGroup("Animation Time"), ShowIf("@TimeBase == ETimeBase.FrameCount")]
    public uint FrameCount;
    [SerializeField, BoxGroup("Animation Time")]
    public float FrameRate => graph is EventGraph animationEventGraph ? animationEventGraph.animationFPS : FRAME_RATE;

    [ShowInInspector, BoxGroup("Animation Time")]
    public float Time
    {
        get
        {
            switch (TimeBase)
            {
                case ETimeBase.TimeElapsed:
                    return FrameElapsed;
                case ETimeBase.FrameCount:
                    return FrameCount * 1f / (FrameRate <= 0f ? FRAME_RATE : FrameRate);
            }

            return -1f;
        }
    }
    public void SetFrameElapsed(float frameElapsed)
    {
        FrameElapsed = frameElapsed;
    }
    public void SetFrameCount(uint frameCount)
    {
        FrameCount = frameCount;
    }
    #endregion

    public abstract EventNodeData EventData { get; }

    [SerializeField, TextArea(minLines:1, maxLines:2)]
    private string Memo;
}
