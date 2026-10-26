using FluentValidation;
using Prometheus;
using PsySchedule.Depends;
using PsySchedule.Doc;
using PsySchedule.Middlewares;
using PsySchedule.Services.Grpc;
using PsySchedule.Validations;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.UsePgSQl();
builder.UseSerilog();
builder.UseAuthentication();
builder.UseDepends();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
    options.AddOperationTransformer<BearerOperationTransformer>();
});

Log.Information("Start application");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options.AddPreferredSecuritySchemes("Bearer");
    });
}

app.UseMiddleware<UseExceptionHandler>();

app.UseHttpMetrics();
app.UseMetricServer();
app.MapMetrics();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<ClientTelegramGrpcService>();

app.Run();
