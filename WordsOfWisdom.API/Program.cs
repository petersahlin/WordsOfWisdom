
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.RateLimiting;
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

            var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("DefaultConnection")
                )
            {
                UserID = builder.Configuration["DatabaseCredentials:UserId"],
                Password = builder.Configuration["DatabaseCredentials:Password"]
            };



            builder.Services.AddDbContext<WordsOfWisdomContext>(options =>
                options.UseSqlServer(sqlConnectionStringBuilder.ConnectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5, // Number of retry attempts
                    maxRetryDelay: TimeSpan.FromSeconds(10), // Maximum delay between retries
                    errorNumbersToAdd: null // Optional: Add specific SQL error codes to retry
                    );
                }));




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
                        Url = new Uri("https://localhost:7090")
                    }
                });
            });

            // Add CORS (if needed)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowBlazorClient", policy =>
                {
                    policy.WithOrigins(
                        "https://localhost:5001",       // Production Client
                        "https://localhost:5001"         // Development Client
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });


            // Add rate limiter
            builder.Services.AddRateLimiter(options =>
            {
                options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
                    RateLimitPartition.GetFixedWindowLimiter("Global", partition => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 100,  // 100 requests per minute
                        Window = TimeSpan.FromMinutes(1)
                    }));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowBlazorClient"); // Enable CORS
            app.UseRateLimiter();


            app.UseHttpsRedirection();
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

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Frame-Options", "DENY");  // Prevent clickjacking
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");  // Prevent MIME sniffing
                context.Response.Headers.Append("Referrer-Policy", "no-referrer");  // No referrer data leaks
                context.Response.Headers.Append("Permissions-Policy", "geolocation=(), microphone=()");  // Block geolocation/mic access
                context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");  // Enforce HTTPS for 1 year

                await next();
            });


            app.MapControllers();

            app.Run();
        }
    }

}
