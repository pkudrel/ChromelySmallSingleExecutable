using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using Nuke.Common;
using Nuke.Common.Tooling;

namespace Helpers
{
    public class LocalRunner
    {
        readonly string ToolPath;

        public LocalRunner(string toolPath)
        {
            ToolPath = toolPath;
        }


        public List<Output> Run(
            [CanBeNull] string arguments,
            [CanBeNull] string workingDirectory,
            int? timeout = null,
            bool logOutput = true,
            Func<string, LogLevel> logLevelParser = null,
            Func<string, string> outputFilter = null)
        {
            var result = StartProcessInternal(ToolPath,
                arguments,
                workingDirectory,
                timeout,
                logOutput,
                logLevelParser,
                outputFilter);
            result.WaitForExit();
            var outPut = result.Output;
            return outPut.ToList();
        }


        public IProcess StartProcess(
            [CanBeNull] string arguments,
            [CanBeNull] string workingDirectory,
            int? timeout = null,
            bool logOutput = true,
            Func<string, LogLevel> logLevelParser = null,
            Func<string, string> outputFilter = null) =>
            StartProcessInternal(ToolPath,
                arguments,
                workingDirectory,
                timeout,
                logOutput,
                logLevelParser,
                outputFilter);

        public static IProcess StartProcessInternal(
            string toolPath,
            [CanBeNull] string arguments,
            [CanBeNull] string workingDirectory,
            int? timeout,
            bool logOutput,
            [CanBeNull] Func<string, LogLevel> logLevelParser,
            [CanBeNull] Func<string, string> outputFilter)
        {
            ControlFlow.Assert(workingDirectory == null || Directory.Exists(workingDirectory),
                $"WorkingDirectory '{workingDirectory}' does not exist.");

            var startInfo = new ProcessStartInfo
            {
                FileName = toolPath,
                Arguments = arguments ?? string.Empty,
                WorkingDirectory = workingDirectory ?? string.Empty,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };


            var process = Process.Start(startInfo);
            if (process == null)
                return null;

            var output = GetOutputCollection(process, logOutput, logLevelParser, outputFilter);
            return new Process2(process, outputFilter, timeout, output);
        }

        static BlockingCollection<Output> GetOutputCollection(
            Process process,
            bool logOutput,
            Func<string, LogLevel> logLevelParser,
            Func<string, string> outputFilter)
        {
            var output = new BlockingCollection<Output>();
            logLevelParser = logLevelParser ?? (x => LogLevel.Information);

            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data == null)
                    return;

                output.Add(new Output {Text = e.Data, Type = OutputType.Std});

                if (logOutput)
                {
                    var logLevel = logLevelParser(e.Data);
                    var text = outputFilter(e.Data);
                    switch (logLevel)
                    {
                        case LogLevel.Trace:
                            Logger.Trace(text);
                            break;
                        case LogLevel.Information:
                            Logger.Info(text);
                            break;
                        case LogLevel.Warning:
                            Logger.Warn(text);
                            break;
                        case LogLevel.Error:
                            Logger.Error(text);
                            break;
                    }
                }
            };
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data == null)
                    return;

                output.Add(new Output {Text = e.Data, Type = OutputType.Err});

                if (logOutput)
                    Logger.Error(outputFilter(e.Data));
            };

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return output;
        }
    }
}