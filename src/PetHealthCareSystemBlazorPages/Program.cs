using System;
using BusinessObject.Entities;
using BusinessObject.Mapper;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(config => { config.ReadFrom.Configuration(builder.Configuration); });
builder.Services.AddDbContext<AppDbContext>();

// Add services to the container.

// config root directory for razor pages
builder.Services.AddRazorPages();

builder.Services.AddScoped<MapperlyMapper>();
// Repository
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
// Service
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IService, ServiceService>();
builder.Services.AddScoped<IPetService, PetService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
