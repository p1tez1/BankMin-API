using BankMin_API.Features.AccountUseCase;
using BankMin_API.Features.Auth;
using BankMin_API.Infrastructure;
using BankMin_API.Infrastructure.Repositories;
using BankMin_API.Services;
using BankMin_API.Services.IServices;
using BankMin_API.Startup;
using BankMin_API.Startup.Middlewares;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<BankDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
});

builder.Services.AddScoped<IHashService, HashService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IJWTGenerator, JWTGenerator>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IAccountRepo, AccountRepo>();
builder.Services.AddScoped<IMoneyTransferService, MoneyTransferService>();

builder.JwtConfigure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthorization();

app.MapControllers();

//minimal APIs
app.MapAuthEndpoints();
app.MapAccountEndpoints();
//minimal APIs

app.Run();
