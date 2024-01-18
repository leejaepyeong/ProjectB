using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateAssetMenu]
public class EventGraph : NodeGraph
{
    public float animationFPS = 60f;
    public string animationName;

    public void SetSkill(SkillInfo skillInfo)
    {
        EventNode node;
        for (int i = 0; i < nodes.Count; i++)
        {
            node = nodes[i] as EventNode;
            node.EventData.SkillInfo = skillInfo;
        }
    }
}
