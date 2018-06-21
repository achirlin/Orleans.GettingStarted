using System.Threading.Tasks;
using Orleans;
using Interaction.Interfaces;
using System.Net.Http;
using System;

namespace Interaction.Grains
{

	/// <summary>
	/// http://dotnet.github.io/orleans/1.5/Tutorials/Interaction-with-Libraries-and-Services.html#refreshing-the-value-with-a-timer
	/// </summary>
	public class StockGrain : Orleans.Grain, IStockGrain
	{
		string price;

		public override async Task OnActivateAsync()
		{
			string stock;

			this.GetPrimaryKey(out stock);

			await UpdatePrice(stock);

			// A traditional .NET timer is not suitable for running in a grain. Instead, Orleans provides it's own timer.
			IDisposable timer = RegisterTimer(
				UpdatePrice,
				stock,
				TimeSpan.FromSeconds(20),
				TimeSpan.FromSeconds(20));

			await base.OnActivateAsync();
		}

		async Task UpdatePrice(object stock)
		{
			price = await GetPriceFromYahoo(stock as string);
			Console.WriteLine(price);
		}

		async Task<string> GetPriceFromYahoo(string stock)
		{
			// You SHOULD AVOID using Task.Run, which always uses the .NET thread pool, 
			// and therefore will not run in the single-threaded execution model.

			return await Task.Factory.StartNew(() => $"\"MSFT\",\"Microsoft Corpora\",37.70,-0.19,\"- 0.50%\", {DateTime.Now.Ticks}");

			// var uri = "http://download.finance.yahoo.com/d/quotes.csv?f=snl1c1p2&e=.csv&s=" + stock;
			// using (var http = new HttpClient())
			// using (var resp = await http.GetAsync(uri))
			// {
			// 	return await resp.Content.ReadAsStringAsync();
			// }
		}

		public Task<string> GetPrice()
		{
			return Task.FromResult(price);
		}
	}
}
