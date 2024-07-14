using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using Realms;

namespace DoreanStore.Models;

public partial class ApplicationInfo : RealmObject
{
    [PrimaryKey]
    public Guid id { get; set; } = Guid.NewGuid();
    public DateTime LastUpdated { get; set; }
    public DateTime ReleaseDate { get; set; }
    public List<Apk> Apks { get; set; } = new();
    public string? ImageUrl { get; set; }
    List<string> Screenshots { get; set; } = new();

}
public partial class Apk : RealmObject
{
    public string Urls { get; set; }
    public int Size { get; set; }
}

