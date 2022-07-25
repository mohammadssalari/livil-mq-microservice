using livil_mq_microservice.RabibitMq;
using Microsoft.Extensions.Hosting.WindowsServices;
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
            // Add services to the container.
            builder.Services.Configure<RabbitMqConfig>(c => configuration.GetSection("RabbitMq").Bind(c));
        
            builder.Services.AddTransient<IRabbitMq, RabbitMq>();
            builder.Services.AddHostedService<RqReciever.RqReciever>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
            }

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}