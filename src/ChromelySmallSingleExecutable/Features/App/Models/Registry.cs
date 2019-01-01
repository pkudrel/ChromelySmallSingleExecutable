using System.IO;
using ChromelySmallSingleExecutable.Common;
using ChromelySmallSingleExecutable.Features.Bootstrap;
using ChromelySmallSingleExecutable.Features.Downloader.Models;

namespace ChromelySmallSingleExecutable.Features.App.Models
{
    public class Registry
    {
        public Registry(AppEnvironment env, Config config)
        {
            PackageConfig = config.PackageConfig;
            CefSharpPackageName = config.PackageConfig.Name;
            CefSharpEnvStorePath = Path.Combine(env.LocalApplicationData, "CefSharp", "packages");
            CefSharpEnvPath = Path.Combine(CefSharpEnvStorePath, config.PackageConfig.Name);
            CefSharpLocalePath = Path.Combine(CefSharpEnvPath, "locales");
            BrowserSubprocessPath = Path.Combine(CefSharpEnvPath, "CefSharp.BrowserSubprocess.exe");
            Io.CreateDirIfNotExist(CefSharpEnvStorePath);
        }

        public string CefSharpLocalePath { get; }
        public PackageConfig PackageConfig { get; }
        public string BrowserSubprocessPath { get; }
        public string CefSharpEnvPath { get; }
        public string CefSharpPackageName { get; }
        public string CefSharpEnvStorePath { get; }
    }
}