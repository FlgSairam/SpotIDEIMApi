using DapperAuthApi.DBContext;
using DapperAuthApi.Interfaces;
using DapperAuthApi.Repositories;
using DapperAuthApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<EmployeeRepository>();
builder.Services.AddScoped<EmployeeAttendanceRepository>();
builder.Services.AddScoped<EmployeePerformanceRepository>();
builder.Services.AddScoped<AppUserRepository>();
builder.Services.AddScoped<CustomerRepository>();
builder.Services.AddScoped<UserCustomerMappingRepository>();

builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<DapperDbContext>();
builder.Services.AddTransient<IPushNotification, PushNotificationRepository>();
builder.Services.AddScoped<IAttendanceRepository, PushNotificationRepository>();
builder.Services.AddHostedService<AttendanceReminderService>();
builder.Services.AddScoped<ISupervisor, SupervisorRepository>();

// JWT Auth
var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SpotIDEIM API",
        Version = "v1"
    });

    // Optional: Add support for JWT token in Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer {token}')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
