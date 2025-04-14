using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
namespace BehaviorTree
{
    public abstract class Composite : Node
    {
        protected int index;
        protected List<Node> children = new List<Node>();
        protected Composite(List<Node> nodes)
        {
            index = 0;
            foreach(var node in nodes)
            {
                children.Add(node);
            }
        }
    }

}

