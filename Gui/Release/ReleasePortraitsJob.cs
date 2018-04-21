using System.Collections.Generic;
using System.IO;

namespace Xan.StellarisModTools.Release
{
    public class ReleasePortraitsJob : Job
    {
        private readonly bool isActive;
        private readonly string repoPath;

        public ReleasePortraitsJob(IMessageConsole Console, Setup Setup) : base(Console)
        {
            this.isActive = Setup.CreatePortraits;
            this.repoPath = Setup.RepositoryPath;
        }

        public override void Execute()
        {
            if (!this.isActive)
            {
                return;
            }

            string sourceRoot = Path.Combine(this.repoPath, "src", "gfx", "portraits");
            if (!Directory.Exists(sourceRoot))
            {
                this.Message(string.Format("sourceRoot: {0} does not exist.", sourceRoot));
                return;
            }

            string destRoot = Path.Combine(this.repoPath, "output", "XansSciFiMashup", "gfx", "portraits");
            if (!Directory.Exists(destRoot))
            {
                this.Message(string.Format("destRoot: {0} does not exist.", destRoot));
                return;
            }

            string[] pngFiles = Directory.GetFiles(sourceRoot, "*.png", SearchOption.AllDirectories);
            this.Message(string.Format("Found {0} png files.", pngFiles.Length));

            List<string> portraitEntries = new List<string>();
            foreach (string pngFile in pngFiles)
            {
                string[] tokens = pngFile.Split(Path.DirectorySeparatorChar);
                string file = Path.ChangeExtension(tokens[tokens.Length - 1], ".dds");
                string empire = tokens[tokens.Length - 2];
                string franchise = tokens[tokens.Length - 3];

                string destFileName = string.Format("xan_{0}_{1}_{2}", franchise, empire, file);

                Utils.ImageMagickPngToDds(pngFile, Path.Combine(destRoot, destFileName));


                string portraitEntryPath = string.Format("gfx/portraits/{0}", destFileName);
                string portraitEntry = string.Format("{0} = {{ texturefile = \"{1}\" }}", Path.GetFileNameWithoutExtension(destFileName), portraitEntryPath);

                portraitEntries.Add(portraitEntry);
            }

            this.Message(string.Format("Converted {0} png to dds", pngFiles.Length));
            
            this.CreateTxtFile(portraitEntries);

        }

        private void CreateTxtFile(List<string> portraitEntries)
        {
            this.Message("Creating xan_portraits.txt");

            string targetPath = Path.Combine(this.repoPath, "output", "XansSciFiMashup", "gfx", "portraits", "portraits", "xan_portraits.txt");
            using (FileStream fileStream = new FileStream(targetPath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.WriteLine("portraits = {");
                    foreach (string entry in portraitEntries)
                    {
                        streamWriter.Write('\t');
                        streamWriter.WriteLine(entry);
                    }
                    streamWriter.WriteLine("}");
                }
            }

            this.Message("xan_portraits.txt created.");
        }
    }
}
