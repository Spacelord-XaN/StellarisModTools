using System.IO;

namespace Xan.StellarisModTools.Release
{
    public class ReleaseCopyToTarget : Job
    {
        private readonly bool isActive;
        private readonly string repositoryPath;
        private readonly string targetPath;

        public ReleaseCopyToTarget(IMessageConsole Console, Setup Setup) : base(Console)
        {
            this.isActive = Setup.CopyToTarget;
            this.repositoryPath = Setup.RepositoryPath;
            this.targetPath = Setup.TargetPath;
        }

        public override void Execute()
        {
            if (!this.isActive)
            {
                return;
            }

            string sourcePath = Path.Combine(this.repositoryPath, "output");

            this.RemoveFiles(this.targetPath);

            if (this.DirectoryCopy(sourcePath, this.targetPath))
            {
                this.console.Message("Copied files to target path.");
            }
            else
            {
                this.console.Message("Error while copying files to target path.");
            }
        }

        private void RemoveFiles(string DestPath)
        {
            string modDirectory = Path.Combine(DestPath, "XansSciFiMashup");
            if (Directory.Exists(modDirectory))
            {
                Directory.Delete(modDirectory, true);
            }
        }

        private bool DirectoryCopy(string SourcePath, string DestPath)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo sourceInfo = new DirectoryInfo(SourcePath);

            if (!sourceInfo.Exists)
            {
                this.console.Message(string.Format("{0} does not exist", SourcePath));
                return false;
            }

            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(DestPath))
            {
                Directory.CreateDirectory(DestPath);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] fileInfos = sourceInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                string targetFilePath = Path.Combine(DestPath, fileInfo.Name);
                fileInfo.CopyTo(targetFilePath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            DirectoryInfo[] subDirectoryInfos = sourceInfo.GetDirectories();
            foreach (DirectoryInfo subDirectoryInfo in subDirectoryInfos)
            {
                string targetPath = Path.Combine(DestPath, subDirectoryInfo.Name);
                if (!DirectoryCopy(subDirectoryInfo.FullName, targetPath))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
