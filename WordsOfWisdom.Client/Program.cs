using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WordsOfWisdom.Client.Services;

namespace WordsOfWisdom.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var config = builder.Configuration;

            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(config["ApiBaseUrl"] ?? "https://localhost:443/api/") 
            });


            builder.Services.AddScoped<IQuotesService, QuotesService>();

            await builder.Build().RunAsync();
        }
    }
}
