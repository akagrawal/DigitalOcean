using Ingestion.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddApiServices();

var app = builder.Build();

app.ConfigurePipeline();

app.Run();
