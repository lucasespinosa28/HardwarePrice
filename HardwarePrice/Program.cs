using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

using HttpClient? client = new();
var content = await client.GetStringAsync("https://www.newegg.com/p/pl?N=100007709%20601357247&PageSize=96");
var parser = new HtmlParser();
var document = await parser.ParseDocumentAsync(content);

static List<int> parsePrices(IHtmlDocument document, List<int> prices)
{
    foreach (var item in document.QuerySelectorAll(".item-cell"))
    {
        var outOfStock = item.QuerySelector(".item-promo");
        if (item.QuerySelector(".item-promo") == null)
        {
            var value = item?.QuerySelector(".price-current")?
                                .QuerySelector("strong")?.TextContent;
            var cents = item?.QuerySelector(".price-current")?
                                .QuerySelector("sup")?.TextContent;
            var price = int.Parse($"{value}{cents}".Replace(",", "").Replace(".", ""));
            prices.Add(price);
        }
    }
    return prices;
}

static int getAveragePrice(List<int> prices)
{
    prices.Remove(prices.Max());
    prices.Remove(prices.Min());
    return (int)prices.Average();
}
var prices = parsePrices(document, new List<int>());
Console.WriteLine(getAveragePrice(prices));
Console.WriteLine(Environment.GetEnvironmentVariable("IEXEC_OUT"));
