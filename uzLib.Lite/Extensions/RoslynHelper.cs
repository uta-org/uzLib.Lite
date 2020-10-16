#if !UNITY_2020 && !UNITY_2019 && !UNITY_2018 && !UNITY_2017 && !UNITY_5
using Microsoft.Build.Evaluation;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    /// <summary>
    /// The RoslynHelper class
    /// </summary>
    public static class RoslynHelper
    {
        /// <summary>
        /// Adds the item.
        /// </summary>
        /// <param name="projPath">The proj path.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="saveFilePath">The save file path.</param>
        public static void AddItem(string projPath, string folderPath, string saveFilePath)
        {
            // Solved issue thanks to: https://stackoverflow.com/a/44260284/3286975

            Project project = new Project(projPath);

            string relPath = IOHelper.MakeRelativePath(folderPath, saveFilePath);
            if (!project.GetItems("Compile").Any(item => item.UnevaluatedInclude == relPath))
                project.AddItem("Compile", relPath);

            project.Save();
        }
    }
}

#endif