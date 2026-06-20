using Horizon.API.Filters;
using Horizon.API.Middleware;
using Horizon.Application.Bootstrap;
using Horizon.Infrastructure.BackgroundJobs;
using Horizon.Infrastructure.Bootstrap;
using Horizon.Infrastructure.Data;
using Horizon.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────
// Controllers
// ─────────────────────────────────────────────────────────────

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
})
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy =
    System.Text.Json.JsonNamingPolicy.CamelCase;


options.JsonSerializerOptions.DefaultIgnoreCondition =
    System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;


});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddEndpointsApiExplorer();

// ─────────────────────────────────────────────────────────────
// OpenAPI
// ─────────────────────────────────────────────────────────────

builder.Services.AddOpenApi();

// ─────────────────────────────────────────────────────────────
// Application & Infrastructure
// ─────────────────────────────────────────────────────────────

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ─────────────────────────────────────────────────────────────
// Caching
// ─────────────────────────────────────────────────────────────

builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

// ─────────────────────────────────────────────────────────────
// JWT Authentication
// ─────────────────────────────────────────────────────────────

var jwtSecret = builder.Configuration["Jwt:Secret"];

if (string.IsNullOrWhiteSpace(jwtSecret))
{
    throw new InvalidOperationException(
    "Jwt:Secret is missing from configuration.");
}

builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.SaveToken = true;


    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret)),

        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception is SecurityTokenExpiredException)
            {
                context.Response.Headers.Append(
                    "Token-Expired",
                    "true");
            }

            return Task.CompletedTask;
        }
    };
});


// ─────────────────────────────────────────────────────────────
// Authorization
// ─────────────────────────────────────────────────────────────

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
    "Admin",
    policy => policy.RequireRole("Admin"));


options.AddPolicy(
    "Instructor",
    policy => policy.RequireRole("Instructor", "Admin"));

    options.AddPolicy(
        "Student",
        policy => policy.RequireRole(
            "Student",
            "Instructor",
            "Admin"));


});

// ─────────────────────────────────────────────────────────────
// CORS
// ─────────────────────────────────────────────────────────────

builder.Services.AddCors(options =>
{
    // Development — allow any origin so the frontend dev server works freely.
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());

    // Production — restrict to origins declared in appsettings.
    options.AddPolicy("ProductionPolicy", policy =>
        policy.WithOrigins(
                builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
                    ?? new[] { "https://horizon.com" })
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

// ─────────────────────────────────────────────────────────────
// Rate Limiting
// ─────────────────────────────────────────────────────────────

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    // ── Global default ─────────────────────────────────────────────────────
    // Applied automatically to every endpoint; [EnableRateLimiting("auth")]
    // etc. override this with stricter named policies where needed.
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(ctx =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ctx.User.Identity?.Name
                          ?? ctx.Connection.RemoteIpAddress?.ToString()
                          ?? "anon",
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit          = 100,
                Window               = TimeSpan.FromMinutes(1),
                QueueLimit           = 10,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            }));

    // ── Auth — brute-force protection ─────────────────────────────────────
    options.AddFixedWindowLimiter("auth", o =>
    {
        o.PermitLimit          = 10;
        o.Window               = TimeSpan.FromMinutes(15);
        o.QueueLimit           = 0;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // ── File uploads ──────────────────────────────────────────────────────
    options.AddFixedWindowLimiter("upload", o =>
    {
        o.PermitLimit          = 20;
        o.Window               = TimeSpan.FromHours(1);
        o.QueueLimit           = 5;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // ── Search / browse ───────────────────────────────────────────────────
    options.AddFixedWindowLimiter("search", o =>
    {
        o.PermitLimit          = 60;
        o.Window               = TimeSpan.FromMinutes(1);
        o.QueueLimit           = 5;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    // ── Stripe webhook — high limit (signature is the real auth) ─────────
    options.AddFixedWindowLimiter("webhook", o =>
    {
        o.PermitLimit          = 500;
        o.Window               = TimeSpan.FromMinutes(1);
        o.QueueLimit           = 50;
        o.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

// ─────────────────────────────────────────────────────────────
// Upload Limits
// ─────────────────────────────────────────────────────────────

builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 500 * 1024 * 1024;
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize =
    500 * 1024 * 1024;
});

// ─────────────────────────────────────────────────────────────
// Health Checks
// ─────────────────────────────────────────────────────────────

builder.Services.AddHealthChecks()
.AddDbContextCheck<ApplicationDbContext>("database");

// ─────────────────────────────────────────────────────────────
// Background Jobs
// ─────────────────────────────────────────────────────────────

builder.Services.AddHostedService<EnrollmentExpiryJob>();
builder.Services.AddHostedService<LiveSessionReminderJob>();
builder.Services.AddHostedService<AnalyticsAggregationJob>();
builder.Services.AddHostedService<PayoutProcessingJob>();
builder.Services.AddHostedService<SessionCleanupJob>();

// ─────────────────────────────────────────────────────────────
// Build
// ─────────────────────────────────────────────────────────────

var app = builder.Build();

// ─────────────────────────────────────────────────────────────
// Middleware
// ─────────────────────────────────────────────────────────────

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

app.MapOpenApi();

app.MapScalarApiReference("/scalar", options =>
{
    options.Title = "Horizon API";
    options.Theme = ScalarTheme.DeepSpace;
});

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseResponseCaching();
app.UseCors(app.Environment.IsDevelopment() ? "AllowAll" : "ProductionPolicy");

app.UseRateLimiter();

app.UseMiddleware<CurrentUserMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

// ─────────────────────────────────────────────────────────────
// Database Migration & Seeding
// ─────────────────────────────────────────────────────────────

using (var scope = app.Services.CreateScope())
{
    var logger =
    scope.ServiceProvider.GetRequiredService<ILogger<Program>>();


try
    {
        var context =
            scope.ServiceProvider
                 .GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        await SeedData.InitializeAsync(
            scope.ServiceProvider);
    }
    catch (Exception ex)
    {
        logger.LogError(
            ex,
            "Startup failed during migration/seeding.");

        throw;
    }
}

app.Run();
