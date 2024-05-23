using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SageERP.Middleware;
using Shampan.Core.Interfaces.Services;
using Shampan.Models;
using Shampan.Services;
using Shampan.UnitOfWork.SqlServer;
using ShampanERP.Configuration;
using ShampanERP.Configuration.ServiceRegistration;
using ShampanERP.Models;
using ShampanERP.Persistence;
using SSLAudit.Models;
using UnitOfWork.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	//options.IdleTimeout = TimeSpan.FromSeconds(10); 

});


builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthContext")));

builder.Services.AddAuthServices(builder.Configuration);

builder.Services
    .AddDbServices()
    .AddAutoMapperServices();

builder.Services.AddExceptional(builder.Configuration.GetSection("Exceptional"));


// static void ConfigureServices(IServiceCollection services)
//{
//    services.AddSession(options =>
//    {
//        options.IdleTimeout = TimeSpan.FromMinutes(30);
//    });
//}
// static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
//{
//    app.UseSession();
//}

var app = builder.Build();


app.UseExceptional();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<DBMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
