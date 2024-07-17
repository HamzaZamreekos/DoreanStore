using DoreanStore.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.Maui;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#if ANDROID
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using System.Reflection;
#endif

namespace DoreanStore.Services
{

    [Activity(Label = "App updater", LaunchMode = LaunchMode.SingleTop)]
    public class IOService : Android.App.Activity
    {
        private readonly JsonService jsonService;
        //public IOService(JsonService jsonService)
        //{
        //    this.jsonService = jsonService;
        //}
        /// <summary>
        /// Requests permissions to install apps if persmission wasn't granted before, Then installs the apk
        /// </summary>
        /// <param name="name"></param>
        public void InstallApk(string apkFileName)
        {
#if ANDROID
            string apkFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, apkFileName);
            Java.IO.File apkFile = new Java.IO.File(apkFilePath);

            // Use the FileProvider to get a content URI for the file
            Android.Net.Uri apkUri = FileProvider.GetUriForFile(Android.App.Application.Context, "[Project.Application].fileprovider", apkFile);

            // Create an Intent to install the APK
            Intent intent = new Intent(Intent.ActionView);
            intent.SetDataAndType(apkUri, "application/vnd.android.package-archive");
            intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.GrantReadUriPermission);

            // Start the installation process
            Android.App.Application.Context.StartActivity(intent);
#endif
        }
        /// <summary>
        /// Opens the index json file as a stream, and uses the JsonService to deserialize it asynchronously
        /// </summary>
        /// <returns>The deserialized index file</returns>
        public async Task<RepoResponse?> DeserializeFdroidRepoAsync()
        {
            //Debug.WriteLine("deserialzing");

            RepoResponse response;
            string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, "indexJson");

            using (var fs = new FileStream(pathUri, FileMode.Open))
            {
                var deserializedResponse = await Task.Run(()=> jsonService.DeserializeFromStream<RepoResponse>(fs));
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
            Stream inputStream = await FileSystem.Current.OpenAppPackageFileAsync(filename);
            FileStream outputStream = System.IO.File.Create(newPath);
            await inputStream.CopyToAsync(outputStream);
            outputStream.Dispose();
            inputStream.Dispose();
        }
    }
}
