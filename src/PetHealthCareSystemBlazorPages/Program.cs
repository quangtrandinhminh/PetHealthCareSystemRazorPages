using System;
using BusinessObject.Entities;
using BusinessObject.Entities.Identity;
using BusinessObject.Mapper;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetHealthCareSystemRazorPages.Middlewares;
using Repository;
using Repository.Interfaces;
using Repository.Repositories;
using Serilog;
using Service.IServices;
using Service.Services;
using Utility.Config;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
builder.Host.UseSerilog((context, config) =>
{
    config.ReadFrom.Configuration(context.Configuration);
});

// Add DbContext
builder.Services.AddDbContext<AppDbContext>();

// Load system settings from appsettings.json
var systemSettingModel = new SystemSettingModel();
builder.Configuration.GetSection("SystemSetting").Bind(systemSettingModel);
SystemSettingModel.Instance = systemSettingModel;

var vnPaySetting = new VnPaySetting();
builder.Configuration.GetSection("VnPaySetting").Bind(vnPaySetting);
VnPaySetting.Instance = vnPaySetting;

var mailSettingModel = new MailSettingModel();
builder.Configuration.GetSection("MailSetting").Bind(mailSettingModel);
MailSettingModel.Instance = mailSettingModel;

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddScoped<MapperlyMapper>();

// Register Repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ICageRepository, CageRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IMedicalItemRepository, MedicalItemRepository>();
builder.Services.AddScoped<IMedicalRecordRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IHospitalizationRepository, HospitalizationRepository>();
builder.Services.AddScoped<ITimeTableRepository, TimeTableRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAppointmentPetRepository, AppointmentPetRepository>();
builder.Services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Register Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IHospitalizationService, HospitalizationService>();
builder.Services.AddScoped<IMedicalService, MedicalService>();
builder.Services.AddScoped<IPetService, PetService>();
builder.Services.AddScoped<IService, ServiceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IConfigurationService, ConfigurationService>();
builder.Services.AddScoped<IVnPayService, VnPayService>();

// Configure Identity
builder.Services.AddIdentity<UserEntity, RoleEntity>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication(); // Ensure this is added if you are using authentication
app.UseAuthorization();

// Add your custom error handling middleware
app.UseMiddleware<ErrorHandlerMiddleware>(Log.Logger);

app.MapRazorPages();
app.Run();
