using System.Text.Json;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        File.AppendAllText("acr.csv", "type;serviceId;serviceName;skuId;skuName;productId;productName;meterId;meterName" + Environment.NewLine);

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://prices.azure.com/api/retail/prices?$filter=serviceId eq 'DZH315F9L8DM' and armRegionName eq 'westeurope'");
        var response = await client.SendAsync(request);
        var data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

        foreach(var item in data.Items)
        {
            File.AppendAllText("acr.csv", $"{item.type};{item.serviceId};{item.serviceName};{item.skuId};{item.skuName};{item.productId};{item.productName};{item.meterId};{item.meterName}" + Environment.NewLine);
        }

        var nextLink = data.NextPageLink;
        while(nextLink != null)
        {
            nextLink = await GetResults(client, nextLink);
        }

        return 0;
    }

    private static async Task<string?> GetResults(HttpClient client, string url)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        var response = await client.SendAsync(request);
        var data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

        foreach (var item in data.Items)
        {
            File.AppendAllText("acr.csv", $"{item.type};{item.serviceId};{item.serviceName};{item.skuId};{item.skuName};{item.productId};{item.productName};{item.meterId};{item.meterName}" + Environment.NewLine);
        }

        return data.NextPageLink;
    }
}