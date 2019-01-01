using System.Collections.Generic;

namespace ChromelySmallSingleExecutable.Features.Downloader.Models
{
    public class NugetInfo
    {
        public NugetInfo(string name, string version, List<CopyInfo> paths)
        {
            Name = name;
            Version = version;
            CopyPaths = paths;
        }

        public string Name { get; set; }
        public string Version { get; set; }

        public List<CopyInfo> CopyPaths { get; set; }
    }
}