using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapNode
{
    public Vector3 position;
    public string GUID;

    public Dictionary<MapNode, int> nextNodes = new Dictionary<MapNode, int>();
    public Dictionary<MapNode, int> preNodes = new Dictionary<MapNode, int>();
    public MapEventType mapEventType;
    public MapEvent eventInfo;

    public void AddNodeInNextNodes(MapNode node,int level)
    {
        nextNodes.Add(node,level);
    }

    public void AddNodeInPreNodes(MapNode node,int level)
    {
        preNodes.Add(node,level);
    }

    public bool NextNodesIsEmpty()
    {
        return nextNodes.Count == 0;
    }

    public bool PreNodesIsEmpty()
    {
        return preNodes.Count == 0;
    }
    public List<MapNode> ReConstractDict()
    {
        List<MapNode> nodeList = new List<MapNode>(nextNodes.Keys);
        return nodeList;
    }
    public MapNode GetRandomNextNode()
    {
        if (NextNodesIsEmpty())
        {
            return null;
        }
        List<MapNode> nodeList = ReConstractDict();
        int randomIndex = UnityEngine.Random.Range(0, nodeList.Count);
        return nodeList[randomIndex];
    }


    // 添加事件委托类型的成员变量，當玩家進入地塊的時候
    public MapNodeEvent onPlayerEnter;
    public MapNodeEvent onplayerPassed;
    public void PlayerEntered()
    {
        if (onPlayerEnter != null)
        {
            onPlayerEnter(this);
        }
    }
    public void PlayerPassed()
    {
        if (onplayerPassed != null)
        {
            onplayerPassed(this);
        }
    }

    // public void OnPlayerEnterNode(MapNode node)
    // {
    //     Debug.Log($"Player entered node {node.position}");
    //     // Add trigger event logic here
    // }
    // public void AnotherMethod(MapNode node)
    // {
    //     Debug.Log($"This is AnotherMethod()");
    // }
    // public void VoidMethod()
    // {
    //     Debug.Log($"This is VoidMethod()");
    // }

    // Start is called before the first frame update
    public MapNode()
    {
        this.GUID = Guid.NewGuid().ToString();
        // two ways of adding events
        // this.onPlayerEnter += OnPlayerEnterNode;
        // this.onPlayerEnter += AnotherMethod;

        TestingEvent testingEvent = new TestingEvent();
        this.onPlayerEnter += testingEvent.OnPlayerEnterNode;
        this.onPlayerEnter += testingEvent.AnotherMethod;

        this.onplayerPassed += testingEvent.CurrentNodePosition;
        
        // but not void
        // this.onPlayerEnter += VoidMethod;

    }

}
