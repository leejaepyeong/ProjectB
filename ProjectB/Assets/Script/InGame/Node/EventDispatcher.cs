using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEventHandler
{
    void OnHandleEvent(EventNodeData eventData);
}
public class EventDispatcher
{
    private class NodeComparer : IComparer<EventNode>
    {
        public int Compare(EventNode x, EventNode y)
        {
            return x.Time.CompareTo(y.Time);
        }
    }

    private static readonly NodeComparer NODE_COMPARER = new();
    private float elasped = 0;

    private List<EventNode> listEventNode;
    private HashSet<EventNode> availabeNodes;
    private UnitBehavior unitBehavior;

    public void Init(UnitBehavior unitBehavior)
    {
        this.unitBehavior = unitBehavior;
        elasped = 0;
        listEventNode = Utilities.StaticeObjectPool.Pop<List<EventNode>>();
        listEventNode.Clear();
        availabeNodes = Utilities.StaticeObjectPool.Pop<HashSet<EventNode>>();
        availabeNodes.Clear();
    }
    public void UnInit()
    {
        elasped = 0;
        listEventNode.Clear();
        availabeNodes.Clear();
    }

    public void Add(EventGraph graph)
    {
        if (graph == null) return;

        for (int i = 0; i < graph.nodes.Count; i++)
        {
            var node = graph.nodes[i];
            if (node is not EventNode eventNode) continue;

            listEventNode.Add(eventNode);
            availabeNodes.Add(eventNode);
        }

        listEventNode.Sort(NODE_COMPARER);
    }

    public void Clear()
    {
        UnInit();
    }

    public void UpdateFrame(float deltaTime, IEventHandler handler)
    {
        elasped += deltaTime;
        int eventNodeCount = listEventNode.Count;

        for (int i = 0; i < eventNodeCount; i++)
        {
            var eventNode = listEventNode[i];
            if (!(elasped > eventNode.Time && availabeNodes.Contains(eventNode)))
                continue;

            handler.OnHandleEvent(eventNode.EventData);
            availabeNodes.Remove(eventNode);
        }

        if(availabeNodes.Count == 0)
        {
            Clear();
        }    
    }
}
