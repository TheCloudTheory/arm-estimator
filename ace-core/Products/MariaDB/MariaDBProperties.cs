internal class MariaDBProperties
{
    public MariaDBStorageProfile? storageProfile { get; set; }
}

internal class MariaDBStorageProfile
{
    public int? storageMB { get; set; }
    public string? geoRedundantBackup { get; set; }
}