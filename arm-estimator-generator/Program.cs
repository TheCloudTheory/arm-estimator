using System.Text.Json;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        File.AppendAllText("generator.csv", "type;serviceId;serviceName;skuId;skuName;productId;productName;meterId;meterName" + Environment.NewLine);

        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Get, $"https://prices.azure.com/api/retail/prices?$filter=serviceId eq 'DZH317F1HKN0' and armRegionName eq 'westeurope'");
        var response = await client.SendAsync(request);
        var data = JsonSerializer.Deserialize<RetailAPIResponse>(await response.Content.ReadAsStreamAsync());

        foreach(var item in data.Items)
        {
            File.AppendAllText("generator.csv", $"{item.type};{item.serviceId};{item.serviceName};{item.skuId};{item.skuName};{item.productId};{item.productName};{item.meterId};{item.meterName}" + Environment.NewLine);
        }

        var nextLink = await GetResults(client, $"https://prices.azure.com/api/retail/prices?$filter=serviceId eq 'DZH317F1HKN0' and armRegionName eq 'westeurope'");
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
            File.AppendAllText("generator.csv", $"{item.type};{item.serviceId};{item.serviceName};{item.skuId};{item.skuName};{item.productId};{item.productName};{item.meterId};{item.meterName}" + Environment.NewLine);
        }

        return data.NextPageLink;
    }
}