using FitnessOperationsApi.Data;
using FitnessOperationsApi.Repositories;
using FitnessOperationsApi.Repositories.Access;
using FitnessOperationsApi.Repositories.Branches;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Repository
builder.Services.AddScoped<IBranchRepository, BranchRepository>();

// Register MemberBranchAccessRepository Repository
builder.Services.AddScoped<IMemberBranchAccessRepository, MemberBranchAccessRepository>();

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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
