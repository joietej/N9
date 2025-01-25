# **Building Resilient, Secure, and Scalable APIs with ASP.NET Core 9 Minimal APIs**

This guide provides a comprehensive approach to building production-ready APIs using **ASP.NET Core 9 Minimal APIs**, aligned with the **Azure Well-Architected Framework** and **Cloud Design Patterns**. Each section dives into key aspects such as reliability, performance efficiency, security, cost optimization, and observability, ensuring you have the tools to create robust solutions.

## **Introduction**

Minimal APIs in ASP.NET Core 9 offer a lightweight, flexible approach to building modern web APIs. By focusing on simplicity and performance, they provide an efficient way to create scalable solutions for cloud-native applications. Leveraging integrations with Azure services such as Azure Front Door, Azure Container Apps, and Azure Monitor, you can design resilient APIs that adhere to best practices and architectural patterns.

## **What’s New in .NET 9 and ASP.NET Core 9**

.NET 9 and ASP.NET Core 9 introduce several enhancements and new features aimed at improving performance, productivity, and scalability for modern application development. Here are some key improvements:

### **Key Improvements in .NET 9**

1. **Native AOT (Ahead-of-Time Compilation)**:
   - Improves application startup performance and reduces memory usage.
   - Suitable for minimal APIs and microservices, enabling lightweight deployments.

2. **HybridCache**:
   - A new caching mechanism combining in-memory and distributed cache for enhanced performance.
   - Simplifies caching configurations and improves scalability in distributed environments.

3. **Improved HTTP/3 Support**:
   - Enhanced support for HTTP/3 for faster, more secure communication.
   - Reduces latency and improves performance for web APIs.

4. **Performance Enhancements**:
   - Improved garbage collection (GC) for better memory management.
   - Enhanced Just-In-Time (JIT) compiler optimizations.

5. **Enhanced OpenTelemetry Support**:
   - Native integration for distributed tracing and metrics collection.
   - Simplifies observability for cloud-native applications.

### **Key Improvements in ASP.NET Core 9**

1. **Typed Results for Minimal APIs**:
   - Improves type safety and reduces boilerplate code when returning responses.
   - Example:
     ```csharp
     app.MapGet("/user", () => TypedResults.Ok(new { Name = "John", Age = 30 }));
     ```

2. **Rate Limiting Middleware**:
   - Built-in middleware for controlling traffic and protecting APIs.
   - Simplifies adding rate-limiting policies to endpoints.

3. **Improved Authentication and Authorization**:
   - Enhanced integration with Azure Active Directory and OpenID Connect.
   - Simplified token validation and role-based access control (RBAC).

4. **HTTP Logging Enhancements**:
   - Allows detailed logging of HTTP requests and responses for debugging.
   - Supports filtering and customization of logged data.

5. **gRPC Improvements**:
   - Improved performance and scalability for gRPC services.
   - Enhanced integration with OpenTelemetry for distributed tracing.

6. **Enhanced Deployment Options**:
   - Better support for Azure Container Apps and other containerized environments.
   - Simplified configuration for Kubernetes and cloud-native deployments.

### **Why These Improvements Matter**

These enhancements in .NET 9 and ASP.NET Core 9 help developers:
- Build high-performance, scalable applications.
- Simplify application monitoring, security, and deployment.
- Align with modern cloud-native practices and frameworks like the Azure Well-Architected Framework.

By leveraging these improvements, developers can create robust, secure, and efficient APIs that meet modern application demands.



## **Azure Well-Architected Framework**

The **Azure Well-Architected Framework** provides a set of best practices and guiding principles to design high-quality cloud-native solutions. It focuses on five key pillars:
1. **Reliability**: Ensures the application can recover from failures and continue functioning.
2. **Security**: Protects applications and data from threats.
3. **Cost Optimization**: Manages and reduces operational expenses.
4. **Operational Excellence**: Improves deployment processes and operational workflows.
5. **Performance Efficiency**: Ensures efficient resource use and optimized performance.

This document aligns with these principles by:
- **Reliability**: Using Polly for resilience, database retry logic, and rate limiting.
- **Security**: Implementing Azure Front Door, Key Vault, and Azure AD for protection.
- **Cost Optimization**: Leveraging Azure Container Apps for cost-effective scaling.
- **Performance Efficiency**: Utilizing HybridCache and OpenTelemetry to optimize performance.
- **Operational Excellence**: Following best practices for deployment, monitoring, and scaling APIs.

By following this guide, you’ll have the foundational knowledge to create secure, scalable, and cost-efficient APIs while adhering to Azure Well-Architected Framework principles.



# **1️⃣ OpenAPI Documentation with Scaler**

**OpenAPI** documentation provides a standard for describing APIs, making them easier to understand and consume. **Scaler** simplifies hosting and sharing OpenAPI schemas.

## **Key Features**
- **Version Control**: Manage multiple versions of API documentation.
- **Collaboration**: Centralized hub for internal and external teams.
- **API Discovery**: Simplifies onboarding by making APIs accessible.

## **Implementation Steps**

### Enable OpenAPI in Minimal APIs

```csharp
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi(options =>
    {
        options.Path = "/openapi/{documentName}.json";
    });
    app.MapScalarApiReference();
}
```

### Publish OpenAPI Schema to Scaler

Export the OpenAPI schema and upload it to Scaler:

```bash
curl http://localhost:5000/openapi/v1.json > openapi.json
scalar publish --file openapi.json --project MyMinimalAPI
```


# **2️⃣ Reliability**

Reliability ensures that APIs remain operational during failures, high loads, or transient errors.

## **Key Features**
- **Rate Limiting**: Protects backend services from being overwhelmed.
- **Resilience with Polly**: Adds retries, timeouts, and circuit breakers for external dependencies.
- **Database Resilience**: Handles transient database failures using retry logic.

## **Implementation Steps**

### Rate Limiting

Add rate limiting to control traffic and prevent overload:

```csharp
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("Fixed", config =>
    {
        config.Window = TimeSpan.FromSeconds(10);
        config.PermitLimit = 5;
        config.QueueLimit = 2;
    });
});

app.UseRateLimiter();

app.MapGet("/rate-limited", () => "Rate limited endpoint!")
   .RequireRateLimiter("Fixed");
```

### Resilience with Polly

Handle transient failures with retries and circuit breakers:

```csharp
builder.Services.AddHttpClient("ExternalService", client =>
{
    client.BaseAddress = new Uri("https://api.example.com");
})
.AddTransientHttpErrorPolicy(policy =>
    policy.WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))))
.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10)))
.AddTransientHttpErrorPolicy(policy =>
    policy.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
```

### Database Resilience

Add retry logic to handle database deadlocks and transient issues:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null)));
```


# **3️⃣ Security**

Security is critical to protecting APIs from unauthorized access, malicious traffic, and data breaches. This section covers integration with Azure services for robust API security.

## **Key Features**
- **Azure Front Door**: Protects APIs with DDoS protection, WAF, and HTTPS enforcement.
- **Authentication**: Uses Azure AD with JWT tokens for secure API access.
- **Secrets Management**: Securely stores sensitive data in Azure Key Vault.
- **Load Balancing**: Azure Traffic Manager handles DNS-based global traffic distribution.

## **Implementation Steps**

### Azure Front Door for API Protection

1. **Create Azure Front Door** in the Azure portal.
2. Configure **WAF rules** to block common threats like SQL injection and XSS.
3. Enable **HTTPS** with an Azure Key Vault-managed TLS certificate.

### Authentication with Azure AD

Use Azure AD for secure access to APIs with JWT tokens:

```csharp
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://login.microsoftonline.com/{tenantId}";
        options.Audience = "api://your-api-client-id";
    });

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/secure-endpoint", () => "This endpoint is secured.")
   .RequireAuthorization();
```

### Secrets Management with Azure Key Vault

Store sensitive data such as connection strings and API keys securely:

```csharp
builder.Configuration.AddAzureKeyVault(
    new Uri("https://your-keyvault-name.vault.azure.net/"),
    new DefaultAzureCredential());
```

### Load Balancing with Azure Traffic Manager

1. **Create a Traffic Manager Profile**:
   - Choose a routing method (e.g., priority, performance).
2. **Add Endpoints**:
   - Add API backends like Azure App Services or on-premises APIs.
3. **Enable Health Probes**:
   - Automatically detect failures and reroute traffic.


# **4️⃣ Performance Efficiency**

Optimizing the performance of APIs ensures faster response times, reduced latency, and a better user experience. This section explores caching, metrics collection, and query optimization.

## **Key Features**
- **HybridCache**: Combines in-memory and distributed caching for high-speed data retrieval.
- **Metrics Collection with OpenTelemetry**: Tracks performance and resource usage.
- **Database Query Optimization**: Improves data retrieval efficiency.

## **Implementation Steps**

### HybridCache for Caching

Leverage HybridCache in .NET 9 to combine local memory and distributed cache (e.g., Redis):

```csharp
builder.Services.AddMemoryCache(); // Local in-memory cache
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

builder.Services.AddHybridCache(options =>
{
    options.DefaultMemoryExpiration = TimeSpan.FromMinutes(5); // Memory TTL
    options.DistributedCacheExpiration = TimeSpan.FromMinutes(10); // Redis TTL
});

app.MapGet("/hybrid-cache", async (IHybridCache cache) =>
{
    var value = await cache.GetOrSetAsync<string>("key", async entry =>
    {
        entry.SetMemoryExpiration(TimeSpan.FromMinutes(5));
        entry.SetDistributedExpiration(TimeSpan.FromMinutes(10));
        return "Fresh data from database.";
    });
    return value;
});
```

### Metrics Collection with OpenTelemetry

Track API metrics using OpenTelemetry:

```csharp
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder.AddAspNetCoreInstrumentation()
           .AddHttpClientInstrumentation()
           .AddAzureMonitorMetricExporter();
});
```

### Database Query Optimization

Use indexed queries and avoid loading unnecessary data:

```csharp
var optimizedQuery = dbContext.Users
    .Where(u => u.IsActive)
    .Select(u => new { u.Id, u.Name })
    .ToList();
```


# **5️⃣ Observability**

Observability is essential for monitoring API behavior, troubleshooting issues, and ensuring high availability. This section covers distributed tracing, metrics collection, and HTTP logging.

## **Key Features**
- **Distributed Tracing with OpenTelemetry**: Captures traces for monitoring API behavior.
- **Metrics Collection**: Tracks API latency, throughput, and error rates.
- **HTTP Logging**: Captures detailed HTTP request and response data for debugging.

## **Implementation Steps**

### Distributed Tracing with OpenTelemetry

Enable tracing for API requests:

```csharp
builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder.AddAspNetCoreInstrumentation()
           .AddHttpClientInstrumentation()
           .AddAzureMonitorTraceExporter();
});
```

### Metrics Collection

Use OpenTelemetry to collect API performance metrics:

```csharp
builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder.AddAspNetCoreInstrumentation()
           .AddHttpClientInstrumentation()
           .AddAzureMonitorMetricExporter();
});
```

### HTTP Logging for Debugging

Enable detailed logging of HTTP requests and responses:

```csharp
builder.Services.AddHttpLogging(options =>
{
    options.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
});

app.UseHttpLogging();
```


# **6️⃣ Exception Handling**

Effective exception handling ensures that APIs provide meaningful error responses while maintaining stability and security.

## **Key Features**
- **Global Error Middleware**: Captures and handles unhandled exceptions.
- **Validation Error Responses**: Provides clear messages for client errors.

## **Implementation Steps**

### Global Error Middleware

Create middleware to handle all unhandled exceptions globally:

```csharp
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new { Error = ex.Message });
        }
    }
}

app.UseMiddleware<ExceptionMiddleware>();
```

### Validation Error Responses

Use FluentValidation for structured validation and error messages:

```csharp
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

app.MapPost("/create-user", (UserDto user, IValidator<UserDto> validator) =>
{
    var validationResult = validator.Validate(user);
    if (!validationResult.IsValid)
    {
        return TypedResults.BadRequest(validationResult.Errors);
    }
    return TypedResults.Ok("User created successfully!");
});
```

# **7️⃣ Cost Optimization**

Cost optimization focuses on reducing operational costs and resource usage without compromising performance or reliability.

## **Key Features**
- **Containerization**: Deploy lightweight, efficient containers for APIs.
- **Scaling**: Use Azure Container Apps for dynamic scaling based on traffic.
- **Resource Optimization**: Configure CPU, memory, and replicas efficiently.

## **Implementation Steps**


### Containerize the Application

#### Create a `Dockerfile` to containerize the Minimal API:

```
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
COPY . ./
ENTRYPOINT ["dotnet", "MyMinimalApi.dll"]

```

#### Build, tag, and push the container image:

```
docker build -t myminimalapi:latest .
docker tag myminimalapi:latest myregistry.azurecr.io/myminimalapi:latest
docker push myregistry.azurecr.io/myminimalapi:latest
```

#### Deploy to Azure Container Apps

```
az containerapp create \
  --name myapp \
  --resource-group my-resource-group \
  --image myregistry.azurecr.io/myminimalapi:latest \
  --cpu 0.5 --memory 1.0Gi \
  --min-replicas 1 --max-replicas 5 \
  --environment my-environment
```

#### Resource Optimization
- **CPU**: Allocate based on expected traffic. Example: `--cpu 0.5`.
- **Memory**: Avoid overprovisioning. Example: `--memory 1.0Gi`.
- **Auto-Scaling**: Configure `--min-replicas` and `--max-replicas` for efficient scaling during peak traffic.


# **8️⃣ Final Architecture Blueprint**

This section provides an overview of the architecture for building resilient, scalable, and secure APIs with ASP.NET Core 9 Minimal APIs.

## **Key Components**
- **Azure Front Door**: For global traffic distribution and enhanced security with DDoS protection.
- **Scaler**: Centralized hosting for OpenAPI documentation.
- **HybridCache**: Combines in-memory and distributed caching for optimal performance.
- **Polly**: Adds resilience with retries, timeouts, and circuit breakers.
- **OpenTelemetry**: Tracks distributed traces and collects metrics for observability.
- **Azure Monitor**: Provides centralized monitoring and diagnostics.

## **Architecture Diagram**

```plaintext
[Client] --> [Azure Front Door] --> [Minimal API]
                                |--> [Azure Redis Cache]
                                |--> [Azure SQL Database]
                                |--> [Azure Monitor + OpenTelemetry]
```

## **Summary Steps**
1. Deploy the Minimal API to **Azure Container Apps** for scalability and cost efficiency.
2. Integrate **OpenTelemetry** for distributed tracing and metrics collection.
3. Configure **HybridCache** to improve performance with in-memory and distributed caching.
4. Secure APIs using **Azure Front Door** for traffic routing and DDoS protection.
5. Monitor and optimize using **Azure Monitor** and **Application Insights**.

## **Links and References**

Below is a list of links to all the tools, frameworks, and resources mentioned in this document for quick access:

### **ASP.NET Core 9 and Related Resources**
- [ASP.NET Core 9 Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [Minimal APIs Overview](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)

### **OpenAPI and Scaler**
- [Scaler - API Documentation Hosting](https://scalar.com/)
- [OpenAPI Specification](https://swagger.io/specification/)

### **Reliability**
- [Polly - Resilience Framework](https://github.com/App-vNext/Polly)
- [Rate Limiting in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/performance/rate-limiting)
- [EF Core Retry Logic](https://learn.microsoft.com/en-us/ef/core/what-is-new/ef-core-6.0#retry-on-failure)

### **Security**
- [Azure Front Door](https://learn.microsoft.com/en-us/azure/frontdoor/)
- [Azure Active Directory (Azure AD)](https://learn.microsoft.com/en-us/azure/active-directory/)
- [Azure Key Vault](https://learn.microsoft.com/en-us/azure/key-vault/)

### **Performance Efficiency**
- [HybridCache in .NET 9](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [OpenTelemetry for .NET](https://opentelemetry.io/docs/instrumentation/net/)
- [Azure Monitor](https://learn.microsoft.com/en-us/azure/azure-monitor/)

### **Observability**
- [HTTP Logging Middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-logging)
- [Azure Monitor Trace Exporter](https://learn.microsoft.com/en-us/azure/azure-monitor/app/opentelemetry-overview)

### **Cost Optimization**
- [Azure Container Apps](https://learn.microsoft.com/en-us/azure/container-apps/)
- [Docker for Containerization](https://www.docker.com/)

### **Azure Well-Architected Framework**
- [Azure Well-Architected Framework Overview](https://learn.microsoft.com/en-us/azure/architecture/framework/)

These links provide detailed documentation and resources to implement the concepts and tools discussed in this guide effectively.
