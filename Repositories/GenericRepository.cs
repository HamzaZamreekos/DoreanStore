using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DoreanStore.Services;
using Microsoft.Maui;
using Realms;

namespace DoreanStore.Repositories;

public class GenericRepository<T> where T : RealmObject
{
    private readonly Realm realm;
    private readonly IOService iOService;
    private static string dbName =  "db.realm";

    public GenericRepository(IOService iOService)
    {
        this.iOService = iOService;
        string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, dbName);
        realm = Realm.GetInstance(new RealmConfiguration(pathUri));
        iOService.CopyFileToAppDataDirectory(dbName, false).Wait();
    }
    public T Update(T toUpdate)
    {
        realm.Write(() =>
        {
            toUpdate = realm.Add(toUpdate, update: true);
        });
        return toUpdate;
    }
    public T Add(T toAdd)
    {
        realm.Write(() =>
        {
            toAdd = realm.Add<T>(toAdd);
        });
        return toAdd;
    }
    public void Remove(T toRemove)
    {
        realm.Write(() =>
        {
            realm.Remove(toRemove);
        });
    }
}
