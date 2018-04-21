using System.Collections.Generic;

namespace Xan.StellarisModTools.Stellaris
{
    public abstract class NodeBase
    {
        public NodeBase()
        {
            this.Childs = new List<NodeBase>();
        }

        public NodeBase Parent
        {
            get;
            set;
        }

        public List<NodeBase> Childs
        {
            get;
            set;
        }

        public bool HasChilds
        {
            get
            {
                return this.Childs.Count > 0;
            }
        }

        public void AddChild(NodeBase child)
        {
            child.Parent = this;
            this.Childs.Add(child);
        }
    }
}
