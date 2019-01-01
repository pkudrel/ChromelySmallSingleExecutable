using System.IO;
using System.Windows.Forms;
using ChromelySmallSingleExecutable.Features.App.Models;
using ChromelySmallSingleExecutable.Features.Downloader.View;

namespace ChromelySmallSingleExecutable.Features.Downloader
{
    public class ProgramDownloader
    {
       public static void DownloadCefSharpEnvIfNeeded(Registry reg)
        {
            if (Directory.Exists(reg.CefSharpEnvPath)) return;
            BeginDownloadProcess(reg);
        }


       private static void BeginDownloadProcess(Registry reg)
       {

           Application.EnableVisualStyles();
           Application.SetCompatibleTextRenderingDefault(false);
           Application.Run(new DownloadForm(reg));
        }
    }
}