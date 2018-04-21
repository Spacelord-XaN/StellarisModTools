using System.IO;
using System.Linq;
using System.Text;

namespace Xan.StellarisModTools.Stellaris
{
    public class EmpireFileUpgrader
    {
        private readonly NodeDeserializer parser = new NodeDeserializer();
        private readonly NodeSerializer serializer = new NodeSerializer();

        public void Upgrade(string filePath)
        {
            NodeBase root = this.parser.DeserializeFile(filePath);

            using (StreamWriter streamWriter = new StreamWriter(filePath, false))
            {
                this.serializer.Writer = streamWriter;

                foreach (var child in root.Childs.Where(X => X is KeyValueNode).Cast<KeyValueNode>())
                {
                    this.WriteOldEmpire(child);
                }
            }
        }

        private void WriteOldEmpire(KeyValueNode old)
        {
            const int indentation = 1;

            this.serializer.Writer.WriteLine("#================================================================================");
            this.serializer.Writer.WriteLine("# {0}", old.FindData("name").Value);
            this.serializer.Writer.WriteLine("#================================================================================");

            this.serializer.Writer.WriteLine("{0}={{", old.Key);
            this.serializer.WriteNode(old.FindData("name"), indentation);
            this.serializer.WriteNode(old.FindData("ship_prefix"), indentation);

            this.serializer.Writer.WriteLine("\tspecies={");
            this.serializer.WriteNode(new KeyValueNode("class", old.FindData("species_class").Value), indentation + 1);
            this.serializer.WriteNode(old.FindData("portrait"), indentation + 1);
            this.serializer.WriteNode(new KeyValueNode("name", old.FindData("species_name").Value), indentation + 1);
            this.serializer.WriteNode(new KeyValueNode("plural", old.FindData("species_plural").Value), indentation + 1);
            this.serializer.WriteNode(new KeyValueNode("adjective", old.FindData("species_adjective").Value), indentation + 1);
            this.serializer.WriteNode(old.FindData("species_bio"), indentation + 1);
            this.serializer.WriteNode(old.FindData("name_list"), indentation + 1);
            this.serializer.WriteNodes(old.FindManyData("trait"), indentation + 1);
            this.serializer.Writer.WriteLine("\t}");

            this.serializer.WriteNode(old.FindData("adjective"), indentation);
            this.serializer.WriteNode(old.FindData("authority"), indentation);
            this.serializer.WriteNode(old.FindData("government"), indentation);
            this.serializer.WriteNode(old.FindData("ftl"), indentation);
            this.serializer.WriteNode(old.FindData("weapon"), indentation);
            this.serializer.WriteNode(old.FindData("planet_name"), indentation);
            this.serializer.WriteNode(old.FindData("planet_class"), indentation);
            this.serializer.WriteNode(old.FindData("system_name"), indentation);
            this.serializer.WriteNode(old.FindData("initializer"), indentation);
            this.serializer.WriteNode(old.FindData("graphical_culture"), indentation);
            this.serializer.WriteNode(old.FindData("city_graphical_culture"), indentation);

            this.serializer.WriteNode(old.FindData("empire_flag"), indentation);
            this.serializer.WriteNode(old.FindData("ruler"), indentation);

            this.serializer.WriteNode(new KeyValueNode("spawn_as_fallen", "no"), indentation);
            this.serializer.WriteNode(new KeyValueNode("ignore_portrait_duplication", "no"), indentation);
            this.serializer.WriteNode(old.FindData("room"), indentation);
            this.serializer.WriteNode(old.FindData("spawn_enabled"), indentation);
            this.serializer.WriteNodes(old.FindManyData("ethic"), indentation);

            this.serializer.WriteNode(old.FindData("civics"), indentation);

            this.serializer.Writer.WriteLine("\tflags={");
            this.serializer.WriteNode(new ValueNode(old.Key), indentation + 1);
            this.serializer.Writer.WriteLine("\t}");

            this.serializer.Writer.WriteLine("}");
        }
    }
}
