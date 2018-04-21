using System.Collections.Generic;
using System.IO;

namespace Xan.StellarisModTools.Stellaris
{
    public class NodeSerializer
    {
        public StreamWriter Writer
        {
            get;
            set;
        }

        public bool ForceValueQuotes
        {
            get;
            set;
        }

        public bool ForceKeyValueQuotes
        {
            get;
            set;
        }

        public void WriteNodes<T>(List<T> nodes, int indentaion = 0) where T : NodeBase
        {
            foreach (var node in nodes)
            {
                this.WriteNode(node, indentaion);
            }
        }

        public void WriteNode(NodeBase node, int indentaion = 0)
        {
            if (node is CommentNode)
            {
                this.WriteNode((CommentNode)node);
            }
            else if (node is KeyValueNode)
            {
                this.WriteNode((KeyValueNode)node, indentaion);
            }
            else if (node is ValueNode)
            {
                this.WriteNode((ValueNode)node, indentaion);
            }
        }

        public void WriteNode(CommentNode node)
        {
            this.Writer.WriteLine(node.Text);
        }

        public void WriteNode(ValueNode node, int indentaion = 0)
        {
            this.WriteIndentaion(indentaion);
            bool quote = this.ForceValueQuotes || string.IsNullOrEmpty(node.Value) || node.Value.Contains(" ");
            if (quote)
            {
                this.Writer.WriteLine("\"{0}\"", node.Value);
            }
            else
            {
                this.Writer.WriteLine("{0}", node.Value);
            }
        }

        public void WriteNode(KeyValueNode node, int indentaion = 0)
        {
            if (node.HasChilds)
            {
                this.WriteGroupNode(node, indentaion);
            }
            else
            {
                this.WriteKeyValueNode(node, indentaion);
            }
        }

        public void WriteKeyValueNode(KeyValueNode node, int indentaion = 0)
        {
            this.WriteIndentaion(indentaion);
            bool quote = this.ForceKeyValueQuotes || string.IsNullOrEmpty(node.Value) || node.Value.Contains(" ");
            if (quote)
            {
                this.Writer.WriteLine("{0}=\"{1}\"", node.Key, node.Value);
            }
            else
            {
                this.Writer.WriteLine("{0}={1}", node.Key, node.Value);
            }
        }

        public void WriteGroupNode(KeyValueNode node, int indentaion = 0)
        {
            this.WriteIndentaion(indentaion);
            this.Writer.WriteLine("{0}={{", node.Key);

            this.WriteNodes(node.Childs, indentaion + 1);

            this.WriteIndentaion(indentaion);
            this.Writer.WriteLine("}");
        }

        public void WriteIndentaion(int indentation)
        {
            for (int tabCount = 0; tabCount < indentation; tabCount++)
            {
                this.Writer.Write("\t");
            }
        }
    }
}
