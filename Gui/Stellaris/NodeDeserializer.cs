using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Xan.StellarisModTools.Stellaris
{
    public class NodeDeserializer
    {
        public NodeBase DeserializeFile(string filePath)
        {
            string fileContent;
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                fileContent = streamReader.ReadToEnd();
            }
            return this.DeserializeContent(fileContent);
        }

        public NodeBase DeserializeContent(string fileContent)
        {
            string beautified = this.Beautify(fileContent);

            string[] lines = beautified.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            List<string> filteredLines = new List<string>();
            foreach (string line in lines)
            {
                string temp = line.Trim(' ', '\t');
                if (temp.IndexOf("={") > 0)
                {
                    string[] tokens = line.Split(new string[] { "={" }, StringSplitOptions.RemoveEmptyEntries);
                    filteredLines.AddRange(tokens);
                    filteredLines.Add("={");
                }
                else if (temp.IndexOf("}") > 0)
                {
                    string[] tokens = line.Split(new char[] { '}' }, StringSplitOptions.RemoveEmptyEntries);
                    filteredLines.AddRange(tokens);
                    filteredLines.Add("}");
                }
                else
                {
                    filteredLines.Add(line);
                }
            }

            //  split into empire groups
            NodeBase root = new KeyValueNode();
            NodeBase currentGroupNode = root;
            string lastLine = null;
            foreach (string line in filteredLines)
            {
                //  check if comment
                string trimmedLine = line.Trim(' ', '\t');
                if(trimmedLine.StartsWith("#"))
                {
                    CommentNode node = new CommentNode
                    {
                        Text = trimmedLine
                    };
                    currentGroupNode.AddChild(node);
                    continue;
                }

                if (line.Contains('{'))
                {
                    KeyValueNode newGroupNode = new KeyValueNode
                    {
                        Key = lastLine.TrimAndRemoveQuotes()
                    };
                    lastLine = null;

                    newGroupNode.Parent = currentGroupNode;
                    currentGroupNode.AddChild(newGroupNode);

                    currentGroupNode = newGroupNode;
                }
                else if (line.Contains('}'))
                {
                    if (lastLine != null)
                    {
                        var parsedNode = this.ParseKeyValueNode(lastLine);
                        if (parsedNode != null)
                        {
                            currentGroupNode.AddChild(parsedNode);
                        }
                        lastLine = null;
                    }

                    currentGroupNode = currentGroupNode.Parent;
                }
                else
                {
                    if (lastLine != null)
                    {
                        var parsedNode = this.ParseKeyValueNode(lastLine);
                        if (parsedNode != null)
                        {
                            currentGroupNode.AddChild(parsedNode);
                        }

                        lastLine = null;
                    }
                    lastLine = line;
                }
            }

            return root;
        }

        private NodeBase ParseKeyValueNode(string lineString)
        {
            string trimmed = lineString.TrimAndRemoveQuotes();
            if (string.IsNullOrEmpty(trimmed))
            {
                return null;
            }
            if (trimmed.Contains('='))
            {
                string[] tokens = trimmed.Split(new char[] { '=' }, StringSplitOptions.None);

                return new KeyValueNode
                {
                    Key = tokens[0],
                    Value = tokens[1],
                };
            }
            return new ValueNode
            {
                Value = trimmed
            };
        }

        private string Beautify(string fileContent)
        {
            // first we remove all carriage returns
            string beautified = fileContent.Replace("\r", "");

            // now we replace all forms of "={" with "={\n"
            beautified =  Regex.Replace(beautified, "[ ,\t]*=[ ,\t]*{", "={\n");

            //  now replace all } with \n}
            beautified = beautified.Replace("}", "\n}");

            return beautified;
        }
    }
}
