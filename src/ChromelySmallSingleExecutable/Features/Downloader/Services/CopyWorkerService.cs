using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ChromelySmallSingleExecutable.Common;
using ChromelySmallSingleExecutable.Features.Downloader.Models;

namespace ChromelySmallSingleExecutable.Features.Downloader.Services
{
    public class CopyWorkerService
    {
        private readonly ComposeSettings _composeSettings;
        private readonly Action<int> _progressFn;

        public CopyWorkerService(ComposeSettings composeSettings, Action<int> progressFn)
        {
            _composeSettings = composeSettings;
            _progressFn = progressFn;
        }


        private List<(string src, string dst)> GetFilesToCopy(NugetInfo nugetInfo)
        {
            var filesToCopy = new List<(string src, string dst)>();
            var dir = $"{nugetInfo.Name}.{nugetInfo.Version}";
            var extractionDir = Path.Combine(_composeSettings.TmpExtractionPath, dir);

            foreach (var copyPath in nugetInfo.CopyPaths)
            {
                var s = copyPath.Src.StartsWith("/") ? copyPath.Src.Substring(1) : copyPath.Src;
                var d = copyPath.Dst.StartsWith("/") ? copyPath.Dst.Substring(1) : copyPath.Dst;
                var src1 = Path.Combine(extractionDir, s);
                var dst1 = Path.Combine(_composeSettings.CefSharpEnvPath, d);
                Io.CreateDirIfNotExist(dst1);
                var files = GetFiles(src1);
                var copyList = CreateCopyList(src1, dst1, files);
                filesToCopy.AddRange(copyList);
            }

            return filesToCopy;
        }

        private List<(string src, string dst)> CreateCopyList(string srcDir, string dstDir, List<string> files)
        {
            var list = new List<(string src, string dst)>();

            foreach (var file in files)
            {
                var srcFullPath = Path.Combine(srcDir, file);
                var dst = Path.Combine(dstDir, file);
                list.Add((srcFullPath, dst));
            }

            return list;
        }

        private static List<string> GetFiles(string dir)
        {
            var list = new List<string>();
            var di = new DirectoryInfo(dir);
            list.AddRange(di.GetFiles().Select(x => x.Name));
            return list;
        }

        public async Task CopyOne(NugetInfo nugetProcessResult)
        {
            var list = GetFilesToCopy(nugetProcessResult);
            await Io.CopyFilesAsync(list, _progressFn);
        }
    }
}