using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleEventNode : EventNode
{
    public TimeScaleEvent TimeScaleEvent = new();
    public override EventNodeData EventData => TimeScaleEvent;
}
