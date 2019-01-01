using System;
using System.IO;
using System.Threading.Tasks;
using ChromelySmallSingleExecutable.Common;
using ChromelySmallSingleExecutable.Features.App.Models;
using ChromelySmallSingleExecutable.Features.Downloader.Helpers;
using ChromelySmallSingleExecutable.Features.Downloader.Models;

namespace ChromelySmallSingleExecutable.Features.Downloader.Services
{
    public class CefSharpEnvBuilder
    {
        private readonly Action<string> _logFn;
        private readonly Action<int> _progressFn;
        private readonly Registry _registry;

        public CefSharpEnvBuilder(Registry registry, Action<string> logFn, Action<int> progressFn)
        {
            _registry = registry;
            _logFn = logFn;
            _progressFn = progressFn;
        }

        public async Task Do()
        {
            _logFn($"Begin create package: {_registry.CefSharpPackageName}");
            var settings = new ComposeSettings(_registry);
            Init(settings);
            await StepGetNugetPackages(settings);
            StepExtractNugets(settings);
            await StepCopyFiles(settings);
            Clean(settings);
            _logFn("Done");
        }


        private static void Init(ComposeSettings settings)
        {
            Io.CreateDirIfNotExist(settings.CefSharpEnvPath);
            Io.CreateDirIfNotExist(settings.TmpDownloadPath);
            Io.CreateDirIfNotExist(settings.TmpExtractionPath);
        }


        private static void Clean(ComposeSettings settings)
        {
            Io.RemoveFolder(settings.TmpDownloadPath);
            Io.RemoveFolder(settings.TmpExtractionPath);
            Io.RemoveFolder(settings.TmpPath);
        }

        private async Task StepGetNugetPackages(ComposeSettings settings)
        {
            foreach (var p in settings.Nugets)
            {
                var file = $"{p.Name}.{p.Version}.nupkg";
                var path = Path.Combine(settings.LocalNugetSourcePath, p.Name, p.Version);
                var local = Path.Combine(path, file);
                if (File.Exists(local) && settings.UseLocalNugetSource)
                {
                    _logFn($"Copy '{p.Name}' from local repository");
                    var dst = Path.Combine(settings.TmpDownloadPath, file);
                    await Io.CopyFileAsync(local, dst);
                }
                else
                {
                    _logFn($"Download '{p.Name}' from nuget repository");
                    await DownloadOneNuget(settings, p, file);
                }
            }
        }

        private void StepExtractNugets(ComposeSettings settings)
        {
            foreach (var p in settings.Nugets)
            {
                var dir = $"{p.Name}.{p.Version}";
                var file = $"{dir}.nupkg";
                var src = Path.Combine(settings.TmpDownloadPath, file);
                var dst = Path.Combine(settings.TmpExtractionPath, dir);
                Io.CreateDirIfNotExist(dst);
                _logFn($"Extract '{p.Name}'");
                ExtractTool.ExtractZipToDirectory(src, dst);
            }
        }

        private async Task StepCopyFiles(ComposeSettings settings)
        {
            Io.CreateDirIfNotExist(settings.CefSharpEnvPath);
            var copyWorker = new CopyWorkerService(settings, _progressFn);
            foreach (var p in settings.Nugets)
            {
                _logFn($"Copy '{p.Name}'");
                await copyWorker.CopyOne(p);
            }
        }

        private async Task DownloadOneNuget(ComposeSettings settings, NugetInfo n, string fileName)
        {
            var dl = new DownloadTool(_progressFn);
            var url = $"https://www.nuget.org/api/v2/package/{n.Name}/{n.Version}";
            var dstFile = Path.Combine(settings.TmpDownloadPath, fileName);
            if (File.Exists(dstFile) == false)
                await dl.DownloadFileAsync(url, dstFile);
        }
    }
}