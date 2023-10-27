namespace ACE.Caching;

/// <summary>
/// Provides an interface for cache handlers
/// </summary>
internal interface ICacheHandler 
{
    T? GetCachedData<T>() where T : class;
    void SaveData(object data);
    bool CacheFileExists();
}