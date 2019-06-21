#if !UNITY_2018 && !UNITY_2017 && !UNITY_5
using Ionic.Zip;
#endif

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

#if !UNITY_2018 && !UNITY_2017 && !UNITY_5
using CompressionLevel = Ionic.Zlib.CompressionLevel;
#endif

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The CompressionHelper class
    /// </summary>
    public static class CompressionHelper
    {
#if !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// Zips the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Unzips the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        /// <exception cref="ArgumentException">Destination must be a folder. - destination</exception>
        public static void Unzip(string source, string destination)
        {
            if (!destination.IsDirectory())
                throw new ArgumentException("Destination must be a folder.", "destination");

            if (!Directory.Exists(destination))
                Directory.CreateDirectory(destination);

            using (ZipFile zipFile = ZipFile.Read(source))
                zipFile.ExtractAll(destination, ExtractExistingFileAction.OverwriteSilently);
        }

#endif

        /// <summary>
        /// Gets the name of the clean folder.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="filepath">The filepath.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Zips the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Unzips the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns></returns>
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