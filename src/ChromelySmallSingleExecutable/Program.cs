using System;
using System.IO;
using System.Reflection;
using Chromely.CefSharp.Winapi.BrowserWindow;
using Chromely.Core;
using Chromely.Core.Helpers;
using Chromely.Core.Host;
using ChromelySmallSingleExecutable.Features.App.Models;
using ChromelySmallSingleExecutable.Features.Bootstrap;
using ChromelySmallSingleExecutable.Features.Downloader;

namespace ChromelySmallSingleExecutable
{
    internal class Program
    {
        private static readonly AppEnvironment _env = AppEnvironmentBuilder.Instance.GetAppEnvironment();
        private static readonly Config _cnf = ConfigBuilder.Create();
        private static readonly Registry _reg = new Registry(_env, _cnf);

        private static int Main(string[] args)
        {
            Init();
            return Start(args);
        }

        private static int Start(string[] args)
        {
            const string startUrl = "https://google.com";

            var config = ChromelyConfiguration
                .Create()
                .WithCustomSetting(CefSettingKeys.BrowserSubprocessPath, _reg.BrowserSubprocessPath)
                .WithCustomSetting(CefSettingKeys.LocalesDirPath, _reg.CefSharpLocalePath)
                .WithCustomSetting(CefSettingKeys.LogFile, ".logs\\chronium.log")
                .UseDefaultLogger(".logs\\chromely.log")
                .WithHostMode(WindowState.Normal)
                .WithHostTitle("ChromelySmallSingleExecutable")
                .WithAppArgs(args)
                .WithHostSize(1100, 700)
                .WithStartUrl(startUrl);


            using (var window = new CefSharpBrowserWindow(config))
            {
                return window.Run(args);
            }
        }

        private static void Init()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            ProgramDownloader.DownloadCefSharpEnvIfNeeded(_reg);
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dll = args.Name.Split(new[] {','}, 2)[0] + ".dll";
            switch (dll)
            {
                case "CefSharp.Core.dll":
                case "CefSharp.dll":
                    var path = Path.Combine(_reg.CefSharpEnvPath, dll);
                    var asm = Assembly.LoadFile(path);
                    return asm;
                default:
                    return null;
            }
        }
    }
}