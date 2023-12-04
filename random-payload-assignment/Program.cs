using RandomPayloadAssignment.Azure;
using RandomPayloadAssignment.Blob;
using RandomPayloadAssignment.Http;
using RandomPayloadAssignment.Synchronizers;
using HttpRequest = RandomPayloadAssignment.Http.HttpRequest;

namespace RandomPayloadAssignment;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        var services = builder.Services;
        services.AddSingleton<IBlobStorage, BlobStorage>();
        services.AddSingleton<IHttpRequest, HttpRequest>();
        services.AddTransient<ILogsTable, LogsTable>();
        services.AddHostedService<Synchronizer>();

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}