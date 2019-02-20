using Microsoft.Build.Evaluation;
using System.Linq;

namespace uzLib.Lite.Extensions
{
    public static class RoslynHelper
    {
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