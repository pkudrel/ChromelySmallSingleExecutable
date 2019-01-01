using System;
using System.Collections.Generic;
using System.IO;
using ChromelySmallSingleExecutable.Features.App.Models;

namespace ChromelySmallSingleExecutable.Features.Downloader.Models
{
    public class ComposeSettings
    {
        public ComposeSettings(Registry registry)
        {
            TmpPath = Path.Combine(registry.CefSharpEnvStorePath, ".tmp");
            TmpDownloadPath = Path.Combine(TmpPath, "download");
            TmpExtractionPath = Path.Combine(TmpPath, "extraction");
            Nugets = registry.PackageConfig.Nugets;
            LocalNugetSourcePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                ".nuget\\packages");
            UseLocalNugetSource = true;
            CefSharpEnvPath = registry.CefSharpEnvPath;
        }

        public string TmpPath { get; }
        public string CefSharpEnvPath { get; }
        public bool UseLocalNugetSource { get; }
        public string LocalNugetSourcePath { get; }
        public List<NugetInfo> Nugets { get; }
        public string TmpDownloadPath { get; }
        public string TmpExtractionPath { get; }
    }
}