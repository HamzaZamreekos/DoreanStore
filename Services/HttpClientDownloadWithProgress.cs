using MudBlazor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Services
{
    public class HttpClientDownloadWithProgress : IDisposable
    {
        public string downloadUrl;
        private string _destinationFilePath;
        public bool Downloading { get; set; } = false;
        private HttpClient _httpClient;

        public async Task StartDownload(string downloadUrl, string destinationFilePath)
        {

            Debug.WriteLine("starting download");
            this.downloadUrl = downloadUrl;
            _destinationFilePath = destinationFilePath;
            _httpClient = new HttpClient { Timeout = TimeSpan.FromDays(1) };
            try
            {
                using (var response = await _httpClient.GetAsync(downloadUrl, HttpCompletionOption.ResponseHeadersRead))
                    await DownloadFileFromHttpResponseMessage(response);
            }
            catch (Exception ex) { snackbar.Add("Download failed", Severity.Error); }

        }

        private async Task DownloadFileFromHttpResponseMessage(HttpResponseMessage response)
        {
            Debug.WriteLine(" download request sent");

            Downloading = true;
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength;

            using (var contentStream = await response.Content.ReadAsStreamAsync())
                await ProcessContentStream(totalBytes, contentStream);
        }

        private async Task ProcessContentStream(long? totalDownloadSize, Stream contentStream)
        {
            Debug.WriteLine("reading stream");

            var totalBytesRead = 0L;
            var readCount = 0L;
            var buffer = new byte[2000];
            var isMoreToRead = true;

            using (var fileStream = new FileStream(_destinationFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 2000, true))
            {
                do
                {
                    var bytesRead = await contentStream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        isMoreToRead = false;
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                        continue;
                    }
                    Debug.WriteLine("ewwwwwwwwwe");

                    await fileStream.WriteAsync(buffer, 0, bytesRead);

                    totalBytesRead += bytesRead;
                    readCount += 1;

                    if (readCount % 5 == 0)
                        TriggerProgressChanged(totalDownloadSize, totalBytesRead);
                }
                while (isMoreToRead);
                Downloading = false;
            }
        }

        private void TriggerProgressChanged(long? totalDownloadSize, long totalBytesRead)
        {
            Debug.WriteLine("Should trigger event rn my brother");
            Debug.WriteLine("event is NOT null baby");

            double? progressPercentage = null;
            if (totalDownloadSize.HasValue)
                progressPercentage = Math.Round((double)totalBytesRead / totalDownloadSize.Value * 100, 2);
            ApplicationState.progressPercentage = progressPercentage;
            ApplicationState.totalBytesDownloaded = totalBytesRead;
            ApplicationState.totalFileSize = totalDownloadSize;
            ApplicationState.InvokeProgressEvent();

        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}

