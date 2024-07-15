using Microsoft.Maui.Storage;
using Realms.Exceptions;
using Realms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Realms;
using DoreanStore.Models;
using System.Diagnostics;
using Newtonsoft.Json;

namespace DoreanStore.Services
{
    public class FdroidService
    {
        private readonly HttpClient httpClient;
        public FdroidService()
        {
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Downloads the index json from fdroid and saves it to a file
        /// </summary>
        /// <returns>Path to the saved file</returns>
        public async Task<string> DownloadIndex()
        {
            Debug.WriteLine("downloading index");
            string uri = @"https://f-droid.org/repo/index-v2.json";
            string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, "indexJson");
            if (System.IO.File.Exists(pathUri))
                return string.Empty;
            using (var s = await httpClient.GetStreamAsync(uri))
            {
                using (var fs = new FileStream(pathUri, FileMode.CreateNew))
                {
                    await s.CopyToAsync(fs);
                }
            }
            Debug.WriteLine("finished downloading");

            return pathUri;
        }
    }
}
