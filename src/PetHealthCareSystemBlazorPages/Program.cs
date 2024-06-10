using System;
using BusinessObject.Entities;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Repository;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(config => { config.ReadFrom.Configuration(builder.Configuration); });
// Add database connection
/*builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));*/

// Add services to the container.

// config root directory for razor pages
builder.Services.AddRazorPages().AddRazorPagesOptions(opt => opt.RootDirectory= "/Pages");

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
