using System;
using System.Collections.Generic;
using System.IO;
using uzLib.Lite.ExternalCode.Core;

// ReSharper disable CompareOfFloatsByEqualityOperator

namespace UnityEngine.UI
{
    /*
        File browser for selecting files or folders at runtime.
     */

    public enum FileBrowserType
    {
        File,
        Directory
    }

    // ReSharper disable once RedundantNameQualifier
    public class FileBrowser : uzLib.Lite.ExternalCode.Core.MonoSingleton<FileBrowser>
    {
        // Called when the user clicks cancel or select
        public delegate void FinishedCallback(string path);

        protected FileBrowserType m_browserType;

        protected FinishedCallback m_callback;

        private bool m_cancelled;

        protected GUIStyle m_centredText;

        protected string m_currentDirectory;

        protected bool m_currentDirectoryMatches;
        protected string[] m_currentDirectoryParts;

        protected string[] m_directories;
        protected GUIContent[] m_directoriesWithImages;

        protected Texture2D m_directoryImage;

        protected List<string> m_drives = new List<string>();

        protected Texture2D m_fileImage;

        protected string m_filePattern;

        protected string[] m_files;
        protected GUIContent[] m_filesWithImages;

        protected string m_name;
        protected string m_newDirectory;

        protected string[] m_nonMatchingDirectories;
        protected GUIContent[] m_nonMatchingDirectoriesWithImages;

        protected string[] m_nonMatchingFiles;
        protected GUIContent[] m_nonMatchingFilesWithImages;
        protected Rect m_screenRect;

        protected Vector2 m_scrollPosition;
        protected int m_selectedDirectory;
        protected int m_selectedFile;
        protected int m_selectedNonMatchingDirectory;

        private GUISkin s_Skin;

        /// <summary>
        /// Gets or sets the skin.
        /// </summary>
        /// <value>
        /// The skin.
        /// </value>
        public GUISkin Skin
        {
            get => s_Skin;
            set
            {
                s_Skin = value;
                UILayout.Skin = value;
            }
        }

        /// <summary>
        /// Gets or sets the button delegate.
        /// </summary>
        /// <value>
        /// The button delegate.
        /// </value>
        public UIUtils.ButtonDelegate ButtonDelegate { get; set; }

        /// <summary>
        /// Gets or sets the custom window.
        /// </summary>
        /// <value>
        /// The custom window.
        /// </value>
        public UIUtils.CustomWindowDelegate CustomWindow { get; set; } = UIUtils.CustomWindow;

        /// <summary>
        /// Prevents a default instance of the <see cref="FileBrowser"/> class from being created.
        /// </summary>
        private FileBrowser()
        {
        }

        // Defaults to working directory
        /// <summary>
        /// Gets or sets the current directory.
        /// </summary>
        /// <value>
        /// The current directory.
        /// </value>
        public string CurrentDirectory
        {
            get => m_currentDirectory;
            set
            {
                SetNewDirectory(value);
                SwitchDirectoryNow();
            }
        }

        // Optional pattern for filtering selectable files/folders. See:
        // http://msdn.microsoft.com/en-us/library/wz42302f(v=VS.90).aspx
        // and
        // http://msdn.microsoft.com/en-us/library/6ff71z1w(v=VS.90).aspx
        /// <summary>
        /// Gets or sets the selection pattern.
        /// </summary>
        /// <value>
        /// The selection pattern.
        /// </value>
        public string SelectionPattern
        {
            get => m_filePattern;
            set
            {
                m_filePattern = value;
                ReadDirectoryContents();
            }
        }

        /// <summary>
        /// Gets a value indicating whether [show file browser].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show file browser]; otherwise, <c>false</c>.
        /// </value>
        public bool ShowFileBrowser { get; private set; }

        /// <summary>
        /// Gets the current path.
        /// </summary>
        /// <value>
        /// The current path.
        /// </value>
        public string CurrentPath { get; private set; }

        // Optional image for directories
        /// <summary>
        /// Gets or sets the directory image.
        /// </summary>
        /// <value>
        /// The directory image.
        /// </value>
        public Texture2D DirectoryImage
        {
            get => m_directoryImage;
            set
            {
                m_directoryImage = value;
                BuildContent();
            }
        }

        // Optional image for files
        /// <summary>
        /// Gets or sets the file image.
        /// </summary>
        /// <value>
        /// The file image.
        /// </value>
        public Texture2D FileImage
        {
            get => m_fileImage;
            set
            {
                m_fileImage = value;
                BuildContent();
            }
        }

        // Browser type. Defaults to File, but can be set to Folder
        /// <summary>
        /// Gets or sets the type of the browser.
        /// </summary>
        /// <value>
        /// The type of the browser.
        /// </value>
        public FileBrowserType BrowserType
        {
            get => m_browserType;
            set
            {
                m_browserType = value;
                ReadDirectoryContents();
            }
        }

        /// <summary>
        /// Gets the centered text.
        /// </summary>
        /// <value>
        /// The centered text.
        /// </value>
        protected GUIStyle CenteredText
        {
            get
            {
                if (m_centredText == null)
                {
                    m_centredText = new GUIStyle(GUI.skin.label);
                    m_centredText.alignment = TextAnchor.MiddleLeft;
                    m_centredText.fixedHeight = GUI.skin.button.fixedHeight;
                }

                return m_centredText;
            }
        }

        /// <summary>
        /// Creates the specified file browser type.
        /// </summary>
        /// <param name="fileBrowserType">Type of the file browser.</param>
        /// <returns></returns>
        public static FileBrowser Create(FileBrowserType fileBrowserType = FileBrowserType.File)
        {
            return Create(Vector2.zero, Application.streamingAssetsPath, fileBrowserType);
        }

        /// <summary>
        /// Creates the specified path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="fileBrowserType">Type of the file browser.</param>
        /// <returns></returns>
        public static FileBrowser Create(string path, FileBrowserType fileBrowserType = FileBrowserType.File)
        {
            return Create(Vector2.zero, path, fileBrowserType);
        }

        /// <summary>
        /// Creates the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="path">The path.</param>
        /// <param name="fileBrowserType">Type of the file browser.</param>
        /// <returns></returns>
        public static FileBrowser Create(Vector2 size, string path,
            FileBrowserType fileBrowserType = FileBrowserType.File)
        {
            return Create(new Rect(Vector2.zero, size), path, fileBrowserType);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="path">The path.</param>
        /// <param name="fileBrowserType">Type of the file browser.</param>
        /// <returns></returns>
        public static FileBrowser Create(Rect rect, string path, FileBrowserType fileBrowserType = FileBrowserType.File)
        {
            //Debug.Log($"Is Instance Null?: {Instance.GetType().FullName}");

            if (m_Instance == null)
            {
                Instance = MonoSingleton<FileBrowser>.Create().Init(rect, "Select path to image", path);
                //Instance = new FileBrowser(rect, "Select path to image");
                Instance.BrowserType = FileBrowserType.Directory;
            }

            return Instance;
            //m_fileBrowser.OnGUI();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="delegate">The delegate.</param>
        /// <param name="skin">The skin.</param>
        /// <param name="path">The path.</param>
        /// <param name="browserType">Type of the browser.</param>
        /// <returns></returns>
        public static FileBrowser Create(UIUtils.ButtonDelegate @delegate, GUISkin skin, out string path, FileBrowserType browserType = FileBrowserType.Directory)
        {
            var myFileBrowser = Create(browserType);
            myFileBrowser.Skin = skin;
            myFileBrowser.ButtonDelegate = @delegate;

            path = myFileBrowser.Open();
            return myFileBrowser;
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public FileBrowser Init(string name, FinishedCallback callback = null)
        {
            return Init(Rect.zero, name, Application.streamingAssetsPath, callback);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public FileBrowser Init(string name, string path, FinishedCallback callback = null)
        {
            return Init(Rect.zero, name, path, callback);
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="screenRect">The screen rect.</param>
        /// <param name="name">The name.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public FileBrowser Init(Rect screenRect, string name, FinishedCallback callback = null)
        {
            return Init(screenRect, name, Application.streamingAssetsPath, callback);
        }

        // Browsers need at least a rect, name and callback
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        /// <param name="screenRect">The screen rect.</param>
        /// <param name="name">The name.</param>
        /// <param name="path">The path.</param>
        /// <param name="callback">The callback.</param>
        /// <returns></returns>
        public FileBrowser Init(Rect screenRect, string name, string path, FinishedCallback callback = null)
        {
            m_name = name;
            m_screenRect = screenRect == Rect.zero
                ? UIUtils.GetCenteredRect(500, 400)
                : screenRect.xMin == 0 && screenRect.yMin == 0
                    ? UIUtils.GetCenteredRect(screenRect.width, screenRect.height)
                    : screenRect;
            m_browserType = FileBrowserType.File;

            if (callback != null)
                m_callback = callback;

            SetNewDirectory(Directory.GetCurrentDirectory());

            m_newDirectory = path;
            SwitchDirectoryNow();

            return this;
        }

        /// <summary>
        /// Sets the new directory.
        /// </summary>
        /// <param name="directory">The directory.</param>
        protected void SetNewDirectory(string directory)
        {
            m_newDirectory = directory;
        }

        /// <summary>
        /// Switches the directory now.
        /// </summary>
        protected void SwitchDirectoryNow()
        {
            if (m_newDirectory == null || m_currentDirectory == m_newDirectory) return;

            m_currentDirectory = m_newDirectory;
            m_scrollPosition = Vector2.zero;
            m_selectedDirectory = m_selectedNonMatchingDirectory = m_selectedFile = -1;
            ReadDirectoryContents();
        }

        /// <summary>
        /// Reads the directory contents.
        /// </summary>
        protected void ReadDirectoryContents()
        {
            // refresh list of drives
            try
            {
                m_drives.Clear();
                m_drives.AddRange(Directory.GetLogicalDrives());
            }
            catch
            {
            }

            if (m_currentDirectory == "/")
            {
                m_currentDirectoryParts = new[] { "" };
                m_currentDirectoryMatches = false;
            }
            else
            {
                m_currentDirectoryParts = m_currentDirectory.Split(Path.DirectorySeparatorChar);
                if (SelectionPattern != null)
                {
                    var generation = GetDirectories(
                        Path.GetDirectoryName(m_currentDirectory),
                        SelectionPattern
                    );
                    m_currentDirectoryMatches = Array.IndexOf(generation, m_currentDirectory) >= 0;
                }
                else
                {
                    m_currentDirectoryMatches = false;
                }
            }

            if (BrowserType == FileBrowserType.File || SelectionPattern == null)
            {
                m_directories = GetDirectories(m_currentDirectory);
                m_nonMatchingDirectories = new string[0];
            }
            else
            {
                m_directories = GetDirectories(m_currentDirectory, SelectionPattern);
                var nonMatchingDirectories = new List<string>();
                foreach (var directoryPath in GetDirectories(m_currentDirectory))
                    if (Array.IndexOf(m_directories, directoryPath) < 0)
                        nonMatchingDirectories.Add(directoryPath);
                m_nonMatchingDirectories = nonMatchingDirectories.ToArray();
                for (var i = 0; i < m_nonMatchingDirectories.Length; ++i)
                {
                    var lastSeparator = m_nonMatchingDirectories[i].LastIndexOf(Path.DirectorySeparatorChar);
                    m_nonMatchingDirectories[i] = m_nonMatchingDirectories[i].Substring(lastSeparator + 1);
                }

                Array.Sort(m_nonMatchingDirectories);
            }

            for (var i = 0; i < m_directories.Length; ++i)
                m_directories[i] = m_directories[i]
                    .Substring(m_directories[i].LastIndexOf(Path.DirectorySeparatorChar) + 1);

            if (BrowserType == FileBrowserType.Directory || SelectionPattern == null)
            {
                m_files = GetFiles(m_currentDirectory);
                m_nonMatchingFiles = new string[0];
            }
            else
            {
                m_files = GetFiles(m_currentDirectory, SelectionPattern);
                var nonMatchingFiles = new List<string>();
                foreach (var filePath in GetFiles(m_currentDirectory))
                    if (Array.IndexOf(m_files, filePath) < 0)
                        nonMatchingFiles.Add(filePath);
                m_nonMatchingFiles = nonMatchingFiles.ToArray();
                for (var i = 0; i < m_nonMatchingFiles.Length; ++i)
                    m_nonMatchingFiles[i] = Path.GetFileName(m_nonMatchingFiles[i]);
                Array.Sort(m_nonMatchingFiles);
            }

            for (var i = 0; i < m_files.Length; ++i) m_files[i] = Path.GetFileName(m_files[i]);

            Array.Sort(m_files);

            BuildContent();

            m_newDirectory = null;
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        private static string[] GetFiles(string path, string searchPattern)
        {
            try
            {
                return Directory.GetFiles(path, searchPattern);
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string[] GetFiles(string path)
        {
            try
            {
                return Directory.GetFiles(path);
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="searchPattern">The search pattern.</param>
        /// <returns></returns>
        private static string[] GetDirectories(string path, string searchPattern)
        {
            try
            {
                return Directory.GetDirectories(path, searchPattern);
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private static string[] GetDirectories(string path)
        {
            try
            {
                return Directory.GetDirectories(path);
            }
            catch
            {
                return new string[0];
            }
        }

        /// <summary>
        /// Builds the content.
        /// </summary>
        protected void BuildContent()
        {
            m_directoriesWithImages = new GUIContent[m_directories.Length];
            for (var i = 0; i < m_directoriesWithImages.Length; ++i)
                m_directoriesWithImages[i] = new GUIContent(m_directories[i], DirectoryImage);
            m_nonMatchingDirectoriesWithImages = new GUIContent[m_nonMatchingDirectories.Length];
            for (var i = 0; i < m_nonMatchingDirectoriesWithImages.Length; ++i)
                m_nonMatchingDirectoriesWithImages[i] = new GUIContent(m_nonMatchingDirectories[i], DirectoryImage);
            m_filesWithImages = new GUIContent[m_files.Length];
            for (var i = 0; i < m_filesWithImages.Length; ++i)
                m_filesWithImages[i] = new GUIContent(m_files[i], FileImage);
            m_nonMatchingFilesWithImages = new GUIContent[m_nonMatchingFiles.Length];
            for (var i = 0; i < m_nonMatchingFilesWithImages.Length; ++i)
                m_nonMatchingFilesWithImages[i] = new GUIContent(m_nonMatchingFiles[i], FileImage);
        }

        /// <summary>
        /// Called when [GUI].
        /// </summary>
        private void OnGUI()
        {
            if (!ShowFileBrowser)
                return;

            if (Skin != null)
                GUI.skin = Skin;

            var e = Event.current;

            m_screenRect =
                CustomWindow?.Invoke(9999, m_screenRect, DrawWindow, m_name, () => SetCurrentPath(null))
                ?? GUI.Window(9999, m_screenRect, DrawWindow, m_name);

            if (e.type == EventType.Repaint) SwitchDirectoryNow();
        }

        /// <summary>
        /// Draws the window.
        /// </summary>
        /// <param name="windowID">The window identifier.</param>
        private void DrawWindow(int windowID)
        {
            var dragRect = new Rect(Vector2.zero, new Vector2(m_screenRect.width, 20));
            GUI.DragWindow(dragRect);

            // Important, this needs to be under DragWindow method call
            //DockWindow.IsHover = m_screenRect.Contains(GlobalInput.MousePosition);

            // display drives
            if (m_drives.Count > 0)
            {
                GUILayout.BeginHorizontal();

                foreach (var drive in m_drives)
                    if (ButtonDelegate?.Invoke(drive) ?? GUILayout.Button(drive))
                        SetNewDirectory(drive);

                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
            }

            // display directory parts
            GUILayout.BeginHorizontal();

            for (var parentIndex = 0; parentIndex < m_currentDirectoryParts.Length; ++parentIndex)
                if (parentIndex == m_currentDirectoryParts.Length - 1)
                {
                    GUILayout.Label(m_currentDirectoryParts[parentIndex], CenteredText);
                }
                else if (ButtonDelegate?.Invoke(m_currentDirectoryParts[parentIndex]) ?? GUILayout.Button(m_currentDirectoryParts[parentIndex]))
                {
                    var parentDirectoryName = m_currentDirectory;
                    for (var i = m_currentDirectoryParts.Length - 1; i > parentIndex; --i)
                        parentDirectoryName = Path.GetDirectoryName(parentDirectoryName);
                    SetNewDirectory(parentDirectoryName);
                }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();

            m_scrollPosition = GUILayout.BeginScrollView(
                m_scrollPosition,
                false,
                false, // fix
                GUI.skin.horizontalScrollbar,
                GUI.skin.verticalScrollbar,
                GUI.skin.box
            );
            m_selectedDirectory = UILayout.SelectionList(
                m_selectedDirectory,
                m_directoriesWithImages,
                DirectoryDoubleClickCallback
            );
            if (m_selectedDirectory > -1) m_selectedFile = m_selectedNonMatchingDirectory = -1;
            m_selectedNonMatchingDirectory = UILayout.SelectionList(
                m_selectedNonMatchingDirectory,
                m_nonMatchingDirectoriesWithImages,
                NonMatchingDirectoryDoubleClickCallback
            );
            if (m_selectedNonMatchingDirectory > -1) m_selectedDirectory = m_selectedFile = -1;
            GUI.enabled = BrowserType == FileBrowserType.File;
            m_selectedFile = UILayout.SelectionList(
                m_selectedFile,
                m_filesWithImages,
                FileDoubleClickCallback
            );
            GUI.enabled = true;
            if (m_selectedFile > -1) m_selectedDirectory = m_selectedNonMatchingDirectory = -1;
            GUI.enabled = false;
            UILayout.SelectionList(
                -1,
                m_nonMatchingFilesWithImages
            );
            GUI.enabled = true;
            GUILayout.EndScrollView();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (UIUtils.ButtonWithCalculatedSize("Cancel", ButtonDelegate))
                SetCurrentPath(null);

            if (BrowserType == FileBrowserType.File)
            {
                GUI.enabled = m_selectedFile > -1;
            }
            else
            {
                if (SelectionPattern == null)
                    GUI.enabled = true; //m_selectedDirectory > -1;
                else
                    GUI.enabled = m_selectedDirectory > -1 ||
                                  m_currentDirectoryMatches &&
                                  m_selectedNonMatchingDirectory == -1 &&
                                  m_selectedFile == -1;
            }

            var selectButtonText = BrowserType == FileBrowserType.File ? "Select" : "Select current folder";

            if (UIUtils.ButtonWithCalculatedSize(selectButtonText, ButtonDelegate))
            {
                if (BrowserType == FileBrowserType.File)
                {
                    var path = Path.Combine(m_currentDirectory, m_files[m_selectedFile]);
                    SetCurrentPath(path);
                }
                else
                {
                    //				if (m_selectedDirectory > -1) {
                    //					m_callback(Path.Combine(m_currentDirectory, m_directories[m_selectedDirectory]));
                    //				} else {
                    //					m_callback(m_currentDirectory);
                    //				}

                    SetCurrentPath(m_currentDirectory);
                }
            }

            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }

        /// <summary>
        /// Files the double click callback.
        /// </summary>
        /// <param name="i">The i.</param>
        protected void FileDoubleClickCallback(int i)
        {
            if (BrowserType == FileBrowserType.File)
                m_callback(Path.Combine(m_currentDirectory, m_files[i]));
        }

        /// <summary>
        /// Directories the double click callback.
        /// </summary>
        /// <param name="i">The i.</param>
        protected void DirectoryDoubleClickCallback(int i)
        {
            SetNewDirectory(Path.Combine(m_currentDirectory, m_directories[i]));
        }

        /// <summary>
        /// Nons the matching directory double click callback.
        /// </summary>
        /// <param name="i">The i.</param>
        protected void NonMatchingDirectoryDoubleClickCallback(int i)
        {
            SetNewDirectory(Path.Combine(m_currentDirectory, m_nonMatchingDirectories[i]));
        }

        /// <summary>
        /// Sets the current path.
        /// </summary>
        /// <param name="path">The path.</param>
        private void SetCurrentPath(string path = "")
        {
            if (path == null)
            {
                CurrentPath = null;
                m_callback?.Invoke(null);
                m_cancelled = true;
                return;
            }

            if (m_callback == null)
            {
                CurrentPath = path;
                ShowFileBrowser = false; // Then, close the browser
            }
            else
                m_callback?.Invoke(path);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        public string Open()
        {
            ShowFileBrowser = true;
            return null;
        }

        /// <summary>
        /// Determines whether this instance is cancelled.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is cancelled; otherwise, <c>false</c>.
        /// </returns>
        public bool IsCancelled()
        {
            if (m_cancelled)
            {
                ShowFileBrowser = false;
                m_cancelled = false;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Determines whether this instance is ready.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance is ready; otherwise, <c>false</c>.
        /// </returns>
        public bool IsReady()
        {
            var ready = !ShowFileBrowser && !string.IsNullOrEmpty(CurrentPath);

            if (IsCancelled())
                return true;

            if (ready)
                ShowFileBrowser = false;

            return ready;
        }
    }
}