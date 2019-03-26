using Onion.SolutionParser.Parser;
using Onion.SolutionParser.Parser.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Text.RegularExpressions;

namespace uzLib.Lite.Extensions
{
    public static class VSHelper
    {
        //public static FileInfo GetStartUpProject(string slnPath)
        //{
        //    if (string.IsNullOrEmpty(slnPath))
        //        throw new ArgumentNullException("slnPath");

        //    if (!File.Exists(slnPath))
        //        throw new ArgumentException("Requested SLN file doesn't exists.", "slnPath");

        //    var fileInfo = new FileInfo(slnPath);

        //    if (fileInfo.Extension.ToLowerInvariant() != ".sln")
        //        throw new ArgumentException("Specified argument must be a file path with extension SLN.", "slnPath");

        //    return GetStartUpProject(fileInfo);
        //}

        //public static FileInfo GetStartUpProject(FileInfo solutionFile)

        public static string GetStartUpProjectName(string slnPath)
        {
            var startupProject = GetStartUpProject(slnPath);

            if (startupProject == null)
                throw new Exception("Solution doesn't have an startup project set.");

            return startupProject.Name;
        }

        public static Project GetStartUpProject(string slnPath)
        {
            if (string.IsNullOrEmpty(slnPath))
                throw new ArgumentNullException("slnPath");

            if (!File.Exists(slnPath))
                throw new ArgumentException("Requested SLN file doesn't exists.", "slnPath");

            var fileInfo = new FileInfo(slnPath);

            if (fileInfo.Extension.ToLowerInvariant() != ".sln")
                throw new ArgumentException("Specified argument must be a file path with extension SLN.", "slnPath");

            //FileInfo startUpProject = null;

            //string projectName = Path.GetFileNameWithoutExtension(solutionFile.FullName);

            //FileInfo suoFileInfo = new FileInfo(Path.Combine(solutionFile.Directory.FullName, string.Format(projectName + "{0}", ".suo")));

            var matches = Directory.GetFiles(Path.GetDirectoryName(slnPath), "*.suo", SearchOption.AllDirectories);

            if (matches.IsNullOrEmpty())
                throw new Exception("Couldn't find .suo file with the solution path specified.");

            string suoFilePath = matches[0];
            var startupGuid = ReadStartupOptions(suoFilePath);

            var solution = SolutionParser.Parse(slnPath);

            //var testGuid = StartupOptions.ReadStartupOptions(suoFilePath);

            //if (!string.IsNullOrEmpty(guid))
            //{
            //    string projectname = GetProjectNameFromGuid(solutionFile, guid).Trim().TrimStart('\"').TrimEnd('\"');
            //    startUpProject = new FileInfo(Path.Combine(solutionFile.DirectoryName, projectname));
            //}

            return solution.Projects.FirstOrDefault(p => p.Guid == startupGuid);
        }

        private static string GetProjectNameFromGuid(FileInfo solutionFile, string guid)
        {
            string projectName = null;

            using (var reader = new StreamReader(solutionFile.FullName))
            {
                string line;

                bool found = false;

                while ((line = reader.ReadLine()) != null && !found)
                {
                    if ((line.IndexOf(guid.ToUpper()) > -1) && line.Contains(",") && line.Contains("="))
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

        private static Guid ReadStartupOptions(string filePath)
        {
            //Guid guid = new Guid();

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

            //var guidBytes = new byte[36 * 2];

            //for (int i2 = 0; i2 < bytes.Length; i2++)
            //    if (bytes.Skip(i2).Take(tokenBytes.Length).SequenceEqual(tokenBytes))
            //    {
            //        Array.Copy(bytes, i2 + tokenBytes.Length + 2, guidBytes, 0, guidBytes.Length); // V2015
            //        // Array.Copy(bytes, i2 + 4 + tokenBytes.Length + 2, guidBytes, 0, guidBytes.Length);

            //        guid = new Guid(Encoding.Unicode.GetString(guidBytes));

            //        break;
            //    }

            return new Guid(Regex.Match(headingStr, regexpToken).Groups[regexpGroup].Value);
        }

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

        [DllImport("ole32.dll")]
        private static extern int StgOpenStorage([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IStorage pstgPriority, STGM grfMode, IntPtr snbExclude, uint reserved, out IStorage ppstgOpen);

        #region Nested type: IStorage

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

        [Flags]
        private enum STGM
        {
            READ = 0x00000000,
            SHARE_DENY_WRITE = 0x00000020,
            SHARE_EXCLUSIVE = 0x00000010,
            // other values not declared for simplicity
        }

        #endregion Nested type: STGM
    }

    public static class StartupOptions
    {
        public static IDictionary<Guid, int> ReadStartupOptions(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            // look for this token in the file
            const string token = "dwStartupOpt\0=";
            byte[] tokenBytes = Encoding.Unicode.GetBytes(token);
            Dictionary<Guid, int> dic = new Dictionary<Guid, int>();
            byte[] bytes;
            using (MemoryStream stream = new MemoryStream())
            {
                CompoundFileUtilities.ExtractStream(filePath, "SolutionConfiguration", stream);
                bytes = stream.ToArray();
            }

            int i = 0;
            do
            {
                bool found = true;
                for (int j = 0; j < tokenBytes.Length; j++)
                {
                    if (bytes[i + j] != tokenBytes[j])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    // back read the corresponding project guid
                    // guid is formatted as {guid}
                    // len to read is Guid length* 2 and there are two offset bytes between guid and startup options token
                    byte[] guidBytes = new byte[38 * 2];
                    Array.Copy(bytes, i - guidBytes.Length - 2, guidBytes, 0, guidBytes.Length);
                    Guid guid = new Guid(Encoding.Unicode.GetString(guidBytes));

                    // skip VT_I4
                    int options = BitConverter.ToInt32(bytes, i + tokenBytes.Length + 2);
                    dic[guid] = options;
                }
                i++;
            }
            while (i < bytes.Length);
            return dic;
        }
    }

    public static class CompoundFileUtilities
    {
        public static void ExtractStream(string filePath, string streamName, string streamPath)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            if (streamName == null)
                throw new ArgumentNullException("streamName");

            if (streamPath == null)
                throw new ArgumentNullException("streamPath");

            using (FileStream output = new FileStream(streamPath, FileMode.Create))
            {
                ExtractStream(filePath, streamName, output);
            }
        }

        public static void ExtractStream(string filePath, string streamName, Stream output)
        {
            if (filePath == null)
                throw new ArgumentNullException("filePath");

            if (streamName == null)
                throw new ArgumentNullException("streamName");

            if (output == null)
                throw new ArgumentNullException("output");

            IStorage storage;
            int hr = StgOpenStorage(filePath, null, STGM.READ | STGM.SHARE_DENY_WRITE, IntPtr.Zero, 0, out storage);
            if (hr != 0)
                throw new Win32Exception(hr);

            try
            {
                IStream stream;
                hr = storage.OpenStream(streamName, IntPtr.Zero, STGM.READ | STGM.SHARE_EXCLUSIVE, 0, out stream);
                if (hr != 0)
                    throw new Win32Exception(hr);

                int read = 0;
                IntPtr readPtr = Marshal.AllocHGlobal(Marshal.SizeOf(read));
                try
                {
                    byte[] bytes = new byte[0x1000];
                    do
                    {
                        stream.Read(bytes, bytes.Length, readPtr);
                        read = Marshal.ReadInt32(readPtr);
                        if (read == 0)
                            break;

                        output.Write(bytes, 0, read);
                    }
                    while (true);
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

        [ComImport, Guid("0000000b-0000-0000-C000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IStorage
        {
            void Unimplemented0();

            [PreserveSig]
            int OpenStream([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IntPtr reserved1, STGM grfMode, uint reserved2, out IStream ppstm);

            // other methods not declared for simplicity
        }

        [Flags]
        private enum STGM
        {
            READ = 0x00000000,
            SHARE_DENY_WRITE = 0x00000020,
            SHARE_EXCLUSIVE = 0x00000010,
            // other values not declared for simplicity
        }

        [DllImport("ole32.dll")]
        private static extern int StgOpenStorage([MarshalAs(UnmanagedType.LPWStr)] string pwcsName, IStorage pstgPriority, STGM grfMode, IntPtr snbExclude, uint reserved, out IStorage ppstgOpen);
    }
}