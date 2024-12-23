
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Repositories;
using WordsOfWisdom.API.Services;

namespace WordsOfWisdom.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<WordsOfWisdomContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();
            builder.Services.AddScoped<QuoteImporter>();



            builder.Services.AddControllers();

            // Swagger/OpenAPI configuration
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Words of Wisdom API",
                    Version = "v1",
                    Description = "An API for managing and retrieving quotes of wisdom.",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Peter",
                        Email = "petersahlindev@gmail.com",
                        Url = new Uri("https://petersahlin.dev/")
                    }
                });
            });

            // Add CORS (if needed)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.WithOrigins("https://localhost:7086")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowBlazorClient"); // Enable CORS
            app.UseAuthorization();


            /* =================================================================
                                MANUAL IMPORT OF QUOTES
               =================================================================*/ 
            //app.Lifetime.ApplicationStarted.Register(async () =>
            //{
            //    using var scope = app.Services.CreateScope();
            //    var services = scope.ServiceProvider;
            //    var dbContext = services.GetRequiredService<WordsOfWisdomContext>();
            //    var quoteRepository = new QuoteRepository(dbContext);
            //    var importer = services.GetRequiredService<QuoteImporter>();

            //    int retryCount = 3;
            //    for (int i = 0; i < retryCount; i++)
            //    {
            //        try
            //        {
            //            await importer.ImportQuotesAsync("C:\\Peter\\petersahlin.dev\\Quotes for Words of Wisdom\\QuotesForImport.txt");
            //            Debug.WriteLine("Quotes imported successfully.");
            //            break; // Exit loop on success
            //        }
            //        catch (Exception ex)
            //        {
            //            Console.WriteLine($"Attempt {i + 1} failed: {ex.Message}");

            //            if (i == retryCount - 1)
            //            {
            //                Console.WriteLine("All retry attempts failed.");
            //            }
            //            else
            //            {
            //                await Task.Delay(2000); // Wait for 2 seconds before retrying
            //            }
            //        }
            //    }
            //});
            /*============================== END OF IMPORT CODE ============================== */

            app.MapControllers();

            app.Run();
        }
    }

}
