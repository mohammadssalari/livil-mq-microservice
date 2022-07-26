using System.Reflection;
using livil_mq_microservice.Models;
using livil_mq_microservice.RabibitMq;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.OpenApi.Models;
using Serilog;

namespace livil_mq_microservice
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var options = new WebApplicationOptions
            {
                Args = args,
                ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
            };
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration().WriteTo.Console().WriteTo
                .File($"mylog-.log", rollingInterval: RollingInterval.Day).MinimumLevel
                .Debug().CreateLogger();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            var builder = WebApplication.CreateBuilder(options);
            builder.Host.UseWindowsService().UseSystemd();

            builder.Services.Configure<RecievingApiConfig>(c =>
            {
                configuration.GetSection("SendAPI").Bind(c);
            });
            //builder.Services.AddSingleton<RecievingApiConfig>();
            // Add services to the container.
            builder.Services.Configure<RabbitMqConfig>(c => configuration.GetSection("RabbitMq").Bind(c));
        //Dependency Injection for The RabbitMqImplementation
            builder.Services.AddTransient<IRabbitMq, RabbitMq>();
            //Dependency Injection for The Worker
            builder.Services.AddHostedService<RqReciever.RqReciever>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen( c=>c.SwaggerDoc("v1",
                new OpenApiInfo(){Title = "TestApi with RabbitMqIntegrations",Version = "v1"}));

         

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
            }
            //Moved out of Scope so its always available (Debug / Release )
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}