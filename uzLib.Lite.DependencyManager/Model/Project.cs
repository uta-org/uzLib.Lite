using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uzLib.Lite.DependencyManager.Model
{
    public class Project
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public Project(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}