using Amazon.EventBridge;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Payments.Api.Infrastructure.Events;
using Payments.Api.Infrastructure.Persistence;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Payments API",
        Version = "v1",
        Description = "Microsserviço responsável por pagamentos e transações"
    });
});

builder.Services.AddDbContext<PaymentsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<EventStore>();
builder.Services.AddAWSService<IAmazonEventBridge>();
builder.Services.AddScoped<EventBridgePublisher>();

var app = builder.Build();

//if (Debugger.IsAttached)
//{
//    // Swagger SEM restrição de ambiente (necessário para ECS)
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments API v1");
//        c.RoutePrefix = "swagger";
//    });
//}
//else
//{
//    // Swagger SEM restrição de ambiente (necessário para ECS)
//    app.UseSwagger();
//    app.UseSwaggerUI(c =>
//    {
//        c.SwaggerEndpoint("/payments/swagger/v1/swagger.json", "Payments API v1");
//        c.RoutePrefix = "swagger";
//    });
//}

if (Debugger.IsAttached)
{

    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users API v1");
        c.RoutePrefix = "swagger";
    });
}
else
{

    app.UseSwagger(c =>
    {
        c.RouteTemplate = "payments/swagger/{documentName}/swagger.json";
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/payments/swagger/v1/swagger.json", "Users API v1");
        c.RoutePrefix = "users/swagger";
    });
}


app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Health check para ALB / ECS
app.MapGet("/health", () => Results.Ok("Healthy"));

// ESSENCIAL para Docker / ECS
app.Urls.Add("http://0.0.0.0:80");

app.Run();

