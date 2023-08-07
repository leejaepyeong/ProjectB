using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEventNode : EventNode
{
    public SoundEvent SoundEvent = new();
    public override EventNodeData EventData => SoundEvent;
}
