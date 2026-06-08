internal interface IRetailQuery
{
    string? GetQueryUrl(string location);
    RetailAPIResponse? GetFakeResponse();
}
