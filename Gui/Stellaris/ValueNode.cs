namespace Xan.StellarisModTools.Stellaris
{
    public class ValueNode : NodeBase
    {
        public ValueNode()
        {
        }

        public ValueNode(string value)
        {
            this.Value = value;
        }

        public string Value
        {
            get;
            set;
        }
    }
}
