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
    private static string dbName =  "dbe.realm";
    private readonly IOService io;

    public GenericRepository(IOService iOService)
    {
        io = iOService;
        string pathUri = Path.Combine(FileSystem.Current.AppDataDirectory, dbName);
        realm = Realm.GetInstance(new RealmConfiguration(pathUri));
    }
    /// <summary>
    /// Hands a reference to the realm instace the repository uses, So that it could be used to update entities in the database 
    /// </summary>
    /// <returns>Reference to a realm</returns>
    public Realm ReturnRealmInstance()
    {
        return realm;
    }
    public T? Get(Guid id)
    {
        return realm.Find<T>(id);
    }
    public IEnumerable<T>? GetAll()
    {
        return realm.All<T>();
    }
    //public T? Update(T toUpdate)
    //{
    //    using (var transaction = realm.BeginWrite())
    //    {
    //        toUpdate = realm.Add(toUpdate, update: true);
    //        transaction.Commit();
    //    }
    //    return toUpdate;
    //}
    public T? Add(T toAdd)
    {
        realm.Write(() =>
        {
            toAdd = realm.Add<T>(toAdd, update:false);
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
