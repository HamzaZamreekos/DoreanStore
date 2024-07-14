using DoreanStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Services
{
    public class IOService
    {
        private readonly JsonService jsonService;
        public IOService(JsonService jsonService)
        {
            this.jsonService = jsonService;
        }

        /// <summary>
        /// Opens the index json file as a stream, and uses the JsonService to deserialize it
        /// </summary>
        /// <returns>The deserialized index file</returns>
        public RepoResponse? DeserializeFdroidRepo()
        {
            RepoResponse response;
            string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, "indexJson");

            using (var fs = new FileStream(pathUri, FileMode.Open))
            {
                var deserializedResponse = jsonService.DeserializeFromStream<RepoResponse>(fs);
                response = deserializedResponse;
            }
            return response;
        }
        /// <summary>
        /// Copies files packaged with the application to its data directory
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public async Task CopyFileToAppDataDirectory(string filename, bool overwrite)
        {
            var newPath = Path.Combine(FileSystem.Current.AppDataDirectory, filename);
            if(!overwrite)
                if (System.IO.File.Exists(newPath))
                    return; 
            using Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
            using FileStream outputStream = System.IO.File.Create(newPath);
            await inputStream.CopyToAsync(outputStream);
        }
    }
}
