using System;
using System.Net;
using System.Threading.Tasks;

namespace ChromelySmallSingleExecutable.Features.Downloader.Helpers
{
    public class DownloadTool
    {
        private readonly Action<int> _progressFn;

        public DownloadTool(Action<int> progressFn)
        {
            _progressFn = progressFn;
        }

        public async Task DownloadFileAsync(string fileUrl, string dst)
        {
            var downloadLink = new Uri(fileUrl);

            void DownloadProgressChangedEvent(object s, DownloadProgressChangedEventArgs e)
            {
                _progressFn(e.ProgressPercentage);
            }

            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += DownloadProgressChangedEvent;
                await webClient.DownloadFileTaskAsync(downloadLink, dst);
            }
        }
    }
}