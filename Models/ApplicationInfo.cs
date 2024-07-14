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
    public string UniqueName { get; set; }
    public string? ProjectUrl { get; set; }
    public string? Summary { get; set; }
    public string? Description { get; set; }
    public string? IconUrl { get; set; }
    public DateTimeOffset LastUpdated { get; set; }
    public DateTimeOffset ReleaseDate { get; set; }
    public IList<Apk> Apks { get; } = new List<Apk>();
    public IList<string>? Categories { get;} = new List<string>();
    public IList<string>? ScreenshotsUrl { get; } = new List<string>();
    public IList<string>? Permissions { get; } = new List<string>();

}
public partial class Apk : RealmObject
{
    [PrimaryKey]
    public string Url { get; set; }
    public int Size { get; set; }
}

