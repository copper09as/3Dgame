using System.Collections.Generic;
namespace BehaviorTree
{
    public class Sequencer : Composite
    {

        public Sequencer(List<Node> nodes) : base(nodes)
        {
        }

        public override State Evulate()
        {
            if (index >= children.Count)
            {
                return State.Success;
            }
            var state = children[index].Evulate();
            switch (state)
            {
                case State.Failure: index = 0; return State.Success;
                case State.Success:
                    index += 1;
                    if (index >= children.Count)
                    {
                        return State.Success;
                    }; return State.Running;
                case State.Running: return State.Running;
                default: return State.Failure;
            }

        }

    }



}