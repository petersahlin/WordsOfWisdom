
using Microsoft.EntityFrameworkCore;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Repositories;

namespace WordsOfWisdom.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddDbContext<WordsOfWisdomContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IQuoteRepository, QuoteRepository>();



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
            app.MapControllers();

            app.Run();
        }
    }

}
