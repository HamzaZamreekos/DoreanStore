using DoreanStore.Models;
using DoreanStore.Services;
using Realms;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Repositories
{
    public class IndexRepository
    {
        private readonly Realm realm;
        private static string dbName = "dbe.realm";
        private readonly IOService _io;
        public IndexRepository(IOService iOService) 
        {
            _io = iOService;
            string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, dbName);
            realm = Realm.GetInstance(new RealmConfiguration(pathUri));
        }
        public async Task ConvertIndexToAppInfoAndSave()
        {
            var index = await _io.DeserializeFdroidRepoAsync();
            foreach (var item in index!.Packages)
            {
                ApplicationInfo applicationInfo = new();
                applicationInfo.UniqueName = item.Key;
                if(item.Value!.Metadata.SummaryInLanguages is not null)
                {
                    string result;
                    bool exists = item.Value.Metadata.SummaryInLanguages.TryGetValue("en-US", out result);
                    if(exists) 
                        applicationInfo.Summary = result;
                    else
                    {
                        applicationInfo.Summary = item.Value.Metadata.SummaryInLanguages.First().Value;
                    }
                }
                if (item.Value!.Metadata.DescriptionInLanguages is not null)
                {
                    string result;
                    bool exists = item.Value.Metadata.DescriptionInLanguages.TryGetValue("en-US", out result);
                    if (exists)
                        applicationInfo.Description = result;
                    else
                    {
                        applicationInfo.Description = item.Value.Metadata.SummaryInLanguages.First().Value;
                    }
                }
                //if (item.Value.Metadata.DescriptionInLanguages is not null)
                //    applicationInfo.Description = item.Value.Metadata.DescriptionInLanguages["en-US"];
                applicationInfo.ProjectUrl = item.Value.Metadata.SourceCode;
                item.Value.Metadata.Categories.ForEach(applicationInfo.Categories.Add);
                if (item.Value!.Metadata.IconInLanguages is not null)
                {
                    Src? result;
                    bool exists = item.Value.Metadata.IconInLanguages.TryGetValue("en-US", out result);
                    if (exists)
                        applicationInfo.IconUrl = result.Name;
                    else
                    {
                        applicationInfo.IconUrl = item.Value.Metadata.IconInLanguages.First().Value.Name;
                    }
                }
                if (item.Value!.Metadata.NameInLanguages is not null)
                {
                    string? result;
                    bool exists = item.Value.Metadata.NameInLanguages.TryGetValue("en-US", out result);
                    if (exists)
                        applicationInfo.DisplayName = result;
                    else
                    {
                        applicationInfo.DisplayName = item.Value.Metadata.NameInLanguages.First().Value;
                    }
                }
                //if (item.Value.Metadata.IconInLanguages is not null)
                //    applicationInfo.IconUrl = item.Value.Metadata.IconInLanguages["en-US"]!.Name;

                DateTime startUpdate = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime dateUpdate = startUpdate.AddMilliseconds(Convert.ToDouble(item.Value.Metadata.LastUpdated));
                DateTime startAdd = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                DateTime dateAdd = startAdd.AddMilliseconds(Convert.ToDouble(item.Value.Metadata.LastUpdated));
                applicationInfo.LastUpdated = dateUpdate;
                applicationInfo.ReleaseDate = dateAdd;
                if(item!.Value.Metadata.Screenshots is not null && item!.Value.Metadata.Screenshots.phone is not null)
                    foreach(var screenshot in item!.Value.Metadata.Screenshots.phone.Values)
                    {
                        foreach(var screenshotItem in screenshot)
                        {
                            applicationInfo.ScreenshotsUrl.Add(screenshotItem.Name);
                        }
                    }
                foreach(var version in item!.Value.Versions)
                {
                    applicationInfo.Apks.Add(new Apk { Url = version.Value.File.Name, Size = Convert.ToInt32(version.Value.File.Size)});
                }
                await realm.WriteAsync(() =>
                {
                    realm.Add(applicationInfo, update: true);
                });

            }
        }
    }
}
