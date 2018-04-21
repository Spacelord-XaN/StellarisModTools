using System;
using System.Linq;
using System.Collections.Generic;

namespace Xan.StellarisModTools.Stellaris
{
    public static class ExtensionMethods
    {
        public static string TrimAndRemoveQuotes(this string value)
        {
            string trimmed = value.Trim('\t', '\r', '\n', ' ');
            return trimmed.Replace("\"", "");
        }

        public static T FindNode<T>(this NodeBase node, Func<T, bool> predicate) where T : NodeBase
        {
            var found = node.Childs.Where(Node => Node is T).Cast<T>().FirstOrDefault(predicate);
            if (found != null)
            {
                return found;
            }
            foreach (var child in node.Childs)
            {
                found = child.FindNode<T>(predicate);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }

        public static List<T> FindManyNodes<T>(this NodeBase node, Func<T, bool> predicate) where T : NodeBase
        {
            List<T> result = new List<T>();
            result.AddRange(node.Childs.Where(Node => Node is T).Cast<T>().Where(predicate));
            foreach (var child in node.Childs)
            {
                result.AddRange(child.FindManyNodes<T>(predicate));
            }
            return result;
        }

        public static KeyValueNode FindData(this NodeBase node, string key)
        {
            return node.FindNode<KeyValueNode>(Node => Node.Key == key);
        }

        public static List<KeyValueNode> FindManyData(this NodeBase node, string key)
        {
            return node.FindManyNodes<KeyValueNode>(Node => Node.Key == key);
        }
    }
}
