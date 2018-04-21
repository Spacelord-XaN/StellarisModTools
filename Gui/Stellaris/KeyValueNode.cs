namespace Xan.StellarisModTools.Stellaris
{
    public class KeyValueNode : ValueNode
    {
        public string Key
        {
            get;
            set;
        }

        public KeyValueNode()
        {
        }

        public KeyValueNode(string key, string value) : base(value)
        {
            this.Key = key;
        }
    }
}
