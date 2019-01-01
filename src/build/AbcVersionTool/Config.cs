using System.Collections.Generic;

namespace AbcVersionTool
{
    public class Config
    {
        public Dictionary<string,Branch> Branches { get; set; } = new Dictionary<string, Branch>();
    }

    public class Branch
    {
        public string Version { get; set; }
        public string ParentSha { get; set; }
    }
}