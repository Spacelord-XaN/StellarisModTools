using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Xan.StellarisModTools
{
    public static class Utils
    {
        public static string SuggestInkscapePath()
        {
            return @"C:\Program Files\Inkscape\inkscape.exe";
        }

        public static string SuggestRepositoryPath()
        {
            Assembly myAssembly = Assembly.GetAssembly(typeof(Utils));
            FileInfo fileInfo = new FileInfo(myAssembly.Location);
            string path = Path.Combine(fileInfo.Directory.FullName, "..", "..", "..", "..", "..", "..");
            path = Path.GetFullPath(path);
            return path;
        }

        public static string SuggestTargetPath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            path = Path.Combine(path, "Paradox Interactive", "Stellaris", "mod");
            return path;
        }

        public static void ImageMagickPngToDds(string InputFile, string OutputFile)
        {
            byte[] data;
            using (MagickImage image = new MagickImage(InputFile))
            {
                image.Format = MagickFormat.Dds;
                image.WriteMask.Depth = 8;
                image.WriteMask.ColorSpace = ColorSpace.RGB;
                image.WriteMask.ColorType = ColorType.TrueColorAlpha;
                image.WriteMask.Settings.Compression = Compression.NoCompression;
                data = image.ToByteArray();
            }

            using (FileStream fileStream = new FileStream(OutputFile, FileMode.Create, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length);
            }
        }

        public static IEnumerable<string> ParseLines(string Text)
        {
            List<string> names = new List<string>();
            string[] tokens = Text.Split('\n', '\r');

            List<string> lines = new List<string>();
            foreach (string token in tokens)
            {
                if (string.IsNullOrEmpty(token))
                {
                    continue;
                }
                lines.Add(token.Trim());
            }
            return lines;
        }

        public static string MakeLines(IEnumerable<string> Lines)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in Lines)
            {
                sb.AppendLine(line);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Creates a relative path from one file or folder to another.
        /// </summary>
        /// <param name="FromPath">Contains the directory that defines the start of the relative path.</param>
        /// <param name="toPath">Contains the path that defines the endpoint of the relative path.</param>
        /// <returns>The relative path from the start directory to the end path or <c>toPath</c> if the paths are not related.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        /// <exception cref="InvalidOperationException"></exception>
        public static string MakeRelativePath(string FromPath, string ToPath)
        {
            if (string.IsNullOrEmpty(FromPath))
            {
                throw new ArgumentNullException("FromPath");
            }
            if (string.IsNullOrEmpty(ToPath))
            {
                throw new ArgumentNullException("ToPath");
            }

            Uri fromUri = new Uri(FromPath);
            Uri toUri = new Uri(ToPath);

            // path can't be made relative.
            if (fromUri.Scheme != toUri.Scheme)
            {
                return ToPath;
            }

            Uri relativeUri = fromUri.MakeRelativeUri(toUri);
            String relativePath = Uri.UnescapeDataString(relativeUri.ToString());

            if (toUri.Scheme.Equals("file", StringComparison.InvariantCultureIgnoreCase))
            {
                relativePath = relativePath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
            }

            return relativePath;
        }
    }
}
