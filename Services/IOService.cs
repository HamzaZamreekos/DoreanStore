using DoreanStore.Models;
using Microsoft.Maui.Controls.PlatformConfiguration;
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
        /// Requests permissions to install apps if persmission wasn't granted before, Then installs the apk
        /// </summary>
        /// <param name="name"></param>
        public void InstallApk(string name)
        {
#if Android
            string _apkPath = Path.Combine(FileSystem.Current.AppDataDirectory, name);
              var packageManager = Platform.CurrentActivity!.PackageManager;
            var res = packageManager!.CanRequestPackageInstalls();
            if (!res)
            {
                Platform.CurrentActivity.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionManageUnknownAppSources!, Android.Net.Uri.Parse("package:" + AppInfo.Current.PackageName)));
            }else
            {
               
                var context = Platform.AppContext;
                Java.IO.File apkFile = new Java.IO.File(_apkPath);
                var res1=apkFile.Exists();
                Android.Content.Intent intent = new Android.Content.Intent(Android.Content.Intent.ActionView);


               var uri = FileProvider.GetUriForFile(context, context.ApplicationContext!.PackageName + ".fileProvider", apkFile);

                intent.SetDataAndType(uri, "application/vnd.android.package-archive");
                intent.AddFlags(Android.Content.ActivityFlags.NewTask);
                intent.AddFlags(Android.Content.ActivityFlags.GrantReadUriPermission);
                intent.AddFlags(Android.Content.ActivityFlags.ClearTop);
                intent.PutExtra(Android.Content.Intent.ExtraNotUnknownSource, true);
                intent.PutExtra("apkPath", _apkPath);

               Platform.CurrentActivity.StartActivityForResult(intent, 1);
            }
#endif
        }
        /// <summary>
        /// Opens the index json file as a stream, and uses the JsonService to deserialize it asynchronously
        /// </summary>
        /// <returns>The deserialized index file</returns>
        public async Task<RepoResponse?> DeserializeFdroidRepoAsync()
        {
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
