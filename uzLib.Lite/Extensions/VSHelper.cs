#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

using Onion.SolutionParser.Parser;
using Onion.SolutionParser.Parser.Model;
using System.Linq;

#endif

using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The VSHelper class
    /// </summary>
    public static class VSHelper
    {
#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5

        /// <summary>
        /// Gets the start name of up project.
        /// </summary>
        /// <param name="slnPath">The SLN path.</param>
        /// <returns></returns>
        /// <exception cref="Exception">Solution doesn't have an startup project set.</exception>
        public static string GetStartUpProjectName(string slnPath)
        {
            var startupProject = GetStartUpProject(slnPath);

            if (startupProject == null)
                throw new Exception("Solution doesn't have an startup project set.");

            return startupProject.Name;
        }

        /// <summary>
        /// Gets the start up project.
        /// </summary>
        /// <param name="slnPath">The SLN path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">slnPath</exception>
        /// <exception cref="ArgumentException">
        /// Requested SLN file doesn't exists. - slnPath
        /// or
        /// Specified argument must be a file path with extension SLN. - slnPath
        /// </exception>
        /// <exception cref="Exception">Couldn't find .suo file with the solution path specified.</exception>
        public static Project GetStartUpProject(string slnPath)
        {
            if (string.IsNullOrEmpty(slnPath))
                throw new ArgumentNullException("slnPath");

            if (!File.Exists(slnPath))
                throw new ArgumentException("Requested SLN file doesn't exists.", "slnPath");

            var fileInfo = new FileInfo(slnPath);

            if (fileInfo.Extension.ToLowerInvariant() != ".sln")
                throw new ArgumentException("Specified argument must be a file path with extension SLN.", "slnPath");

            var matches = Directory.GetFiles(Path.GetDirectoryName(slnPath), "*.suo", SearchOption.AllDirectories);

            if (matches.IsNullOrEmpty())
                throw new Exception("Couldn't find .suo file with the solution path specified.");

            string suoFilePath = matches[0];
            var startupGuid = ReadStartupOptions(suoFilePath);

            var solution = SolutionParser.Parse(slnPath);

            return solution.Projects.FirstOrDefault(p => p.Guid == startupGuid);
        }

#endif

        private static string GetProjectNameFromGuid(FileInfo solutionFile, string guid)
        {
            string projectName = null;

            using (var reader = new StreamReader(solutionFile.FullName))
            {
                string line;

                bool found = false;

                while ((line = reader.ReadLine()) != null && !found)
                {
                    if (line.IndexOf(guid.ToUpper()) > -1 && line.Contains(",") && line.Contains("="))
                    {
                        projectName = line.Split(',')[1].Split(',')[0];

                        found = true;
                    }
                }
            }

            return projectName;
        }

        // Thanks to: https://stackoverflow.com/questions/8817693/how-do-i-programmatically-find-out-the-action-of-each-startup-project-in-a-solut
        /*
         For VS2017:

         - The token has to be: "StartupProject\0="
         - The 'Array.Copy' needs a shift of 4: Array.Copy(bytes, i2 +4+ tokenBytes.Length + 2, guidBytes, 0, guidBytes.Length);
         */

        /// <summary>
        /// Reads the startup options.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">No file selected. - filePath</exception>
        private static Guid ReadStartupOptions(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("No file selected.", "filePath");

            const string token = "StartupProject\0=&\0"; // VS2015
            // const string token = "StartupProject\0=";
            // const string token = "dwStartupOpt\0=";

            const string regexpGroup = "GUID";
            string regexpToken = $@"StartupProject.+?\{{(?<{regexpGroup}>(.+?))\}};";

            byte[] tokenBytes = Encoding.Unicode.GetBytes(token);

            byte[] bytes;

            using (var stream = new MemoryStream())
            {
                ExtractStream(filePath, "SolutionConfiguration", stream);

                bytes = stream.ToArray();
            }

            string headingStr = Encoding.Unicode.GetString(bytes);

            return new Guid(Regex.Match(headingStr, regexpToken).Groups[regexpGroup].Value);
        }

        /// <summary>
        /// Extracts the stream.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="streamName">Name of the stream.</param>
        /// <param name="output">The output.</param>
        /// <exception cref="ArgumentNullException">
        /// filePath
        /// or
        /// streamName
        /// or
        /// output
        /// </exception>
        /// <exception cref="Win32Exception">
        /// </exception>
        private static void ExtractStream(string filePath, string streamName, Stream output)
        {
            if (string.IsNullOrEmpty(filePath)) throw new ArgumentNullException("filePath");

            if (string.IsNullOrEmpty(streamName)) throw new ArgumentNullException("streamName");

            if (output == null) throw new ArgumentNullException("output");

            IStorage storage;
            int hr = StgOpenStorage(filePath, null, STGM.READ | STGM.SHARE_DENY_WRITE, IntPtr.Zero, 0, out storage);
            if (hr != 0) throw new Win32Exception(hr);

            try
            {
                IStream stream;
                hr = storage.OpenStream(streamName, IntPtr.Zero, STGM.READ | STGM.SHARE_EXCLUSIVE, 0, out stream);
                if (hr != 0) throw new Win32Exception(hr);

                int read = 0;
                IntPtr readPtr = Marshal.AllocHGlobal(Marshal.SizeOf(read));
                try
                {
                    var bytes = new byte[0x1000];
                    do
                    {
                        stream.Read(bytes, bytes.Length, readPtr);
                        read = Marshal.ReadInt32(readPtr);
                        if (read == 0) break;

                        output.Write(bytes, 0, read);
                    } while (true);
                }
                finally
                {
                    Marshal.FreeHGlobal(readPtr);
                    Marshal.ReleaseComObject(stream);
                }
            }
            finally
            {
                Marshal.ReleaseComObject(storage);
            }
        }

        /// <summary>
        /// STGs the open storage.
        /// </summary>
        /// <param name="pwcsName">Name of the PWCS.</param>
        /// <param name="pstgPriority">The PSTG priority.</param>
        /// <param name="grfMode">The GRF mode.</param>
        /// <param name="snbExclude">The SNB exclude.</param>
        /// <param name="reserved">The reserved.</param>
        /// <param name="ppstgOpen">The PPSTG open.</param>
        /// <returns></returns>
        [DllImport("ole32.dll")]
        private static extern int StgOpenStorage([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IStorage pstgPriority, STGM grfMode, IntPtr snbExclude, uint reserved, out IStorage ppstgOpen);

        #region Nested type: IStorage

        /// <summary>
        /// The IStorage class
        /// </summary>
        [ComImport, Guid("0000000b-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IStorage
        {
            void Unimplemented0();

            [PreserveSig]
            int OpenStream([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IntPtr reserved1, STGM grfMode, uint reserved2, out IStream ppstm);

            // other methods not declared for simplicity
        }

        #endregion Nested type: IStorage

        #region Nested type: STGM

        /// <summary>
        /// The STGM class
        /// </summary>
        [Flags]
        private enum STGM
        {
            /// <summary>
            /// The read
            /// </summary>
            READ = 0x00000000,

            /// <summary>
            /// The share deny write
            /// </summary>
            SHARE_DENY_WRITE = 0x00000020,

            /// <summary>
            /// The share exclusive
            /// </summary>
            SHARE_EXCLUSIVE = 0x00000010,

            // other values not declared for simplicity
        }

        #endregion Nested type: STGM
    }
}