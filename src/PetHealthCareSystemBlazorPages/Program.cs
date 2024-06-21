using System;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Services;
using Utility.Config;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog(config => { config.ReadFrom.Configuration(builder.Configuration); });
builder.Services.AddDbContext<AppDbContext>();
// Add system setting from appsettings.json
var systemSettingModel = new SystemSettingModel();
builder.Configuration.GetSection("SystemSetting").Bind(systemSettingModel);
SystemSettingModel.Instance = systemSettingModel;

var vnPaySetting = new VnPaySetting();
builder.Configuration.GetSection("VnPaySetting").Bind(vnPaySetting);
VnPaySetting.Instance = vnPaySetting;
// Add services to the container.

// config root directory for razor pages
builder.Services.AddRazorPages();

builder.Services.AddScoped<MapperlyMapper>();
// Repository
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ICageRepository, CageRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IMedicalItemRepository, MedicalItemRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IHospitalizationRepository, HospitalizationRepository>();
builder.Services.AddScoped<ITimeTableRepository, TimeTableRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentPetRepository, AppointmentPetRepository>();
// Service
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHospitalizationService, HospitalizationService>();
builder.Services.AddScoped<IMedicalItemService, MedicalItemService>();
builder.Services.AddScoped<IMedicalService, MedicalService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IService, ServiceService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddSession(
    options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(10);
    });
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
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
