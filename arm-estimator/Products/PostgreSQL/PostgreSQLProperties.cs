internal class PostgreSQLProperties
{
    public PostgreSQLStorageProfile? storageProfile { get; set; }
}

internal class PostgreSQLStorageProfile
{
    public int? storageMB { get; set; }
    public string? geoRedundantBackup { get; set; }
}