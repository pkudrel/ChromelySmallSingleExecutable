using System;
using System.IO;
using System.Reflection;
using ChromelySmallSingleExecutable.Common;

namespace ChromelySmallSingleExecutable.Features.Bootstrap
{
    public class AppEnvironmentBuilder
    {
        private static AppEnvironment _appEnvironmentValue;
        private static readonly object _padlock = new object();

        static AppEnvironmentBuilder()
        {
        }

        private AppEnvironmentBuilder()
        {
        }


        public static AppEnvironmentBuilder Instance { get; } = new AppEnvironmentBuilder();


        public AppEnvironment GetAppEnvironment()
        {
            if (_appEnvironmentValue == null)
                lock (_padlock)
                {
                    var asm = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();
                    if (_appEnvironmentValue != null) return _appEnvironmentValue;
                    _appEnvironmentValue = GetTemporaryRegistryImpl(asm);
                }

            return _appEnvironmentValue;
        }

        private AppEnvironment GetTemporaryRegistryImpl(Assembly asm)
        {
            var res = new AppEnvironment();
            res.AssemblyFilePath = new Uri(asm.CodeBase).LocalPath;
            res.ExeFileDir = Path.GetDirectoryName(res.AssemblyFilePath);
            res.LogDir = Path.Combine(res.ExeFileDir ?? throw new InvalidOperationException(), ".logs");
            res.LocalApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            res.AssemblyName = asm.GetName().Name;
            Io.CreateDirIfNotExist(res.LogDir);
            return res;
        }
    }
}