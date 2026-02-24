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
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"))
);

builder.Services.AddScoped<EventStore>();

var app = builder.Build();

app.UsePathBase("/payments");

app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/payments/swagger/v1/swagger.json", "Payments API v1");
    c.RoutePrefix = "swagger";
});

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", () => Results.Ok("Healthy"));

app.Urls.Add("http://0.0.0.0:80");

// Migration automática
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PaymentsDbContext>();
    db.Database.Migrate();
}

app.Run();