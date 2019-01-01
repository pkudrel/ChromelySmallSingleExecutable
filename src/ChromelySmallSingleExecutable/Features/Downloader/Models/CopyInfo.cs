namespace ChromelySmallSingleExecutable.Features.Downloader.Models
{
    public class CopyInfo
    {
        public CopyInfo(string src, string dst)
        {
            Src = src;
            Dst = dst;
        }

        public string Src { get; set; }
        public string Dst { get; set; }
    }
}