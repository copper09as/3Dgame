using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree
{
    public abstract class Node
    {
        protected Node parent;
        protected State state;
        public Node()
        {

        }
        public abstract State Evulate();
     }




}

