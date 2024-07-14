﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoreanStore.Models;

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
    public ulong LastUpdated { get; set; }
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
