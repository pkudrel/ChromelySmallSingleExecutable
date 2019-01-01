using System;
using System.IO;
using System.IO.Compression;

namespace ChromelySmallSingleExecutable.Features.Downloader.Helpers
{
    public static class ExtractTool
    {
        public static void ExtractZipToDirectory(string zipFile, string dstDirectory)
        {
            ExtractZipToDirectoryImpl(zipFile, dstDirectory);
        }

        private static void ExtractZipToDirectoryImpl(string zipFile, string dstDirectory)
        {
            using (var source = ZipFile.OpenRead(zipFile))
            {
                var di = Directory.CreateDirectory(dstDirectory);
                var destinationDirectoryFullPath = di.FullName;


                foreach (var entry in source.Entries)
                {
                    var fileDestinationPath =
                        Path.GetFullPath(Path.Combine(destinationDirectoryFullPath, entry.FullName));

                    if (!fileDestinationPath.StartsWith(destinationDirectoryFullPath,
                        StringComparison.OrdinalIgnoreCase))
                        throw new IOException("File is extracting to outside of the folder specified.");

                    if (Path.GetFileName(fileDestinationPath).Length == 0)
                    {
                        if (entry.Length != 0)
                            throw new IOException("Directory entry with data.");

                        Directory.CreateDirectory(fileDestinationPath);
                    }
                    else
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(fileDestinationPath) ??
                                                  throw new InvalidOperationException());
                        entry.ExtractToFile(fileDestinationPath, true);
                    }
                }
            }
        }
    }
}