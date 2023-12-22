using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleEventNode : EventNode
{
    [LabelWidth(160)]
    public ParticleEvent ParticleEvent = new();
    public override EventNodeData EventData => ParticleEvent;
}
