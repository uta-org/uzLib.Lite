using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

using CompressionLevel = Ionic.Zlib.CompressionLevel;

namespace uzLib.Lite.Extensions
{
    public static class CompressionHelper
    {
        public static string Zip(string source, string destination, string fileExtension = "zip")
        {
            string destinationFilename;
            using (ZipFile zip = new ZipFile
            {
                CompressionLevel = CompressionLevel.BestCompression
            })
            {
                var files = Directory.GetFiles(source, "*", SearchOption.AllDirectories)
                    .Where(f => Path.GetExtension(f).ToLowerInvariant() != $".{fileExtension}")
                    .ToArray();

                foreach (var f in files)
                    zip.AddFile(f, GetCleanFolderName(source, f));

                destinationFilename = destination;

                if (Directory.Exists(destination) && !destination.EndsWith($".{fileExtension}"))
                    destinationFilename += $"\\{new DirectoryInfo(source).Name}-{DateTime.Now:yyyy-MM-dd-HH-mm-ss-ffffff}.{fileExtension}";

                zip.Save(destinationFilename);
            }

            return destinationFilename;
        }

        public static void Unzip(string source, string destination)
        {
            if (!destination.IsDirectory())
                throw new ArgumentException("Destination must be a folder.", "destination");

            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            using (ZipFile zipFile = ZipFile.Read(source))
                zipFile.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
        }

        private static string GetCleanFolderName(string source, string filepath)
        {
            if (string.IsNullOrWhiteSpace(filepath))
            {
                return string.Empty;
            }

            var result = filepath.Substring(source.Length);

            if (result.StartsWith("\\"))
            {
                result = result.Substring(1);
            }

            result = result.Substring(0, result.Length - new FileInfo(filepath).Name.Length);

            return result;
        }

        public static async Task<IEnumerable<byte>> Zip(this object obj)
        {
            byte[] bytes = obj.Serialize();

            using (MemoryStream msi = new MemoryStream(bytes))
            using (MemoryStream mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    await msi.CopyToAsync(gs);

                return mso.ToArray().AsEnumerable();
            }
        }

        public static async Task<object> Unzip(this byte[] bytes)
        {
            using (MemoryStream msi = new MemoryStream(bytes))
            using (MemoryStream mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    await gs.CopyToAsync(mso);
                }

                return mso.ToArray().Deserialize();
            }
        }
    }
}