﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Serilog;
using Serilog.Events;
using Api_Sport_Business_Logic.Services.Interfaces;
using Api_Sport_Business_Logic.Services;
using Api_Sport_Business_Logic.Models.Mapping;
using Api_Sport_Business_Logic_Business_Logic.Services.Interfaces;
using Api_Sport_Business_Logic_Business_Logic.Services;
using Api_Sport_DataLayer_DataLayer.Models;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<SportDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMatcheService, MatchService>();
builder.Services.AddScoped<IAuthHelperService, AuthHelperService>();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Authentication:Issuer"],
            ValidAudience = builder.Configuration["Authentication:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration["Authentication:SecretForKey"]))
        };
    }
    );
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200") // آدرس پروژه Angular
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)  // تنظیمات را از appsettings.json می‌خوانیم
    .CreateLogger();

builder.Host.UseSerilog();

Log.Information("Starting web application");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAngularClient"); // این باید قبل از UseAuthorization باشه

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();


app.MapControllers();

app.Run();
