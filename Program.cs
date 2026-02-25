using BaseBackend.Application.Services;
using BaseBackend.Domain.Interfaces;
using BaseBackend.Infrastructure.Persistence;
using BaseBackend.Infrastructure.Security;
using BaseBackend.Infrastructure.Persistence.Repositories;
using BaseBackend.Api.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// 1. DATABASE
// ----------------------
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ----------------------
// 2. CORS (PERMITIR ANGULAR)
// ----------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularDev", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL de tu frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ----------------------
// 3. DEPENDENCY INJECTION
// ----------------------
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<CategoryService>();

// ----------------------
// 4. JWT AUTHENTICATION
// ----------------------
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey)) throw new Exception("JWT Key is missing in configuration");

var key = Encoding.UTF8.GetBytes(jwtKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.AddAuthorization();

// ----------------------
// 5. CONTROLLERS + SWAGGER
// ----------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// ==============================
// ✅ MIGRACIONES AUTOMÁTICAS
// ==============================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

// ----------------------
// 6. MIDDLEWARE PIPELINE
// ----------------------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ✅ CORS debe ir ANTES de Authentication y Authorization
app.UseCors("AllowAngularDev"); 

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// ==============================
// ✅ LOG DE ENDPOINTS EN CONSOLA
// ==============================
app.Lifetime.ApplicationStarted.Register(() =>
{
    var endpointDataSource = app.Services.GetRequiredService<EndpointDataSource>();
    var endpoints = endpointDataSource.Endpoints
        .OfType<RouteEndpoint>()
        .Where(e => e.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>() != null)
        .ToList();

    var grouped = endpoints
        .GroupBy(e => e.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>()!.ControllerName)
        .OrderBy(g => g.Key);

    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("\n🚀 Registered Endpoints by Module:\n");

    foreach (var group in grouped)
    {
        Console.WriteLine($"===== {group.Key.ToUpper()} =====");
        foreach (var endpoint in group)
        {
            var descriptor = endpoint.Metadata.GetMetadata<Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor>();
            var httpMethod = descriptor!.ActionConstraints?.OfType<Microsoft.AspNetCore.Mvc.ActionConstraints.HttpMethodActionConstraint>()
                .FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "HTTP";

            Console.WriteLine($"{httpMethod.PadRight(6)} /{endpoint.RoutePattern.RawText}");
        }
        Console.WriteLine();
    }
    Console.ResetColor();
});

app.Run();