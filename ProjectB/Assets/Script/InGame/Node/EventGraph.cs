using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using XNode;

[CreateAssetMenu]
public class EventGraph : NodeGraph
{
    public float animationFPS = 60f;

    public void SetSkill(SkillInfo skillInfo)
    {
        EventNode node;
        for (int i = 0; i < nodes.Count; i++)
        {
            node = nodes[i] as EventNode;
            switch (node.EventData)
            {
                case ProjectileEvent projectileEvent:
                    projectileEvent.SkillInfo = skillInfo;
                    break;
            }
        }
    }
}
