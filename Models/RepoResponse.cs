using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Models;
/*
                "icon": {
                    "en-US": {
                        "name": "/com.drip/en-US/icon_RSOvW3M2IavMMcp22IDGcclg1CIlF0othhm4nx-YDVI=.png",
                        "sha256": "4523af5b733621abcc31ca76d880c671c960d42225174a2d8619b89f1f980d52",
                        "size": 15651
                    }
                },
 */
public class RepoResponse
{
    [JsonProperty("repo")]
    public RepoInfo Repo { get; set; }
    [JsonProperty("packages")]
    public Dictionary<string, AppData?> Packages { get; set; }
}
public class AppData
{
    [JsonProperty("metadata")]
    public Metadata Metadata;
    [JsonProperty("versions")]
    public Dictionary<string, FileInfo?> Versions { get; set; }
}
public class Metadata
{
    [JsonProperty("lastUpdated")]
    public ulong? LastUpdated { get; set; }
    [JsonProperty("added")]
    public ulong? Added { get; set; }
    [JsonProperty("categories")]
    public List<string> Categories { get; set; }
    [JsonProperty("changelog")]
    public string Changelog { get; set; }
    [JsonProperty("sourceCode")]
    public string SourceCode { get; set; }
    [JsonProperty("screenshots")]
    public Dictionary<string, Screenshots?> Screenshots { get; set; }
    [JsonProperty("name")]
    public Dictionary<string,string> NameInLanguages{ get; set; } //take en-US
    [JsonProperty("summary")]
    public Dictionary<string,string> SummaryInLanguages { get; set; } //take en-US
    [JsonProperty("description")]
    public Dictionary<string,string> DescriptionInLanguages { get; set; } //take en-US
    [JsonProperty("icon")]
    public Dictionary<string, Src?> IconInLanguages { get; set; } //take en-US

}
public class RepoInfo
{
    [JsonProperty("timestamp")]
    public ulong Timestamp { get; set; }
}
public class FileInfo
{
    [JsonProperty("added")]
    public ulong Added { get; set; }
    [JsonProperty("file")]
    public File File { get; set; }
    [JsonProperty("src")]
    public Src source { get; set; }
    [JsonProperty("manifest")]
    public Manifest Manifest { get; set; }
}

public class File
{
    [JsonProperty("name")]
    public string Name { get; set; }
    public string sha256 { get; set; }
    [JsonProperty("size")]
    public ulong Size { get; set; }
    public string ipfsCIDv1 { get; set; }
}

public class Src
{
    [JsonProperty("name")]
    public string Name { get; set; }
    public string sha256 { get; set; }
    [JsonProperty("size")]
    public ulong Size { get; set; }
}

public class Manifest
{
    [JsonProperty("versionName")]
    public string VersionName { get; set; }
    [JsonProperty("versionCode")]
    public ulong VersionCode { get; set; }
    [JsonProperty("usesSdk")]
    public Usessdk UsesSdk { get; set; }
    [JsonProperty("signer")]
    public Signer Signer { get; set; }
    [JsonProperty("usesPermission")]
    public Usespermission[] UsesPermission { get; set; }
}

public class Usessdk
{
    [JsonProperty("minSdkVersion")]
    public ulong MinSdkVersion { get; set; }
    [JsonProperty("targetSdkVersion")]
    public ulong TargetSdkVersion { get; set; }
}

public class Signer
{
    public string[] sha256 { get; set; }
}

public class Usespermission
{
    [JsonProperty("targetSdkVersion")]
    public string Name { get; set; }
}


public class Screenshots
{
    public Phone phone { get; set; }
}

public class Phone
{
    public EnUS[] enUS { get; set; }
}

public class EnUS
{
    [JsonProperty("name")]
    public string Path { get; set; }
    public string sha256 { get; set; }
    [JsonProperty("size")]
    public int Size { get; set; }
}
