using ImageMagick;
using System.Diagnostics;
using System.IO;

namespace Xan.StellarisModTools.Release
{
    public class ReleaseFlagsJob : Job
    {
        private readonly bool isActive;
        private readonly string inkscapePath;
        private readonly string repoPath;

        public ReleaseFlagsJob(IMessageConsole Console, Setup Setup) : base(Console)
        {
            this.isActive = Setup.CreateFlags;
            this.repoPath = Setup.RepositoryPath;
            this.inkscapePath = Setup.InkscapePath;
        }

        public override void Execute()
        {
            if (!this.isActive)
            {
                return;
            }

            if (string.IsNullOrEmpty(this.inkscapePath))
            {
                this.Message("InkscapePath is empty or null");
                return;
            }

            if (!File.Exists(this.inkscapePath))
            {
                this.Message(string.Format("InkscapePath: {0} does not exit.", this.inkscapePath));
                return;
            }

            string sourceRoot = Path.Combine(this.repoPath, "src", "flags", "xan");
            if (!Directory.Exists(sourceRoot))
            {
                this.Message(string.Format("sourceRoot: {0} does not exist.", sourceRoot));
                return;
            }

            string destRoot = Path.Combine(this.repoPath, "output", "XansSciFiMashup", "flags", "xan");
            if (!Directory.Exists(destRoot))
            {
                this.Message(string.Format("destRoot: {0} does not exist.", destRoot));
                return;
            }

            string[] svgFiles = Directory.GetFiles(sourceRoot, "*.svg", SearchOption.AllDirectories);
            this.Message(string.Format("Found {0} svg files.", svgFiles.Length));

            foreach (string svgFile in svgFiles)
            {
                string[] tokens = svgFile.Split(Path.DirectorySeparatorChar);
                string subFlag = tokens[tokens.Length - 2];
                if (subFlag != "map" && subFlag != "small")
                {
                    subFlag = string.Empty;
                }

                string pngTempPath = Path.GetTempFileName();
                this.CallInkscape(svgFile, pngTempPath);

                string destFileName = Path.GetFileName(svgFile);
                destFileName = Path.ChangeExtension(destFileName, ".dds");
                string destFilePath = string.Empty;
                if (string.IsNullOrEmpty(subFlag))
                {
                    destFilePath = Path.Combine(destRoot, destFileName);
                }
                else
                {
                    destFilePath = Path.Combine(destRoot, subFlag, destFileName);
                }

                Utils.ImageMagickPngToDds(pngTempPath, destFilePath);

                File.Delete(pngTempPath);
            }

            this.Message(string.Format("Converted {0} svg to dds", svgFiles.Length));

        }

        private void CallInkscape(string InputFile, string OutputFile)
        {
            string arguments = " -z -e \"{0}\" \"{1}\"";
            arguments = string.Format(arguments, OutputFile, InputFile);

            Process p = Process.Start(this.inkscapePath, arguments);
            p.WaitForExit();
        }
    }
}
