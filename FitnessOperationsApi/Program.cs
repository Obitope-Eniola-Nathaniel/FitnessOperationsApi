using AspNetCoreRateLimit;
using FitnessOperationsApi.Configuration;
using FitnessOperationsApi.Data;
using FitnessOperationsApi.Middleware;
using FitnessOperationsApi.Repositories.Access;
using FitnessOperationsApi.Repositories.Auth;
using FitnessOperationsApi.Repositories.Branches;
using FitnessOperationsApi.Repositories.Members;
using FitnessOperationsApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// JWT
var Jwt = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(Jwt);

var settings = Jwt.Get<JwtSettings>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
            };
    });

builder.Services.AddAuthorization();

// Register Cloudinary
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IFileUploadService,FileUploadService>();

// Resgister Token Service
builder.Services.AddScoped<ITokenService, TokenService>();

// Resgister Refresh Token Service
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Register Auth Repository
builder.Services.AddScoped<IAuthRepository, AuthRepository>();

// Add services to the container.
builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Rate Limit
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Branch Repository
builder.Services.AddScoped<IBranchRepository, BranchRepository>();

// Register Member Repository
builder.Services.AddScoped<IMemberRepository, MemberRepository>();

// Register MemberBranchAccessRepository Repository
builder.Services.AddScoped<IMemberBranchAccessRepository, MemberBranchAccessRepository>();

// Logging
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

// Register Dapper
builder.Services.AddSingleton<DapperContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Calling the Seed
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await DbSeeder.Seed(context);
}



app.UseHttpsRedirection();
app.UseIpRateLimiting();

app.UseMiddleware<IpWhitelistMiddleware>();
app.UseMiddleware<ApiKeyMiddleware>();
// Global Error Handdleing
app.UseMiddleware<GlobalExceptionMiddleware>();





// JWT Pipeline
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
