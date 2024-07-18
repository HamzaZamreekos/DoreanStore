using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Services
{
    //public delegate void ProgressChangedHandler(long? totalFileSize, long totalBytesDownloaded, double? progressPercentage);

    public static class ApplicationState
    {
        public static HttpClientDownloadWithProgress DownloadClient { get; set; } = new();
        //public static ProgressChangedHandler ProgressEvent;
        public static event EventHandler ProgressEvent;
        public static long? totalFileSize { get; set; }
        public static long totalBytesDownloaded { get; set; }
        public static double? progressPercentage { get; set; }
        public static string? AppBeingdownloadedName{ get; set; }

        public static void InvokeProgressEvent()
        {
            ProgressEvent.Invoke(null, EventArgs.Empty);
        }
    }
}
