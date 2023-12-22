using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class SoundEventNode : EventNode
{
    [LabelWidth(160)]
    public SoundEvent SoundEvent = new();
    public override EventNodeData EventData => SoundEvent;
}
