using Asp.Versioning;
using ConversionAPI.Application.Interfaces;
using ConversionAPI.Application.Services;
using ConversionAPI.Domain.Interfaces;
using ConversionAPI.Infrastructure;
using ConversionAPI.Infrastructure.ExternalAPIs;
using DotNetCrudWebApi;
using DotNetCrudWebApi.Data;
using DotNetCrudWebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;
using System.Text;


// Serilog Setup for Logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.File(new CompactJsonFormatter(), "log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();
var builder = WebApplication.CreateBuilder(args);


try
{
    Log.Information("Starting CurrencyConverter API...");

    // Configure Services
    builder.Services.ConfigureServices(builder.Configuration);


    var app = builder.Build();

    // Setup Middleware
    app.ConfigureMiddleware();

    // Setup Swagger in Development
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();
    app.Run();


    //try
    //{
    //    Log.Information("Starting CurrencyConverter API...");
    //    builder.Host.ConfigureSerilog();
    //    builder.Services.AddDbContext<AppDbContext>();
    //    builder.Services.AddHttpClient();
    //    builder.Services.AddControllers();

    //    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    //    builder.Services.AddEndpointsApiExplorer();
    //    //builder.Services.AddSwaggerGen();
    //    //builder.Services.AddSwaggerGen(c =>
    //    //{
    //    //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CurrencyConverter API", Version = "v1" });
    //    //});

    //    builder.Services.AddSwaggerGen(c =>
    //    {
    //        c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    //        {
    //            Name = "Authorization",
    //            Type = SecuritySchemeType.Http,
    //            Scheme = "Bearer",
    //            BearerFormat = "JWT",
    //            In = ParameterLocation.Header,
    //            Description = "Enter 'Bearer <your-token>'"
    //        });

    //        c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //    {
    //        {
    //            new OpenApiSecurityScheme
    //            {
    //                Reference = new OpenApiReference
    //                {
    //                    Type = ReferenceType.SecurityScheme,
    //                    Id = "Bearer"
    //                }
    //            },
    //            new List<string>()
    //        }
    //    });
    //    });

    //    builder.Services.AddMemoryCache();

    //    builder.Services.AddAuthentication(x =>
    //    {
    //        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    //    })
    //  .AddJwtBearer(options =>
    //  {
    //      options.Authority = "https://localhost:5001"; // Match Authority with ValidIssuer
    //      options.TokenValidationParameters = new TokenValidationParameters
    //      {
    //          ValidateIssuer = true,
    //          ValidIssuer = "https://localhost:5001",
    //          ValidAudience = "https://localhost:5001",
    //          ValidateAudience = true,
    //          ValidateLifetime = true,
    //          ValidateIssuerSigningKey = true,
    //          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("AdcbSecretKey@12345678901234567890123456789012")) // Ensure key is 32+ characters
    //      };
    //  });

    //    builder.Services.AddAuthorization(options =>
    //    {
    //        options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
    //        options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    //    });


    //    builder.Logging
    //        .ClearProviders()
    //        .AddConsole()
    //        .AddDebug()
    //        .AddOpenTelemetry(opt =>
    //        {
    //            opt.AddConsoleExporter()
    //            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("CurrencyService.Net"));
    //            opt.IncludeFormattedMessage = true;
    //            opt.IncludeScopes = true;

    //        });

    //    builder.Services.AddOpenTelemetry()
    //           //.WithMetrics(metricsBuilder => metricsBuilder
    //           //    .AddAspNetCoreInstrumentation()
    //           //    .AddHttpClientInstrumentation()
    //           //    .AddConsoleExporter())
    //           .WithTracing(tracing => tracing
    //               .AddAspNetCoreInstrumentation()
    //               .AddHttpClientInstrumentation()
    //               .AddSource("DotNetCrudWebApi")
    //               .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("Tracing.Net"))
    //               .AddConsoleExporter());

    //    builder.Services.AddHttpClient("Frankfurter", client =>
    //    {
    //        client.BaseAddress = new Uri("https://api.frankfurter.app/");
    //    })
    //    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
    //    .AddPolicyHandler(PolicyFactoryExtension.GetRetryPolicy())
    //    .AddPolicyHandler(PolicyFactoryExtension.GetCircuitBreakerPolicy());


    //    builder.Services.AddScoped<ICurrencyProvider, FrankfurterAPI>();
    //    builder.Services.AddScoped<FrankfurterAPI>();
    //    builder.Services.AddScoped<ICurrencyProviderFactory, CurrencyProviderFactory>();
    //    builder.Services.AddScoped<ICurrencyService, CurrencyService>();

    //    builder.Services.AddApiVersioning(options =>
    //    {
    //        options.DefaultApiVersion = new ApiVersion(1, 0);
    //        options.AssumeDefaultVersionWhenUnspecified = true;
    //        options.ReportApiVersions = true;
    //        options.ApiVersionReader = ApiVersionReader.Combine(
    //            new UrlSegmentApiVersionReader(),
    //            new HeaderApiVersionReader("X-Api-Version")
    //        );
    //    })
    //  .AddApiExplorer(options =>
    //  {
    //      options.GroupNameFormat = "'v'VVV";
    //      options.SubstituteApiVersionInUrl = true;
    //  });


    //    // 🔹 API Rate Limiting
    //    builder.Services.AddRateLimiter(options =>
    //    {
    //        options.AddFixedWindowLimiter(policyName: "Fixed", configureOptions: opt =>
    //        {
    //            opt.PermitLimit = 100;
    //            opt.Window = TimeSpan.FromMinutes(1);
    //            opt.QueueProcessingOrder = System.Threading.RateLimiting.QueueProcessingOrder.OldestFirst;
    //            opt.QueueLimit = 2;
    //        });
    //    });

    //    var app = builder.Build();

    //    app.UseAuthentication();
    //    app.UseAuthorization();
    //    app.UseRateLimiter();

    //    // Configure the HTTP request pipeline.
    //    if (app.Environment.IsDevelopment())
    //    {
    //        app.UseSwagger();
    //        app.UseSwaggerUI();
    //    }
    //    app.UseForwardedHeaders();
    //    app.UseMiddleware<ExceptionHandlingMiddleware>();
    //    app.UseHttpsRedirection();

    //    // 🔹 Logging Middleware for API Requests
    //    app.Use(async (context, next) =>
    //    {
    //        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    //        var watch = System.Diagnostics.Stopwatch.StartNew();
    //        await next.Invoke();
    //        watch.Stop();
    //        var ip = context.Connection.RemoteIpAddress?.ToString();
    //        var user = context.User?.Identity?.Name ?? "anonymous";

    //        logger.LogInformation("{Method} {Path} responded {StatusCode} in {Elapsed} ms from {IP} (User: {User})",
    //            context.Request.Method,
    //            context.Request.Path,
    //            context.Response.StatusCode,
    //            watch.ElapsedMilliseconds,
    //            ip,
    //            user);
    //    });




    //    app.MapControllers();

    //    app.Run();


}
catch (Exception ex)
{
    Log.Fatal(ex, "Application start-up failed");
}
finally
{
    Log.CloseAndFlush();
}
