using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEventNode : EventNode
{
    public ParticleEvent ParticleEvent = new();
    public override EventNodeData EventData => ParticleEvent;
}
