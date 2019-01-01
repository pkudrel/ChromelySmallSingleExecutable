using System.IO;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.IO;

namespace Helpers
{
    [PublicAPI]
    public static class Downloader
    {
        public static void DownloadIfNotExists(string src, string dst, string label = null)
        {
            var textLabel = string.IsNullOrEmpty(label) ? string.Empty : $"{label}; ";
            if (File.Exists(dst) == false)
            {
                Logger.Info($"{textLabel}File do not exists; Downloading; Src: {src}; Dst: {dst}");
                HttpTasks.HttpDownloadFile(src, dst);
            }
            else
            {
                Logger.Info($"{textLabel}File exists; Path: {dst}");
            }
        }
    }
}