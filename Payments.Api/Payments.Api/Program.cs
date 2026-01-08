using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Payments.Api.Infrastructure.Events;
using Payments.Api.Infrastructure.Persistence;

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

var app = builder.Build();

// Swagger SEM restrição de ambiente (necessário para ECS)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payments API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseAuthorization();

app.MapControllers();

// Health check para ALB / ECS
app.MapGet("/health", () => Results.Ok("Healthy"));

// ESSENCIAL para Docker / ECS
app.Urls.Add("http://0.0.0.0:80");

app.Run();

