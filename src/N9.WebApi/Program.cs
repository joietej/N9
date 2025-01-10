using Azure.Identity;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using N9.WebApi.Apis;
using N9.WebApi.Extensions;
using N9.WebApi.GraphQL;
using N9.Data.Context;
using N9.Data.Init;
using N9.WebApi.GraphQL.Queries;
using Polly;
using Polly.Retry;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureOpenTelemetry();
// Add Http logging
builder.Services.AddHttpLogging(options => { options.LoggingFields = HttpLoggingFields.All; }); 
builder.Services.AddProblemDetails();

// Add services to the container.
builder.AddConfig();

builder.Services.AddHealthChecks();

// Add Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddAuthorization();

// Add Polly
builder.Services.AddResiliencePipeline("default",
    rp =>
    {
        rp.AddRetry(new RetryStrategyOptions
        {
            MaxRetryAttempts = 4,
            Delay = TimeSpan.FromMicroseconds(1000),
            ShouldHandle = new PredicateBuilder().Handle<Exception>()
        });
    });

// Add Http Client
builder.Services.AddHttpClient("default").AddStandardResilienceHandler();

// Add Cors
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()));

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApiWithSecurityScheme();

// Add Azure Key Vault
if (builder.Environment.IsProduction())
    builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
        new DefaultAzureCredential());

builder.AddDbContextWithSqlConnection<BooksDbContext>();

builder.Services.AddGraphQLServer()
    .AddQueryType<BookQuery>()
    .AddProjections()
    .AddFiltering()
    .AddSorting()
    .InitializeOnStartup();

var app = builder.Build();

// Error handling
app.UseExceptionHandling();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(options => { options.WithEndpointPrefix("/api-docs/{documentName}"); });
}

app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQLHttp().RequireAuthorization();
app.MapNitroApp("/graphql/ui");

app.MapHealthChecks("/healthz");

// Map routes
app.MapBooksApi();

// Initialize Db with migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetService<IDbInitializer>();
    await db!.InitializeAsync();
}

app.Run();