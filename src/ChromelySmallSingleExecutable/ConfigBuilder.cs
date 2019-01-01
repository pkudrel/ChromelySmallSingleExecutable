using System.Collections.Generic;
using ChromelySmallSingleExecutable.Features.App.Models;
using ChromelySmallSingleExecutable.Features.Downloader.Models;

namespace ChromelySmallSingleExecutable
{
    public static class ConfigBuilder
    {
        public static Config Create()
        {
            var ret = new Config { PackageConfig = GetPackageConfig() };
            return ret;
        }

        private static PackageConfig GetPackageConfig()
        {
            return new PackageConfig
            {
                Name = "cefsharp_67.0.0_x64",
                Nugets = new List<NugetInfo>
                {
                    new NugetInfo("CefSharp.Common", "67.0.0",
                        new List<CopyInfo> {new CopyInfo("/CefSharp/x64", "/")}),
                    new NugetInfo("cef.redist.x64", "3.3396.1786",
                        new List<CopyInfo>
                        {
                            new CopyInfo("/CEF", "/"),
                            new CopyInfo("/CEF/locales", "/locales"),
                            new CopyInfo("/CEF/swiftshader", "/swiftshader"),
                        }),

                }
            };
        }
    }
}